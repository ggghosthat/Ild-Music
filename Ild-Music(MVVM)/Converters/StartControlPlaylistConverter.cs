using ShareInstances.PlayerResources;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ild_Music_MVVM_.Converters
{
    internal class StartControlPlaylistConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Playlist playlist)
                return playlist.Name.Substring(0, 1).ToUpper();
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
