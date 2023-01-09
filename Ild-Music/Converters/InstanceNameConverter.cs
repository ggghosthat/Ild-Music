using ShareInstances.PlayerResources;

using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Ild_Music.Converters
{
    internal class InstanceNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Artist artist)
                return artist.Name;
            else if (value is Playlist playlist)
                return playlist.Name;
            else if (value is Track track)
                return track.Name;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}