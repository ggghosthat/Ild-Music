using ShareInstances.PlayerResources;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ild_Music_MVVM_.Converters
{
    internal class StartControlInstanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Artist artist)
                return artist.Name.Substring(0, 1).ToUpper();
            if (value is Playlist playlist)
                return playlist.Name.Substring(0, 1).ToUpper();
            if (value is Track track)
                return track.Name;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
