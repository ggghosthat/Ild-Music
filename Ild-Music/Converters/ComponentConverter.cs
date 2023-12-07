using Ild_Music.Core.Contracts;

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia;
using Avalonia.Controls;

namespace Ild_Music.Converters;
internal class ComponentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "ico")
        {
            if (value is IPlayer player)
                return Application.Current.FindResource("PlayerDefaultIcon");
            else if (value is ICube area)
                return Application.Current.FindResource("CubeDefaultIcon");
            return null;
        }
        if (parameter == "0")
        {
            if (value is IPlayer player)
                return player.PlayerName;
            else if (value is ICube area)
                return area.CubeName;
            return null;
        }
        else if (parameter == "1")
        {
            if (value is IPlayer player)
                return player.PlayerId;
            else if (value is ICube area)
                return area.CubeId;
            return null;   
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
