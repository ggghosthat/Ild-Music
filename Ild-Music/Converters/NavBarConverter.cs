using Ild_Music;
using Ild_Music.ViewModels;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;

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
	private Hashtable ViewModelsTable => App.ViewModelTable;
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
    	//return icons from app resource
    	var result = value switch
    	{
    		'a' => Application.Current.FindResource("NavHome"),
    		'b' => Application.Current.FindResource("NavList"),
    		'c' => Application.Current.FindResource("NavSetting"),
            'd' => Application.Current.FindResource("NavBrowse"),
            'e' => Application.Current.FindResource("NavAbout"),
    		_ => null
    	};

    	return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}