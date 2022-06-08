using Ild_Music_MVVM_.ViewModel.VM;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ild_Music_MVVM_.Converters
{
    public class ListControlIconsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ListType)
            {
                var drawingImage = new Canvas();

                switch (value)
                {
                    case ListType.ARTISTS:
                        var artistIcon = Application.Current.TryFindResource("ArtistsIcon");

                        if (artistIcon is Canvas aIcon)
                            return aIcon;

                        break;
                    case ListType.PLAYLISTS:
                        var playlistIcon = Application.Current.TryFindResource("PlaylistIcon");

                        if (playlistIcon is Canvas pIcon)
                            return pIcon;

                        break;
                    case ListType.TRACKS:
                        var trackIcon = Application.Current.TryFindResource("TracksIcon");

                        if (trackIcon is Canvas tIcon)
                            return tIcon;

                        break;
                    default:
                        break;
                }

                return drawingImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
