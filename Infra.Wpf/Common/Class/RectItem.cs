using System.Windows;

namespace Infra.Wpf.Common
{
    public class RectItem
    {
        public double X { get; set; }

        public double Y { get; set; }

        public double Height { get; set; }

        public double Width { get; set; }

        public RectItem(double x, double y, double height, double width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
        }

        public RectItem(Point location, Size size) : this(location.X,location.Y,size.Height, size.Width)
        {
        }

        public RectItem(double x, double y, Size size) : this(x, y, size.Height, size.Width)
        {
        }
    }
}
