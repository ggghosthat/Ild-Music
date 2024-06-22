using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
// using SixLabors.ImageSharp;
// using SixLabors.ImageSharp.Processing;
// using SixLabors.ImageSharp.PixelFormats;

namespace Ild_Music.Converters;

public class InstanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "name")
        {
            if (value is CommonInstanceDTO dto)
                return dto.Name.ToString();
            else if(value is Artist artist)
                return artist.Name;
            else if(value is Playlist playlist)
                return playlist.Name;
            else if(value is Track track)
                return track.Name;
            else return "name";
        }
        else if (parameter == "desc")
        {
            if (value is Artist artist)
                return artist.Description;
            else if(value is Playlist playlist)
                return playlist.Description;
            else if(value is Track track)
                return track.Description;
            else return "description";
        }
        else if (parameter == "year")
        {
            if (value is Artist artist)
                return artist.Year;
            else if(value is Playlist playlist)
                return playlist.Year;
            else if(value is Track track)
                return track.Year;
            else return "year";
        }      
        else if (parameter == "duration")      
        {
            if (value is Track track)
                return track.Duration;
            return null;   
        }
        else if (parameter == "dto_icon" && value is CommonInstanceDTO dto)
        {
            if (dto.Avatar.Length > 0)
            {
                return LoadBitmapAsync(dto).Result;
            }
            else return dto.Tag switch
            {
                EntityTag.ARTIST => LoadBitmapAsync(@"avares://Ild-Music/Assets/DefaultIcons/artist.png").Result,
                EntityTag.PLAYLIST => LoadBitmapAsync(@"avares://Ild-Music/Assets/DefaultIcons/playlist.png").Result,
                EntityTag.TRACK => LoadBitmapAsync(@"avares://Ild-Music/Assets/DefaultIcons/track.png").Result,
                EntityTag.TAG => LoadBitmapAsync(@"avares://Ild-Music/Assets/DefaultIcons/tag.png").Result,
            };
        }
        else if (parameter == "dto_has_icon" && value is CommonInstanceDTO dto_has_icon)
        {            
            return dto_has_icon.Avatar.Length == 0;
        }
        else if (parameter == "aico_col")
        {
            if (value is byte[] artistIconSource && artistIconSource.Length > 0)
                return CraftImage(artistIconSource, 300d, 300d).Result;

            return LoadAsset(@"avares://Ild-Music/Assets/DefaultIcons/artist.png", 300d, 300d).Result;
        }
        else if (parameter == "pico_col")
        {
            if (value is byte[] playlistIconSource && playlistIconSource.Length > 0)
                return CraftImage(playlistIconSource, 300d, 300d).Result;
                
            return LoadAsset(@"avares://Ild-Music/Assets/DefaultIcons/playlist.png", 300d, 300d).Result;
        }
        else if (parameter == "tico_col")
        {
            if (value is byte[] trackIconSource && trackIconSource.Length > 0)
                return CraftImage(trackIconSource, 300d, 300d).Result;

            return LoadAsset(@"avares://Ild-Music/Assets/DefaultIcons/track.png", 300d, 300d).Result;
        }
        else if (parameter == "tag_col")
        {
            return LoadAsset(@"avares://Ild-Music/Assets/DefaultIcons/tag.png", 150d, 150d).Result; 
        }
        else if (parameter == "aico_dis" && value is byte[] )
        {
            if (value is byte[] artistDisplaySource && artistDisplaySource.Length > 0)
                return DockAvatar(artistDisplaySource);

            return Application.Current.FindResource("ArtistAvatar");
        }
        else if (parameter == "pico_dis")
        {
            if (value is byte[] playlistDisplaySource && playlistDisplaySource.Length > 0)
                return DockAvatar(playlistDisplaySource);
            
            return Application.Current.FindResource("PlaylistAvatar");
        }
        else if (parameter == "tico_dis")
        {
            if (value is byte[] trackDisplaySource && trackDisplaySource.Length > 0)
                return DockAvatar(trackDisplaySource);

            return Application.Current.FindResource("TrackAvatar");
        }
        else if (parameter == "back")
        {
            var defaultColor = new Avalonia.Media.Color(125, 39, 218, 72);
            if (value is byte[] source)
            {
                if (source != null && source.Length > 0)
                    // return CreateBackImage(ref source);
                    return null;
                else return defaultColor;
            }
            else return defaultColor;
        }        
        else if (parameter == "coll_active" && value is int collectionCount)
        {
            return collectionCount > 0;
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


    private object DockAvatar(byte[] source, double w = 0d, double h = 0d)
    {
        var resource = (Border)Application.Current.FindResource("DisplayImage");
        var image = (Avalonia.Controls.Image)resource.Child;

        if (w >= 0d && h >= 0d)
        {
            image.Width = w;
            image.Height = h;
        }

        using var ms = new MemoryStream(source);
        image.Source = new Bitmap(ms);

        resource.Child = image;
        return resource;
    }
    
    private async Task<object> CraftImage(ReadOnlyMemory<byte> source, double w = 0d, double h = 0d)
    {
        var image = new Avalonia.Controls.Image();

        if (w > 0d && h > 0d)
        {
            image.Width = w;
            image.Height = h;
        }

        using var ms = new MemoryStream(source.ToArray());
        image.Source = new Bitmap(ms);
        return image;
    }

    private async Task<object> LoadAsset(string path, double w = 0d, double h = 0d)
    {
        var image = new Avalonia.Controls.Image();

        if (w > 0d && h > 0d)
        {
            image.Width = w;
            image.Height = h;
        }

        var bitmap = new Bitmap(AssetLoader.Open(new Uri(path)));
        image.Source = bitmap;
        return image;
    }

    private async Task<Bitmap> LoadBitmapAsync(string path)
    {
        return new Bitmap(AssetLoader.Open(new Uri(path)));
    }

    private async Task<Bitmap> LoadBitmapAsync(CommonInstanceDTO instanceDto)
    {
        byte[] source = instanceDto.Avatar.ToArray();
        using var ms = new MemoryStream(source);
        return new Bitmap(ms);
    }

    private object CreateBackImage(ref byte[] source)
    {
        // Avalonia.Media.Color dominantColor;
        // using (var pic = SixLabors.ImageSharp.Image.Load<Rgba32>( source ))
        // {
        //     var resizeOptions = new ResizeOptions 
        //     {
        //         Sampler = KnownResamplers.NearestNeighbor,
        //         Size = new SixLabors.ImageSharp.Size(100, 0)
        //     };
        //     pic.Mutate(x => x.Resize(resizeOptions));

        //     int r = 0;
        //     int g = 0;
        //     int b = 0;
        //     int totalPixels = 0;

        //     for (int x = 0; x < pic.Width; x++)
        //     {
        //         for (int y = 0; y < pic.Height; y++)
        //         {
        //             var pixel = pic[x, y];
        //             r += System.Convert.ToInt32(pixel.R);
        //             g += System.Convert.ToInt32(pixel.G);
        //             b += System.Convert.ToInt32(pixel.B);
        //             totalPixels++;
        //         }
        //     }

        //     r /= totalPixels;
        //     g /= totalPixels;
        //     b /= totalPixels;

        //     dominantColor = new Avalonia.Media.Color(255, (byte) r, (byte) g, (byte) b);
        // }

        // return dominantColor;
        return null;
    }
}
