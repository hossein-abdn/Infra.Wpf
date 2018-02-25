using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Infra.Wpf.Controls
{
    public class ImageButton : Button
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        public ImageButton()
        {
            Loaded += ImageButton_Loaded;
        }

        private void ImageButton_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (Image == null)
                MouseOverImage = null;
            
            else
            {
                Image image = (Image)Template.FindName("image", this);
                image.Margin = new Thickness(0, 0, 7, 0);
                if (MouseOverImage == null)
                    MouseOverImage = Image;
            }
        }

        public ImageSource Image
        {
            get { return (ImageSource) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public ImageSource MouseOverImage
        {
            get { return (ImageSource) GetValue(MouseOverImageProperty); }
            set { SetValue(MouseOverImageProperty, value); }
        }

        public static readonly DependencyProperty MouseOverImageProperty =
            DependencyProperty.Register("MouseOverImage", typeof(ImageSource), typeof(ImageButton), new PropertyMetadata(null));

        public double ImageSize
        {
            get { return (double) GetValue(ImageSizeProperty); }
            set { SetValue(ImageSizeProperty, value); }
        }

        public static readonly DependencyProperty ImageSizeProperty =
            DependencyProperty.Register("ImageSize", typeof(double), typeof(ImageButton), new PropertyMetadata(12d));
    }
}
