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

public class InstanceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter == "name")
        {
            if (value is CommonInstanceDTO dto)
                return dto.Name.ToString();
            else if(value is Artist artist)
                return artist.Name.ToString();
            else if(value is Playlist playlist)
                return playlist.Name.ToString();
            else if(value is Track track)
                return track.Name.ToString();
            else return null;
        }
        else if (parameter == "shortname")
        {
            if (value is CommonInstanceDTO dto)
                return SliceName(dto.Name);
            else if(value is Artist artist)
                return SliceName(artist.Name);
            else if(value is Playlist playlist)
                return SliceName(playlist.Name);
            else if(value is Track track)
                return SliceName(track.Name);
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
        else if (parameter == "year")
        {
            if (value is Artist artist)
                return artist.Year;
            else if(value is Playlist playlist)
                return playlist.Year;
            else if(value is Track track)
                return track.Year;
            else return null;
        }      
        else if (parameter == "duration")      
        {
            if (value is Track track)
                return track.Duration;
            return null;   
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

    private static string SliceName(ReadOnlyMemory<char> name)
    {
        if (name.Length <= 10)
            return name.ToString();

        return name.Slice(0, 10).ToString() + "...";
    }

    private static string SliceName(string name)
    {
        if (name.Length <= 10)
            return name.ToString();

        return name.AsMemory().Slice(0, 10).ToString() + "...";
    }
}
