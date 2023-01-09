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
    internal class ComponentsIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IPlayer player)
                return Application.Current.FindResource("PlayerDefaultIcon");
            else if (value is ISynchArea area)
                return Application.Current.FindResource("AreaDefaultIcon");
                
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}