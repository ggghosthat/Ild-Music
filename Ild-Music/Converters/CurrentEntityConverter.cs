using Ild_Music.Core.Instances;

using System;
using System.Globalization;
using Avalonia.Data.Converters;


namespace Ild_Music.Converters;
/// <summary>
/// This converter class using for CurrentEntity in MainViewModel only.
/// So it does not throw any special info ecxept name and it own artist name
/// </summary>
internal class CurrentEntityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (parameter is string param)
        {
            if (param.Equals("name"))
            {
                if (value is Track track)
                {
                    return track.Name;
                }
                else if (value is Playlist playlist)
                {
                    return null;
                }
                else return null;
            }
            else if(param.Equals("artist"))
            {
                return null;
            }
            else return null;
        }
        else return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
