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

public class ArtistEditorViewModel : BaseViewModel
{
    public static readonly string nameVM = "ListVM";        
    public override string NameVM => nameVM;
    
    #region Services
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
    #endregion
    
    #region Instance properties
    #endregion

    #region Commands
    public CommandDelegator SelectAvatarCommand { get; }
    public CommandDelegator CreateArtistCommand { get; }
    public CommandDelegator CancelCommand { get; }
    #endregion

    #region Artist Properties
    public static Artist ArtistInstance { get; private set; } = default!;
    public string Name {get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Year { get; set; } = default!;
    public byte[] Avatar { get; private set; } = default!;
    #endregion

    #region Log Reply Properties
    public string ArtistLogLine { get; set; } = default!;
    public bool ArtistLogError { get; set; } = default!;
    #endregion

    #region Properties
    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Artist";
    #endregion

    public ArtistEditorViewModel()
    {
        CreateArtistCommand = new(HandleChanges, null);
        SelectAvatarCommand = new(SelectAvatar, null);
        CancelCommand = new(Cancel, null);
    }

    #region Private Methods
    private async void ExitFactory()
    {
        await FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task FieldsClear()
    {
        Name = default!;
        Description = default!;
        ArtistLogLine = default!;
        Avatar = default!;
        ArtistInstance = default;
    }

    private async Task<byte[]> LoadAvatar(string path)
    {
        byte[] result;
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            result = new byte[fileStream.Length];
            await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
        }
        return result;
    }
    #endregion

    #region Public Methods
    public void CreateArtistInstance()
    {
        try
        {
            if (!String.IsNullOrEmpty(Name))
            {
                factory.CreateArtist(Name, Description, Year, Avatar);
                ArtistLogLine = "Successfully created!";
                ExitFactory();
            }
        }
        catch (InvalidArtistException ex)
        {
            ArtistLogLine = ex.Message;
        }
    }

    public void EditArtistInstance()
    {
        try
        {
            if (!String.IsNullOrEmpty(Name))
            {
                var editArtist = (Artist)ArtistInstance;
                editArtist.Name = Name.AsMemory();
                editArtist.Description = Description.AsMemory();
                editArtist.Year = Year;
                editArtist.AvatarSource = (Avatar is not null)? Avatar:default!;

                supporter.EditArtistInstance(editArtist); 

                IsEditMode = false;
                ExitFactory();                
            }
        }
        catch(Exception exm)
        {
            ArtistLogLine = exm.Message;
        }
    }

    public async Task DropInstance(CommonInstanceDTO instanceDTO) 
    {
        ArtistInstance = await supporter.GetArtistAsync(instanceDTO);
        IsEditMode = true;
        Name = ArtistInstance.Name.ToString();
        Description = ArtistInstance.Description.ToString();

        Avatar = ArtistInstance.GetAvatar(); 
        OnPropertyChanged("AvatarSource");
    }
    #endregion

    #region Command Methods
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

    private async void SelectAvatar(object obj)
    {
        OpenFileDialog dialog = new();
        string[] result = await dialog.ShowAsync(FindWindowByViewModel(this));
        if(result != null && result.Length > 0)
        {
            var avatarPath = string.Join(" ", result);
            Avatar = await LoadAvatar(avatarPath);
            OnPropertyChanged("AvatarSource");
        }
    }
    #endregion

}
