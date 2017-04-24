using Infra.Wpf.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public class GridWrapPanel : Panel
    {
        #region Properties
        private List<Size> desiredList;

        private List<GridWrapColumn> columnList { get; set; }

        private List<GridWrapRow> rowList { get; set; }

        public bool Stretch
        {
            get { return (bool) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch", typeof(bool), typeof(GridWrapPanel),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits));

        #endregion

        #region Methods
        public GridWrapPanel()
        {
            columnList = new List<GridWrapColumn>();
            rowList = new List<GridWrapRow>();
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
                return new Size(width, height);
            }
            else
            {
                int columnCount = CalculateColumnCount(availableSize.Width);
                ArrangeItems(columnCount, availableSize);
                Size result = new Size(availableSize.Width, 0);
                if (columnList.Count > 0)
                    result.Height = columnList.First().TotalHeight;

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
                restWidth = (finalSize.Width - rowList.First().TotalWidth) / columnList.Count;

            foreach (var item in columnList)
                item.Arrange(restWidth);

            foreach (var item in rowList)
                item.Arrange();

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
                if (totalWidth > width)
                    break;
                column++;
            }
            if (column == 0)
                column = 1;

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
                    rowList.Add(new GridWrapRow());
                if (columnList.Count < columnCount)
                    columnList.Add(new GridWrapColumn());

                RectItem rect = new RectItem(0, 0, desiredList[desiredIndex]);
                rowList[row].AddRect(rect);
                columnList[column].AddRect(rect);

                if (rowList.First().TotalWidth > finalSize.Width && columnCount != 1)
                {
                    column = 0;
                    row = 0;
                    desiredIndex = -1;
                    columnCount--;
                    rowList.RemoveAll(x => true);
                    columnList.RemoveAll(x => true);
                    continue;
                }

                column++;
                if (column == columnCount)
                {
                    column = 0;
                    row++;
                }
            }
        }

        #endregion
    }
}