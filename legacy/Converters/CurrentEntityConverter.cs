using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.StoreSpace;
using ShareInstances.Services.Entities;
using Ild_Music;
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


namespace Ild_Music.Converters;
/// <summary>
/// This converter class using for CurrentEntity in MainViewModel only.
/// So it does not throw any special info ecxept name and it own artist name
/// </summary>
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
                    var store = ((StoreService)App.Stage.GetServiceInstance("StoreService")).StoreInstance;
                    return store.GetTracksById(playlist[playlist.CurrentIndex]);
                }
                else return null;
            }
            else if(param.Equals("artist"))
            {
                if (value is ICoreEntity instance)
                {
                    var strBuilder = new StringBuilder();
                    InstanceExtensions.GetInstanceArtist(instance).ToList().ForEach(a => strBuilder.Append(a.Name + ' '));
                    return strBuilder.ToString();
                }
                else return null;
            }
            else return null;
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
