using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Infra.Wpf.Common.Helpers
{
    public class AttachImage : DependencyObject
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached("Image", typeof(ImageSource), typeof(AttachImage), new PropertyMetadata(null));

        public static ImageSource GetImage(DependencyObject obj)
        {
            return (ImageSource) obj.GetValue(ImageProperty);
        }

        public static void SetImage(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageProperty, value);
        }

        public static readonly DependencyProperty ImageOverProperty = DependencyProperty.RegisterAttached("ImageOver", typeof(ImageSource), typeof(AttachImage), new PropertyMetadata(null));

        public static ImageSource GetImageOver(DependencyObject obj)
        {
            return (ImageSource) obj.GetValue(ImageOverProperty);
        }

        public static void SetImageOver(DependencyObject obj, ImageSource value)
        {
            obj.SetValue(ImageOverProperty, value);
        }
    }
}
