using Infra.Wpf.Common;
using System.Collections.Generic;

namespace Infra.Wpf.Controls
{
    public class SearchGridWrapRow
    {
        private List<RectItem> rectList { get; set; }

        public IEnumerable<RectItem> RectList
        {
            get
            {
                return rectList;
            }
        }

        public double MaxHeight { get; private set; }

        public double TotalWidth
        {
            get
            {
                double result = 0;
                foreach (var item in rectList)
                    result += item.Width;

                return result;
            }
        }

        public SearchGridWrapRow()
        {
            rectList = new List<RectItem>();
            MaxHeight = 0;
        }

        public void AddRect(RectItem rect)
        {
            rectList.Add(rect);
            if (MaxHeight < rect.Height)
                MaxHeight = rect.Height;

            EqualizeHeight();
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

        public void EqualizeHeight()
        {
            foreach (var item in rectList)
                item.Height = MaxHeight;
        }

        public void Arrange(double restHeight)
        {
            double offset = 0;
            foreach (var item in rectList)
            {
                item.X = offset;
                item.Y += restHeight;
                offset += item.Width;
            }
        }
    }
}
