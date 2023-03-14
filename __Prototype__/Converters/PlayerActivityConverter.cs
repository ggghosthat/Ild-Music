using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Ild_Music_MVVM_.Converters
{
    internal class PlayerActivityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool state)
            {
                if (state)
                    return Geometry.Parse("M14,19H18V5H14M6,19H10V5H6V19Z");

                return Geometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z");
            }
            return Geometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z"); 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
