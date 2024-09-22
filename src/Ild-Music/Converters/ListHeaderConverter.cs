using Ild_Music.Assets;
using Ild_Music.Core.Instances;

using System;
using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace Ild_Music.Converters;
public class ListHeaderConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not EntityTag entitytag)
            return null;

        return entitytag switch
        {
            EntityTag.ARTIST => Resources.ArtistHeaderListView,
            EntityTag.PLAYLIST => Resources.PlaylistHeaderListView,
            EntityTag.TRACK => Resources.TrackHeaderListView
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
