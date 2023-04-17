using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        else if (parameter == "ico_col")
        {
            if (value is ICoreEntity entity)
            {
                if (entity.AvatarBase64 is not null)
                {
                    return CraftImage(ref entity);
                }
                else
                {
                    if (value is Artist artist)
                        return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/artist.png");
                    else if (value is Playlist playlist)
                        return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/playlist.png");
                    else if (value is Track track)
                        return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/track.png");
                    else return null;
                }
            }
            else return null;
        }
        else if (parameter == "aico_dis")
        {
            if (value is byte[] source)
            {
                if (source is not null)
                {
                    return ComputeAvatarIcon(ref source);
                }
                else return Application.Current.FindResource("ArtistAvatar");
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
            else return Application.Current.FindResource("PlaylistAvatar");
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
            else return Application.Current.FindResource("TrackAvatar");
        }
        else if (parameter == "back")
        {
            if (value is byte[] source)
            {
                if (source is not null)
                {
                    return CreateBackImage(ref source);
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


    private object ComputeAvatarIcon(ref byte[] source)
    {
        var resource = (Border)Application.Current.FindResource("DisplayImage");
        var image = (Avalonia.Controls.Image)resource.Child;
        
        using(var mem = new MemoryStream(source))
        {
            image.Source = new Bitmap(mem);
        }
        return resource;
    }

    private object CraftImage(ref ICoreEntity entity)
    {
        var image = new Avalonia.Controls.Image();
        using(var mem = new MemoryStream( entity.GetAvatar() ))
        {
            image.Source = new Bitmap(mem);
        }
        return image;
    }

    private object CreateBackImage(ref byte[] source)
    {
        Avalonia.Media.Color dominantColor;
        using (var pic = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>( source ))
        {
           pic.Mutate(x => x
                .Resize(new ResizeOptions {Sampler = KnownResamplers.NearestNeighbor, Size = new SixLabors.ImageSharp.Size(100, 0)}));

            int r = 0;
            int g = 0;
            int b = 0;
            int totalPixels = 0;

            for (int x = 0; x < pic.Width; x++)
            {
                for (int y = 0; y < pic.Height; y++)
                {
                    var pixel = pic[x, y];
                    r += System.Convert.ToInt32(pixel.R);
                    g += System.Convert.ToInt32(pixel.G);
                    b += System.Convert.ToInt32(pixel.B);
                    totalPixels++;
                }
            }

            r /= totalPixels;
            g /= totalPixels;
            b /= totalPixels;

            dominantColor = new Avalonia.Media.Color(255, (byte) r, (byte) g, (byte) b);
        }

        return dominantColor;
    }

    private object LoadAsset(string path)
    {
        var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
        var bitmap = new Bitmap(assets.Open(new Uri(path)));

        var image = new Avalonia.Controls.Image();
        image.Source = bitmap;
        return image;
    }
}
