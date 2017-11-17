using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDisplayAttribute : Attribute
    {
        public EnumDisplayAttribute(string displayName) : this(displayName, "")
        {
        }

        public EnumDisplayAttribute(string displayName = "", string imageUri = "")
        {
            this.displayName = displayName;

            try
            {
                if (string.IsNullOrEmpty(imageUri) == false)
                    Image = new BitmapImage(new Uri(imageUri,UriKind.RelativeOrAbsolute));
            }
            catch
            {
            }
        }

        private readonly string displayName;

        public string DisplayName
        {
            get { return displayName; }
        }

        private ImageSource image;

        public ImageSource Image
        {
            private set { image = value; }
            get { return image; }
        }
    }
}
