using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace Ild_Music.Converters
{
    internal class SubFactoryDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SubItem subItem)
                return subItem.Control;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}