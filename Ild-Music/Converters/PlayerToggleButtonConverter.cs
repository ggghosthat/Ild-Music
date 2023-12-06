using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Ild_Music.Converters;
internal class PlayerToggleButtonConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool state)
            if (state)
                return Geometry.Parse("M14,19H18V5H14M6,19H10V5H6V19Z");
            else 
                return Geometry.Parse("M8.5,8.64L13.77,12L8.5,15.36V8.64M6.5,5V19L17.5,12");
            
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
