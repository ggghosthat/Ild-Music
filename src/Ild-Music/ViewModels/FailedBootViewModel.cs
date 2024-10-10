using Ild_Music.Command;
using Ild_Music.Core.Exceptions.Flag;
using Ild_Music.Core.Contracts.ViewModel;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input.Platform;

namespace Ild_Music.ViewModels;

public class FailedBootViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    public FailedBootViewModel()
    {
        AppCloseCommand = new(AppClose, null);
        CopySelectedErrorCommand = new(CopySelectedError, null);
    }

    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public ObservableCollection<ErrorFlag> Errors { get; } = new ();

    public ErrorFlag? SelectedError { get; set; }

    public CommandDelegator AppCloseCommand { get; }

    public CommandDelegator CopySelectedErrorCommand { get; }

    public void SetErrors(IList<ErrorFlag> errors)
    {
        foreach(var error in errors)
            Errors.Add(error);
    }

    public void ExportErrorsList(string filePath)
    {
        File.WriteAllLines(filePath, Errors.ToList().Select(x => x.ToString()));
    }

    public void CopySelectedError(object obj)
    {
        if (SelectedError == null)
            return;

        string errorString = SelectedError.ToString();

        var clipboard = Clipboard.Get();
        if (clipboard != null) 
            clipboard.SetTextAsync(errorString).Wait();
    }

    public void AppClose(object obj)
    {
        if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.Shutdown();
    }
}

public class Clipboard
{
    public static IClipboard Get() 
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: { } window })
            return window.Clipboard!;

        return null!;
    }
}