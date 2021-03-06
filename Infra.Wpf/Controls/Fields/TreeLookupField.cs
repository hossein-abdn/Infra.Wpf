﻿using Infra.Wpf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public class TreeLookupField : TreeLookup, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }

        public Type ModelType { get; set; }

        public bool IsGetFocus
        {
            get { return (bool)GetValue(IsGetFocusProperty); }
            set { SetValue(IsGetFocusProperty, value); }
        }

        public static readonly DependencyProperty IsGetFocusProperty =
            DependencyProperty.Register("IsGetFocus", typeof(bool), typeof(TreeLookupField), new PropertyMetadata(false, OnIsGetFocusChanged));

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        public string SearchPhrase
        {
            get
            {
                if (SelectedItems == null)
                    return null;

                string query = "";
                foreach (var item in SelectedItems)
                {
                    string id = item?.GetType()?.GetProperty(IdColumn)?.GetValue(item)?.ToString();
                    if (string.IsNullOrEmpty(id))
                        return null;
                    query = query + $@"{FilterField}=={id.Trim()}" + " OR ";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Substring(0, query.Length - 4);
                    return query;
                }

                return null;
            }
        }

        #endregion

        #region Methods

        public TreeLookupField()
        {
            Loaded += LookupField_Loaded;
            SelectionChanged += LookupField_SelectionChanged;
            Focusable = true;
        }

        private void LookupField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchPhraseChanged?.Invoke();
        }

        private void LookupField_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();

            Binding binding = new Binding("IsFocused")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            SetBinding(IsGetFocusProperty, binding);
        }

        private static void OnIsGetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
                ((TreeLookup)d).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = null;
            if (SelectionMode == LookupSelectionMode.Single)
            {
                bindEx = BindingOperations.GetBindingExpression(this, SelectedItemProperty);
                if (bindEx == null || string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
                    bindEx = BindingOperations.GetBindingExpression(this, SelectedIdProperty);
            }
            else
            {
                bindEx = BindingOperations.GetBindingExpression(this, SelectedItemsProperty);
                if (bindEx == null || string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
                    bindEx = BindingOperations.GetBindingExpression(this, SelectedIdsProperty);
            }

            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                if (ModelType != null)
                {
                    var propInfo = ModelType.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var isRequired = propInfo.IsRequired();
                    var result = string.Empty;

                    if (attrib != null && attrib.Count() > 0)
                        result = ((DisplayAttribute)attrib[0]).Name;
                    else
                        result = bindEx.ResolvedSourcePropertyName;

                    if (!string.IsNullOrEmpty(result) && isRequired)
                        result = "* " + result;

                    return result;
                }
                else
                {
                    var result = bindEx.ResolvedSourcePropertyName;
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(FilterField))
            {
                var propInfo = ModelType?.GetProperty(FilterField);
                if (propInfo != null)
                {
                    var attrib = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attrib != null && attrib.Count() > 0)
                        return ((DisplayAttribute)attrib[0]).Name;
                }

                return FilterField;
            }

            return string.Empty;
        }

        public void Clear()
        {
            ClearLookup();
        }

        #endregion
    }
}