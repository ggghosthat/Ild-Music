using Ild_Music;
using Ild_Music.ViewModels;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Data.Converters;
using System;
using System.IO;
using System.Collections;
using System.Globalization;

namespace Ild_Music.Converters;

public class NavBarConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
    	//return icons from app resource
    	var result = value switch
    	{
    		"Home" => Application.Current.FindResource("NavHome"),
    		"Collection" => Application.Current.FindResource("NavList"),
    		"Settings" => Application.Current.FindResource("NavSetting"),
            "Browse" => Application.Current.FindResource("NavBrowse"),
            "About" => Application.Current.FindResource("NavAbout"),
    		_ => null
    	};

    	return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
