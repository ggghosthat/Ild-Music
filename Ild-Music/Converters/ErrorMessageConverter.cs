using System;
using System.Globalization;
using Avalonia.Media;
using Avalonia.Data.Converters;

namespace Ild_Music.Converters;

internal class ErrorMessageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool errorState)
        {
            if (errorState)
                return (Brush)new BrushConverter().ConvertFrom("#a83240");
            else 
                return (Brush)new BrushConverter().ConvertFrom("#32a852");
        }
        return (Brush)new BrushConverter().ConvertFrom("#32a852");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
