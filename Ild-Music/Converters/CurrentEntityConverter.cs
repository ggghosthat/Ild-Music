using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using Ild_Music.Extensions;

using System;
using System.Text;
using System.Linq;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Controls;


namespace Ild_Music.Converters
{
    internal class CurrentEntityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string param)
            {
                if (param.Equals("name"))
                {
                    if (value is Track track)
                    {
                        return track.Name;
                    }
                    else if (value is Playlist playlist)
                    {
                        return playlist.Tracks[playlist.CurrentIndex].Name;
                    }
                }
                else if(param.Equals("artist"))
                {
                    if (value is ICoreEntity instance)
                    {
                        var strBuilder = new StringBuilder();
                        InstanceExtensions.GetInstanceArtist(instance).ToList().ForEach(a => strBuilder.Append(a.Name + ' '));
                        return strBuilder.ToString();
                    }
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
