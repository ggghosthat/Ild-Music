using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Exceptions.CubeExceptions;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class ArtistEditorViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;
    
    public ArtistEditorViewModel()
    {
        CreateArtistCommand = new(HandleChanges, null);
        CancelCommand = new(Cancel, null);
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT); 
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];
    
    public CommandDelegator CreateArtistCommand { get; }

    public CommandDelegator CancelCommand { get; }

    public static Artist ArtistInstance { get; private set; } = default!;
    
    public string Name {get; set; } = default!;
    
    public string Description { get; set; } = default!;
    
    public int Year { get; set; } = default!;

    public string YearStr { get; set; }
    
    public byte[] Avatar { get; private set; } = default!;
    
    public string AvatarPath { get; private set; } = default!;
    
    public string ArtistLogLine { get; set; } = default!;
    
    public bool ArtistLogError { get; set; } = default!;
    
    public bool IsEditMode {get; private set;} = false;
    
    public string ViewHeader {get; private set;} = "Artist";

    private async void ExitFactory()
    {
        await FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task FieldsClear()
    {
        Name = default;
        Description = default;
        ArtistLogLine = default;
        Avatar = default;
        AvatarPath = default;
        ArtistInstance = default;
    }

    public void CreateArtistInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            Artist artist;
            factory.CreateArtist(Name, Description, AvatarPath, Year, out artist);
            ArtistInstance = artist;
            ArtistLogLine = "Successfully created!";
            ExitFactory();
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
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            var editArtist = ArtistInstance;
            editArtist.Name = Name.AsMemory();
            editArtist.Description = Description.AsMemory();
            editArtist.Year = Year;
            editArtist.AvatarSource = (Avatar is not null)? Avatar:default!;
            editArtist.AvatarPath = AvatarPath.AsMemory();

            supporter.EditArtistInstance(editArtist);
            IsEditMode = false;
            ExitFactory();                
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
        AvatarPath = ArtistInstance.AvatarPath.ToString();
                
        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            {
                Avatar = new byte[fs.Length];
                await fs.ReadAsync(Avatar, 0, (int)fs.Length);
            }
        }
    }

    private void Cancel(object obj)
    {
        ExitFactory();
    }


    private void HandleChanges(object obj)
    {
        if (IsEditMode == true)
            EditArtistInstance();
        else
            CreateArtistInstance();
    }

    public async void SelectAvatar(string path)
    {
        if(!String.IsNullOrEmpty(path) && !String.IsNullOrWhiteSpace(path))
        {
            AvatarPath = path;
            Avatar = await LoadAvatar(path);
            OnPropertyChanged("Avatar");
            OnPropertyChanged("AvatarPath");
        }
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
}
