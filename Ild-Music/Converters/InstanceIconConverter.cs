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
    internal class InstanceIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string param)
            {
                if (param.Equals("def"))
                {
                    if (value is Artist artist)
                        return Application.Current.FindResource("DefaultArtistIcon");
                    else if (value is Playlist playlist)
                        return Application.Current.FindResource("DefaultPlaylistIcon");
                    else if (value is Track track)
                        return Application.Current.FindResource("DefaultTrackIcon");
                }
                else if (param.Equals("col"))
                {
                    if (value is Artist artist)
                        return Application.Current.FindResource("ColoredArtistIcon");
                    else if (value is Playlist playlist)
                        return Application.Current.FindResource("ColoredPlaylistIcon");
                    else if (value is Track track)
                        return Application.Current.FindResource("ColoredTrackIcon");
                }

            }
            return new object();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
