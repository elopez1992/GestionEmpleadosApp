using System.Globalization;
using GestionEmpleadosApp.Helpers;

namespace GestionEmpleadosApp.Helpers
{
    public class Base64ToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64 = value as string;
            return ImageHelper.Base64ToImageSource(base64);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}

