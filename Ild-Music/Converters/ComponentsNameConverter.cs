using ShareInstances;

using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Controls;
using Avalonia.Media;

namespace Ild_Music.Converters
{
    internal class ComponentsNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ( parameter is string param ){
                if (param.Equals("0"))
                {
                    if (value is IPlayer player)
                        return player.PlayerName;
                    else if (value is ISynchArea area)
                        return area.AreaName;
                    return null;
                }
                else if (param.Equals("1"))
                {
                    if (value is IPlayer player)
                        return player.PlayerId;
                    else if (value is ISynchArea area)
                        return area.AreaId;
                    return null;   
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}