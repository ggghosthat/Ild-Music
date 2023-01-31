using ShareInstances.PlayerResources;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Controls;


namespace Ild_Music.Converters
{
    internal class CurrentEntityNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Track track)
            {
                return track.Name;
            }
            else if (value is Playlist playlist)
            {
                return playlist.Tracks[playlist.CurrentIndex].Name;
            }


            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
