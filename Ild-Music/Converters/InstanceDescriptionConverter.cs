using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;

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
    internal class InstanceDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Artist artist)
                return artist.Description;
            else if (value is Playlist playlist)
                return playlist.Description;
            else if (value is Track track)
                return track.Description;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}