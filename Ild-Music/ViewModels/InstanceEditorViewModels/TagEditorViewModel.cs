using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Exceptions.CubeExceptions;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ild_Music.ViewModels;

public class TagEditorViewModel : BaseViewModel
{
    public static readonly string nameVM = "TagEditorVM";        
    public override string NameVM => nameVM;

    public TagEditorViewModel()
    {
        CreateArtistCommand = new(HandleChanges, null);
        CancelCommand = new(Cancel, null);
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
    
    public CommandDelegator CreateArtistCommand { get; }
    public CommandDelegator CancelCommand { get; }

    public static Tag TagInstance { get; private set; } = default!;
    public string Name {get; set; } = default!;
    public string Color { get; set; } = default!;

    public string TagLogLine { get; set; } = default!;
    public bool TagLogError { get; set; } = default!;

    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Tag";

    private async void ExitFactory()
    {
        await FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task FieldsClear()
    {
        Name = default!;
        Color = default!;
    }

    public void CreateArtistInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            factory.CreateTag(Name, Color);
            TagLogLine = "Successfully created!";
            ExitFactory();
        }
        catch (InvalidArtistException ex)
        {
            TagLogLine = ex.Message;
        }
    }

    public void EditArtistInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            var editTag = TagInstance;
            editTag.Name = Name.AsMemory();
            editTag.Color = Color.AsMemory();

            supporter.EditTagInstance(editTag);
            IsEditMode = false;
            ExitFactory();                
        }
        catch(Exception exm)
        {
            TagLogLine = exm.Message;
        }
    }

    public async Task DropInstance(Tag tag) 
    {
        TagInstance = tag;
        IsEditMode = true;
        Name = TagInstance.Name.ToString();
        Color = TagInstance.Color.ToString();
    }

    private void Cancel(object obj)
    {
        ExitFactory();
    }


    private void HandleChanges(object obj)
    {
        if (IsEditMode)
            EditArtistInstance();
        else
            CreateArtistInstance();
    }

    private static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

    public Window? FindWindowByViewModel(INotifyPropertyChanged viewModel) =>
        Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext));
}
