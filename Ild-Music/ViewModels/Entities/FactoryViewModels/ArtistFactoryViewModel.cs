using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Exceptions.SynchAreaExceptions;

using System;
using System.Threading.Tasks;
using System.IO;
using Avalonia.Controls;

namespace Ild_Music.ViewModels;
public class ArtistFactoryViewModel : BaseViewModel
{
    public static readonly string nameVM = "ArtistFactoryVM";
    public override string NameVM => nameVM;
        
    #region Services
    private FactoryGhost factoryService => (FactoryGhost)base.GetService(Ghosts.FACTORY);
    private SupportGhost supporterService => (SupportGhost)base.GetService(Ghosts.SUPPORT);
    private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    #endregion

    #region Instance
    public static Artist Instance { get; private set; } = default;
    #endregion

    #region Background_Avatar properties
    public byte[] AvatarSource {get; private set;} = default!;
    #endregion

    #region Commands
    public CommandDelegator SelectAvatarCommand { get; }
    public CommandDelegator CreateArtistCommand { get; }
    public CommandDelegator CancelCommand { get; }
    #endregion

    #region Artist Factory Properties
    public string ArtistName {get; set; } = default!;
    public string ArtistDescription { get; set; } = default!;
    public int ArtistYear { get; set; } = default!;
    #endregion

    #region Log Reply Properties
    public string ArtistLogLine { get; set; } = default!;
    public bool ArtistLogError { get; set; } = default!;
    #endregion

    #region Properties
    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Artist";
    #endregion

    #region Const
    public ArtistFactoryViewModel()
    {
        SelectAvatarCommand = new(SelectAvatar, null);
        CreateArtistCommand = new(CreateArtist, null);
        CancelCommand = new(Cancel, null);
    }
    #endregion


    #region Private Methods
    private async void ExitFactory()
    {
        await FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task FieldsClear()
    {
        ArtistName = default!;
        ArtistDescription = default!;
        ArtistLogLine = default!;
        AvatarSource = default!;
        Instance = default;
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
    public void CreateArtistInstance(object[] values)
    {
        try
        {
            var name = (string)values[0];
            var description = (string)values[1];
            var year = (int)values[2];
            var avatar = (byte[])values[3];
            
            if (!string.IsNullOrEmpty(name))
            {
                factoryService.CreateArtist(name, description, year, avatar);
                ArtistLogLine = "Successfully created!";
                ExitFactory();
            }
        }
        catch (InvalidArtistException ex)
        {
            ArtistLogLine = ex.Message;
        }
    }

    public void EditArtistInstance(object[] values)
    {
        try
        {
            var name = (string)values[0];
            var description = (string)values[1];
            var year = (int) values[2];
            var avatar = (byte[])values[3];

            if (!string.IsNullOrEmpty(name))
            {
                var editArtist = (Artist)Instance;
                editArtist.Name = name.AsMemory();
                editArtist.Description = description.AsMemory();
                editArtist.Year = year;
                editArtist.AvatarSource = (avatar is not null)? avatar:default!;

                supporterService.EditArtistInstance(editArtist); 

                IsEditMode = false;
                ExitFactory();                
            }
        }
        catch(Exception exm)
        {
            ArtistLogLine = exm.Message;
        }
    }

    public async Task DropInstance(Guid artistId) 
    {
        Instance = await supporterService.FetchArtist(artistId);
        IsEditMode = true;
        ArtistName = Instance.Name.ToString();
        ArtistDescription = Instance.Description.ToString();

        AvatarSource = Instance.GetAvatar(); 
        OnPropertyChanged("AvatarSource");
    }
    #endregion

    #region Command Methods
    private void Cancel(object obj)
    {
        ExitFactory();
    }


    private void CreateArtist(object obj)
    {
        object[] value = { ArtistName, ArtistDescription, AvatarSource };
        if (IsEditMode == false)
            CreateArtistInstance(value);
        else 
            EditArtistInstance(value); 
    }

    private async void SelectAvatar(object obj)
    {
        OpenFileDialog dialog = new();
        string[] result = await dialog.ShowAsync(new Window());
        if(result != null && result.Length > 0)
        {
            var avatarPath = string.Join(" ", result);
            AvatarSource = await LoadAvatar(avatarPath);
            OnPropertyChanged("AvatarSource");
        }
    }
    #endregion
}
