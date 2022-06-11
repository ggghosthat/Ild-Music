using Ild_Music_MVVM_.ViewModel.ModelEntities;
using Ild_Music_MVVM_.ViewModel.VM;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Ild_Music_MVVM_.Converters
{
    public class ListControlIconsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var drawingImage = new object();

            if (value is ArtistEntityViewModel) 
            {
                var artistIcon = Application.Current.TryFindResource("ArtistsIcon");

                if (artistIcon is DrawingImage aIcon)
                    return aIcon;
            }
            if (value is PlaylistEntityViewModel)
            {
                var playlistIcon = Application.Current.TryFindResource("PlaylistsIcon");

                if (playlistIcon is DrawingImage pIcon)
                    return pIcon;
            }
            if (value is TrackEntityViewModel)
            {
                var trackIcon = Application.Current.TryFindResource("TracksIcon");

                if (trackIcon is DrawingImage tIcon)
                    return tIcon;
            }


            return drawingImage;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
