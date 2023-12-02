using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Globalization;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace Ild_Music.Converters;

public class InstanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        if (parameter == "name")
        {
            if (value is CommonInstanceDTO dto)
                return dto.Name;
            else if(value is Artist artist)
                return artist.Name;
            else if(value is Playlist playlist)
                return playlist.Name;
            else if(value is Track track)
                return track.Name;
            else return null;
        }
        else if (parameter == "desc")
        {
            if (value is Artist artist)
                return artist.Description;
            else if(value is Playlist playlist)
                return playlist.Description;
            else if(value is Track track)
                return track.Description;
            else return null;
        }      
        else if (parameter == "duration")      
        {
            if (value is Track track)
                return track.Duration;
            return null;   
        }
        else if (parameter == "ico_col")
        {
            if(value is Artist artist)
            {
                if(artist.AvatarSource.Length is 0)
                    return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/artist.png");
                return CraftImage(artist.AvatarSource);
            }
            else if(value is Playlist playlist)
            {
                if(playlist.AvatarSource.Length is 0)
                    return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/playlist.png");
                return CraftImage(playlist.AvatarSource);
            }
            else if(value is Track track)
            {
                if(track.AvatarSource.Length is 0)
                    return LoadAsset(@"avares://Ild_Music/Assets/DefaultIcons/track.png");
                return CraftImage(track.AvatarSource);
            }
            else return null;
        }
        else if (parameter == "aico_dis")
        {
            if (value is byte[] source)
            {
                if (source is null)
                    return Application.Current.FindResource("ArtistAvatar");

                return ComputeAvatarIcon(ref source);
            }
            else return Application.Current.FindResource("ArtistAvatar");
        }
        else if (parameter == "pico_dis")
        {
            if (value is byte[] source)
            {
                if (source is null)
                    return Application.Current.FindResource("PlaylistAvatar");

                return ComputeAvatarIcon(ref source);
            }
            else return Application.Current.FindResource("PlaylistAvatar");
        }
        else if (parameter == "tico_dis")
        {
            if (value is byte[] source)
            {
                if (source is null)
                    return Application.Current.FindResource("TrackAvatar");

                return ComputeAvatarIcon(ref source);
            }
            else return Application.Current.FindResource("TrackAvatar");
        }
        else if (parameter == "back")
        {
            if (value is byte[] source)
            {
                if (source is not null)
                    return CreateBackImage(ref source);
                
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

        image.Source = new Bitmap(new MemoryStream(source));
        return resource;
    }


    private object CreateBackImage(ref byte[] source)
    {
        Avalonia.Media.Color dominantColor;
        using (var pic = SixLabors.ImageSharp.Image.Load<Rgba32>( source ))
        {
           pic.Mutate(x => x.Resize(new ResizeOptions 
                                    {
                                        Sampler = KnownResamplers.NearestNeighbor,
                                        Size = new SixLabors.ImageSharp.Size(100, 0)
                                    }));

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

    private object CraftImage(ReadOnlyMemory<byte> source)
    {
        var image = new Avalonia.Controls.Image();
        var raw = source.ToArray();
        image.Source = new Bitmap(new MemoryStream( raw ));
        return image;
    }

    private object LoadAsset(string path)
    {
        var bitmap = new Bitmap(AssetLoader.Open(new Uri(path)));

        var image = new Avalonia.Controls.Image();
        image.Source = bitmap;
        return image;
    }
}
