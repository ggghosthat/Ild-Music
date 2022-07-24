using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ild_Music_MVVM_.Converters
{
    internal class FactoryControlUserControlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FactorySubControlTab tab)
                return tab.UserControl;

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
