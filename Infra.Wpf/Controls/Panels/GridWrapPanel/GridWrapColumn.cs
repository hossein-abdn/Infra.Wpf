using Infra.Wpf.Common;
using System.Collections.Generic;

namespace Infra.Wpf.Controls
{
    public class GridWrapColumn
    {
        private List<RectItem> rectList { get; set; }

        public double MaxWidth { get; private set; }

        public double TotalHeight
        {
            get
            {
                double result = 0;
                foreach (var item in rectList)
                    result += item.Height;

                return result;
            }
        }

        public GridWrapColumn()
        {
            rectList = new List<RectItem>();
            MaxWidth = 0;
        }

        public void AddRect(RectItem rect)
        {
            rectList.Add(rect);
            if (MaxWidth < rect.Width)
                MaxWidth = rect.Width;

            EqualizeWidth();
        }

        public RectItem Get(int index)
        {
            try
            {
                return rectList[index];
            }
            catch
            {
                return null;
            }
        }

        public void EqualizeWidth()
        {
            foreach (var item in rectList)
                item.Width = MaxWidth;
        }

        public void Arrange(double restWidth)
        {
            double offset = 0;
            foreach (var item in rectList)
            {
                item.Y = offset;
                offset += item.Height;
                item.Width += restWidth;
            }
        }
    }
}
