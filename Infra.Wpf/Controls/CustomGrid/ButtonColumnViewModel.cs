using System.Windows.Media;

namespace Infra.Wpf.Controls
{
    public class ButtonColumnViewModel
    {
        public ImageSource Image { get; set; }

        private ImageSource _MouseOverImage;
        public ImageSource MouseOverImage 
        { 
            get
            {
                return _MouseOverImage ?? Image;
            }
            set
            {
                _MouseOverImage = value;
            }
        }
    }
}
