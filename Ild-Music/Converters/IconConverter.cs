using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Ild_Music.Converters;

public class IconConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "dto_icon" && value is CommonInstanceDTO dto)
        {
            if (dto.Avatar.Length > 0)
            {
                return LoadBitmapFromDto(dto).Result;
            }
            else return dto.Tag switch
            {
                EntityTag.ARTIST => LoadBitmapfromPath(@"avares://Ild-Music/Assets/DefaultIcons/artist.png").Result,
                EntityTag.PLAYLIST => LoadBitmapfromPath(@"avares://Ild-Music/Assets/DefaultIcons/playlist.png").Result,
                EntityTag.TRACK => LoadBitmapfromPath(@"avares://Ild-Music/Assets/DefaultIcons/track.png").Result,
                EntityTag.TAG => LoadBitmapfromPath(@"avares://Ild-Music/Assets/DefaultIcons/tag.png").Result,
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
                return CreateInstanceIcon(artistDisplaySource);

            return Application.Current.FindResource("ArtistAvatar");
        }
        else if (parameter == "pico_dis")
        {
            if (value is byte[] playlistDisplaySource && playlistDisplaySource.Length > 0)
                return CreateInstanceIcon(playlistDisplaySource);
            
            return Application.Current.FindResource("PlaylistAvatar");
        }
        else if (parameter == "tico_dis")
        {
            if (value is byte[] trackDisplaySource && trackDisplaySource.Length > 0)
                return CreateInstanceIcon(trackDisplaySource);

            return Application.Current.FindResource("TrackAvatar");
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private object CreateInstanceIcon(byte[] source, double w = 0d, double h = 0d)
    {
        var resource = (Border)Application.Current.FindResource("DisplayImage");
        var image = (Avalonia.Controls.Image)resource.Child;        
        resource.Child = ProcessImageFromSource(image, source, w, h);
        return resource;
    }
    
    private async Task<object> CraftImage(ReadOnlyMemory<byte> source, double w = 0d, double h = 0d)
    {
        var image = new Avalonia.Controls.Image();
        return ProcessImageFromSource(image, source, w, h);
    }

    private async Task<object> LoadAsset(string path, double w = 0d, double h = 0d)
    {
        var image = new Avalonia.Controls.Image();
        return ProcessImageFromPath(image, path, w, h);
    }

    private Avalonia.Controls.Image ProcessImageFromSource(
        Avalonia.Controls.Image image,
        ReadOnlyMemory<byte> source,
        double w, double h)
    {
        if (w > 0d && h > 0d)
        {
            image.Width = w;
            image.Height = h;
        }

        using var ms = new MemoryStream(source.ToArray());
        image.Source = new Bitmap(ms);
        image.Stretch = Stretch.Fill;

        return image;
    }

    private Avalonia.Controls.Image ProcessImageFromPath(
        Avalonia.Controls.Image image,
        string path,
        double w, double h)
    {
        if (w > 0d && h > 0d)
        {
            image.Width = w;
            image.Height = h;
        }

        var bitmap = new Bitmap(AssetLoader.Open(new Uri(path)));
        image.Source = bitmap;
        image.Stretch = Stretch.Fill;

        return image;
    }

    private async Task<Bitmap> LoadBitmapfromPath(string path)
    {
        return new Bitmap(AssetLoader.Open(new Uri(path)));
    }

    private async Task<Bitmap> LoadBitmapFromDto(CommonInstanceDTO instanceDto)
    {
        byte[] source = instanceDto.Avatar.ToArray();
        using var ms = new MemoryStream(source);
        return new Bitmap(ms);
    }
}