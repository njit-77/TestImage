using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TestImage
{
    class StringToImageSourceConverter : IValueConverter
    {

        static StringToImageSourceConverter()
        {
            Instance = new StringToImageSourceConverter();
        }

        public static StringToImageSourceConverter Instance { get; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string path = (string)value;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            return new BitmapImage(new Uri(path));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return false;
        }
    }
}
