using Ild_Music_MVVM_.ViewModel.ModelEntities;
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
                var drawingImage = new Viewbox();

                if (value is ArtistEntityViewModel) 
                {
                    var artistIcon = Application.Current.TryFindResource("ArtistsIcon");

                    if (artistIcon is Viewbox aIcon)
                        return aIcon;
                }
                if (value is PlaylistEntityViewModel)
                {
                    var playlistIcon = Application.Current.TryFindResource("PlaylistsIcon");

                    if (playlistIcon is Viewbox pIcon)
                        return pIcon;
                }
                if (value is TrackEntityViewModel)
                {
                    var trackIcon = Application.Current.TryFindResource("TracksIcon");

                    if (trackIcon is Viewbox tIcon)
                        return tIcon;
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
