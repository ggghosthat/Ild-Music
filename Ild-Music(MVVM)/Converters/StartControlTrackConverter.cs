using ShareInstances.PlayerResources;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Ild_Music_MVVM_.Converters
{
    internal class StartControlTrackConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
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
