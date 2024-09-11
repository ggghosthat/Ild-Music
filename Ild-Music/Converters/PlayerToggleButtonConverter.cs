using System;
using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Ild_Music.Converters;
public class PlayerToggleButtonConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool state)
        {
            if (state)
                return Application.Current.FindResource("PlayerActive");
            else
                return Application.Current.FindResource("PlayerInactive");
        }
        else
        { 
            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
