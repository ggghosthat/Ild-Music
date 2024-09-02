using Ild_Music;
using Ild_Music.Assets;
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
    	if (value is not Guid id)
            return null;

        return parameter.ToString() switch
        {
            "icon" => GetIcon(id),
            "name" => GetName(id)
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static object GetIcon(Guid id)
    {
        if (id.Equals(StartViewModel.viewModelId))
            return Application.Current.FindResource("NavHome");
        else if (id.Equals(ListViewModel.viewModelId))
            return Application.Current.FindResource("NavList");
        else if (id.Equals(BrowserViewModel.viewModelId))
            return Application.Current.FindResource("NavBrowse");
        else 
            return null;
    }

    private static object GetName(Guid id)
    {
        if (id.Equals(StartViewModel.viewModelId))
            return Resources.HomeNavbarItem;
        else if (id.Equals(ListViewModel.viewModelId))
            return Resources.ListNavbarItem;
        else if (id.Equals(BrowserViewModel.viewModelId))
            return Resources.BrowseNavbarItem;
        else 
            return null;
    }
}
