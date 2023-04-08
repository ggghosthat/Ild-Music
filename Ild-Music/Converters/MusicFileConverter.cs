using ShareInstances.Instances;
using ShareInstances.Filer;
using ShareInstances.Instances.Interfaces;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Globalization;

namespace Ild_Music.Converters;
public class MusicFileConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "name")
        {
            if (value is MusicFile entity)
                return entity.FileName;
            return null;
        }
        else if (parameter == "duration")      
        {
            if (value is MusicFile entity)
                return entity.Duration;
            return null;   
        }
        else if (parameter == "ico")
        {
            if (value is MusicFile entity)
            {
                if (entity.Picture != null)
                {
                    return CraftImage(ref entity);
                }
                else return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/track.png");
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

    private object CraftImage(ref MusicFile entity)
    {
        var image = new Image();
        image.Source = new Bitmap(new MemoryStream( entity.Picture ));
        return image;
    }

    private object LoadAsset(string path)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var bitmap = new Bitmap(assets.Open(new Uri(path)));

        var image = new Image();
        image.Source = bitmap;
        return image;
    }
}
