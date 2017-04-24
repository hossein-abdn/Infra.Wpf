using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestControls
{
    public class SimpleStackPanel : Panel
    {
        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(SimpleStackPanel), new FrameworkPropertyMetadata(Orientation.Vertical,FrameworkPropertyMetadataOptions.AffectsMeasure));

        protected override Size MeasureOverride(Size availableSize)
        {
            Size desiredSize = new Size();

            if (Orientation == Orientation.Vertical)
                availableSize.Height = double.PositiveInfinity;
            else
                availableSize.Width = double.PositiveInfinity;

            foreach (UIElement child in this.Children)
            {
                if(child != null)
                {
                    child.Measure(availableSize);
                    if(Orientation == Orientation.Vertical)
                    {
                        desiredSize.Width = Math.Max(desiredSize.Width, child.DesiredSize.Width);
                        desiredSize.Height += child.DesiredSize.Height;
                    }
                    else
                    {
                        desiredSize.Height = Math.Max(desiredSize.Height, child.DesiredSize.Height);
                        desiredSize.Width += child.DesiredSize.Width;
                    }
                }
            }
            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offset = 0;

            foreach (UIElement child in this.Children)
            {
                if(Orientation == Orientation.Vertical)
                {
                    child.Arrange(new Rect(0, offset, finalSize.Width, child.DesiredSize.Height));
                    offset += child.DesiredSize.Height;
                }
                else
                {
                    child.Arrange(new Rect(offset, 0, child.DesiredSize.Width, finalSize.Height));
                    offset += child.DesiredSize.Width;
                }
            }
            return finalSize;
        }
    }
}
