using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Globalization;

namespace Ild_Music.Converters;
public class InstanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "name")
        {
            if (value is ICoreEntity entity)
                return entity.Name;
            return null;
        }
        else if (parameter == "desc")
        {
            if (value is ICoreEntity entity)
                return entity.Description;
            return null;
        }
        else if (parameter == "ico_def")
        {   
            if (value is ICoreEntity entity)
            {
                if (entity.AvatarBase64 != null)
                {
                    return entity.GetAvatar();
                }
                else
                {
                    if (entity is Artist artist)
                        return Application.Current.FindResource("DefaultArtistIcon");
                    else if (entity is Playlist playlist)
                        return Application.Current.FindResource("DefaultPlaylistIcon");
                    else if (entity is Track track)
                        return Application.Current.FindResource("DefaultTrackIcon");        
                    else return null;        
                }
            }
            else return null;
        }
        else if (parameter == "ico_col")
        {
            if (value is ICoreEntity entity)
            {
                if (entity.AvatarBase64 != null)
                {
                    return entity.GetAvatar();
                }
                else
                {
                    if (value is Artist artist)
                        return Application.Current.FindResource("ColoredArtistIcon");
                    else if (value is Playlist playlist)
                        return Application.Current.FindResource("ColoredPlaylistIcon");
                    else if (value is Track track)
                        return Application.Current.FindResource("ColoredTrackIcon");
                    else return null;
                }
            }
            else return null;
        }
        else if (parameter == "aico_dis")
        {
            Console.WriteLine($"Out:{value}; Null is {value is null}");
            if (value is byte[] source)
            {
                if (source is not null)
                {
                    return ComputeAvatarIcon(ref source);
                }
                else
                {
                    Console.WriteLine("Why??");
                    return Application.Current.FindResource("ArtistAvatar");
                }
            }
            else return Application.Current.FindResource("ArtistAvatar");
        }
        else if (parameter == "pico_dis")
        {
            if (value is byte[] source)
            {
                if (source is not null)
                {
                    return ComputeAvatarIcon(ref source);
                }
                else return Application.Current.FindResource("PlaylistAvatar");
            }
            else return null;
        }
        else if (parameter == "tico_dis")
        {
            if (value is byte[] source)
            {
                if (source is not null)
                {
                    return ComputeAvatarIcon(ref source);
                }
                else return Application.Current.FindResource("TrackAvatar");
            }
            else return null;
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


    private object ComputeAvatarIcon(ref byte[] source)
    {
        var resource = (Border)Application.Current.FindResource("DisplayImage");
        var image = (Image)resource.Child;

        image.Source = new Bitmap(new MemoryStream(source));
        return resource;
    }
}
