using System;
using System.Globalization;
using System.Windows.Data;
using System.Diagnostics;

namespace Ild_Music_MVVM_.Converters
{
    internal class PlayerTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan time)
            {
                Debug.WriteLine(time.ToString());
                return time.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
