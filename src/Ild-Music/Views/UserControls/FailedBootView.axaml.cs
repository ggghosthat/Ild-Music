using Ild_Music.ViewModels;

using System;
using System.Linq;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using PropertyChanged;

namespace Ild_Music.Views;

[DoNotNotifyAttribute]
public partial class FailedBootView : UserControl
{
    public FailedBootView()
    {
        InitializeComponent();
    }

    private async void SaveErrorFile_Clicked(object sender, RoutedEventArgs  args)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Select error file.",
        });

        if (DataContext is FailedBootViewModel failedBootViewModel)
            failedBootViewModel.ExportErrorsList(file.Path.LocalPath);
    }
}