using Infra.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    internal class SearchGridWrapPanel : Panel
    {
        #region Properties

        private List<Size> desiredList;

        private List<SearchGridWrapColumn> columnList { get; set; }

        private List<SearchGridWrapRow> rowList { get; set; }

        public bool Stretch
        {
            get { return (bool) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(bool), typeof(SearchGridWrapPanel),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double RowMargin
        {
            get { return (double) GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        public static readonly DependencyProperty RowMarginProperty = DependencyProperty.Register("RowMargin", typeof(double), typeof(SearchGridWrapPanel),
                new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public double ColumnMargin
        {
            get { return (double) GetValue(ColumnMarginProperty); }
            set { SetValue(ColumnMarginProperty, value); }
        }

        public static readonly DependencyProperty ColumnMarginProperty = DependencyProperty.Register("ColumnMargin", typeof(double), typeof(SearchGridWrapPanel),
            new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        #endregion

        #region Methods

        public SearchGridWrapPanel()
        {
            columnList = new List<SearchGridWrapColumn>();
            rowList = new List<SearchGridWrapRow>();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            desiredList = new List<Size>();
            foreach (UIElement item in Children)
            {
                if (item != null)
                {
                    item.Measure(availableSize);
                    desiredList.Add(item.DesiredSize);
                }
            }

            if (!double.IsInfinity(availableSize.Width) && !double.IsInfinity(availableSize.Height))
                return availableSize;
            else if (double.IsInfinity(availableSize.Width))
            {
                double height = 0;
                double width = 0;
                foreach (var item in desiredList)
                {
                    height = Math.Max(height, item.Height);
                    width += item.Width;
                }
                if (desiredList.Count > 0)
                    width += ((desiredList.Count / 2) - 1) * ColumnMargin;
                return new Size(width, height);
            }
            else
            {
                int columnCount = CalculateColumnCount(availableSize.Width);
                ArrangeItems(columnCount, availableSize);
                Size result = new Size(availableSize.Width, 0);
                if (columnList.Count > 0)
                    result.Height = columnList.First().TotalHeight + ((rowList.Count - 1) * RowMargin);

                return result;
            }

            return availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int columnCount = CalculateColumnCount(finalSize.Width);
            ArrangeItems(columnCount, finalSize);

            double restWidth = 0;
            if (rowList.Count > 0 && Stretch == true)
                restWidth = (finalSize.Width - rowList.First().TotalWidth) / (columnList.Count / 2);

            int counter = 1;
            foreach (var item in columnList)
            {
                if (counter == 2)
                    item.Arrange(restWidth);
                else
                    item.Arrange(0);

                counter++;
                if (counter == 3)
                    counter = 1;
            }

            counter = 0;
            foreach (var item in rowList)
            {
                double margin = 0;
                if (!rowList.First().Equals(item))
                    margin = RowMargin * counter;
                item.Arrange(margin);
                counter++;
            }

            for (int i = 0; i < Children.Count; i++)
            {
                int row = i / columnList.Count;
                int column = i % columnList.Count;

                RectItem rectitem = rowList[row].Get(column);
                Rect rect = new Rect(rectitem.X, rectitem.Y, rectitem.Width, rectitem.Height);

                Children[i].Arrange(rect);
            }

            return finalSize;
        }

        private int CalculateColumnCount(double width)
        {
            int column = 0;
            double totalWidth = 0;

            foreach (var item in desiredList)
            {
                totalWidth += item.Width;
                double margin = 0;
                if (column != 0)
                    margin = ((column - 1) / 2) * ColumnMargin;
                if (totalWidth + margin > width)
                    break;
                column++;
            }

            if (column % 2 != 0)
                column--;

            if (column == 0)
                column = 2;

            return column;
        }

        private void ArrangeItems(int columnCount, Size finalSize)
        {
            rowList.RemoveAll(x => true);
            columnList.RemoveAll(x => true);
            int column = 0;
            int row = 0;

            for (int desiredIndex = 0; desiredIndex < desiredList.Count; desiredIndex++)
            {
                if (column == 0)
                    rowList.Add(new SearchGridWrapRow());

                if (columnList.Count < columnCount)
                    columnList.Add(new SearchGridWrapColumn());

                double margin = 0;
                if (column > 1 && column % 2 == 0)
                    margin = ColumnMargin;

                RectItem rect = new RectItem(0, 0, desiredList[desiredIndex].Height, desiredList[desiredIndex].Width + margin);
                rowList[row].AddRect(rect);
                columnList[column].AddRect(rect);

                if (rowList.First().TotalWidth > finalSize.Width && columnCount != 2)
                {
                    column = 0;
                    row = 0;
                    desiredIndex = -1;
                    columnCount -= 2;
                    rowList.RemoveAll(x => true);
                    columnList.RemoveAll(x => true);
                    continue;
                }

                column++;
                if (column == columnCount)
                {
                    column = 0;
                    margin = 0;
                    row++;
                }
            }
        }

        #endregion
    }
}