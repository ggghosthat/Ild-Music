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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class TrackEditorViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;
    
    public TrackEditorViewModel()
    {
        CancelCommand = new(Cancel, null);
        CreateTrackCommand = new(HandleChanges, null);
        TrackArtistExplorerCommand = new(OpenTrackArtistExplorer, null);
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);    
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];    
    private static InstanceExplorerViewModel Explorer => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.viewModelId];
   
    public Track TrackInstance { get; private set; }
    public string Path { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Year {get; set;}
    public byte[] Avatar {get; private set;}
    public string AvatarPath {get; private set;}

    public CommandDelegator CreateTrackCommand { get; }    
    public CommandDelegator CancelCommand { get; }
    public CommandDelegator TrackArtistExplorerCommand {get;}

    public ObservableCollection<CommonInstanceDTO> ArtistsProvider { get; set; } = new ();
    public ObservableCollection<CommonInstanceDTO> SelectedTrackArtists { get; set; } = new (); 
    
    public string TrackLogLine { get; set; }
    public bool TrackLogError { get; set; }
    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Track";
  
    private void ExitFactory()
    {
        FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task InitProviders()
    {
        ArtistsProvider.ToList().Clear();

        using (var instancePool = await supporter.GetInstancePool())
        {
            instancePool.ArtistsDTOs.ToList()
                .ForEach(artist => ArtistsProvider.Add(artist));
        }
    }

    private void ArtistProviderUpdate()
    {
        ArtistsProvider.Clear();
        
        using (var instancePool = supporter.GetInstancePool().Result)
        {
            instancePool.ArtistsDTOs.ToList()
                .ForEach(artist => ArtistsProvider.Add(artist));        
        }
    }

    private void FieldsClear()
    {
        Path = default;
        Name = default;
        Description = default;
        TrackLogLine = default;
        Avatar = default;
        AvatarPath = default;
        SelectedTrackArtists.Clear();
    }

    public void CreateTrackInstance()
    {
        try
        {            
            var artists = SelectedTrackArtists
                .Select(a => supporter.GetArtistAsync(a).Result)
                .ToList();
            
            Track track;
            factory.CreateTrack(Path, Name, Description, AvatarPath, Year, artists, out track);
            TrackInstance = track;
            TrackLogLine = "Successfully created!"; 
            ExitFactory();
        }
        catch(InvalidTrackException ex)
        {
            TrackLogLine = ex.Message;
        }
    }

    public void EditTrackInstance()
    {
        try
        {
            var editTrack = TrackInstance;
            editTrack.Pathway = Path.AsMemory();
            editTrack.Name = Name.AsMemory();
            editTrack.Description = Description.AsMemory();
            editTrack.AvatarPath = AvatarPath.AsMemory();

            if(SelectedTrackArtists != null && SelectedTrackArtists.Count > 0)
            {
                ArtistsProvider.ToList()
                    .Except(SelectedTrackArtists).ToList()
                    .ForEach(async a => 
                    {
                        var artistInstance = await supporter.GetArtistAsync(a);
                        artistInstance.DeleteTrack(ref editTrack);
                    });

                SelectedTrackArtists.ToList()
                    .ForEach(async a => 
                    {
                        var artistInstance = await supporter.GetArtistAsync(a);
                        artistInstance.AddTrack(ref editTrack);
                    });
            }

            supporter.EditTrackInstance(editTrack);
            IsEditMode = false;
            ExitFactory();
        }
        catch(InvalidTrackException ex)
        {
            TrackLogLine = ex.Message;
        }
    }

    public async void DropInstance(CommonInstanceDTO instanceDto) 
    {
        TrackInstance = await supporter.GetTrackAsync(instanceDto);
        IsEditMode = true;
        Path = TrackInstance.Pathway.ToString();
        Name = TrackInstance.Name.ToString();
        Description = TrackInstance.Description.ToString();
        AvatarPath = TrackInstance.AvatarPath.ToString();

        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            {
                Avatar = new byte[fs.Length];
                await fs.ReadAsync(Avatar, 0, (int)fs.Length);
            }
        }
        
        using (var instancePool = await supporter.GetInstancePool())
        {
            instancePool.ArtistsDTOs
                .Where(a => TrackInstance.Artists.Contains(a.Id))
                .ToList()
                .ForEach(a => SelectedTrackArtists.Add(a));
        }
    }

    private void OnArtistItemsSelected()
    {
        if(Explorer.Output.Count > 0)
        {
            if (Explorer.Output[0].Tag == EntityTag.ARTIST)
            {
                SelectedTrackArtists.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                
                using (var instancePool = supporter.GetInstancePool().Result)
                {
                    instancePool.ArtistsDTOs
                        .Where(a => outIds.Contains(a.Id))
                        .ToList()
                        .ForEach(i => SelectedTrackArtists.Add(i));
                }
            }
        }
    }

    private void OpenTrackArtistExplorer(object obj)
    {
        Explorer.OnArtistsSelected += OnArtistItemsSelected;
        
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.ARTIST)
            Explorer.Arrange(EntityTag.ARTIST, preSelected); 
        else
            Explorer.Arrange(EntityTag.ARTIST); 

        MainVM.PushVM(this, Explorer);
        MainVM.ResolveWindowStack();        
    }

    private void HandleChanges(object obj)
    {
        if (IsEditMode)
            EditTrackInstance();
        else
            CreateTrackInstance();
    }
    
    private void Cancel(object obj)
    {
        ExitFactory();
    }

    public void DefineTrackPath(string path)
    {
        if(!String.IsNullOrEmpty(path) && !String.IsNullOrWhiteSpace(path))
        {
            Path = path;
            using( var taglib = TagLib.File.Create(Path))
            {
                Name = (!string.IsNullOrEmpty(taglib.Tag.Title))
                    ? taglib.Tag.Title
                    : "Unknown"; 
            }
        }
    }

    public async void SelectAvatar(string path)
    {
        if(!String.IsNullOrEmpty(path) && !String.IsNullOrWhiteSpace(path))
        {
            AvatarPath = path;
            Avatar = await LoadAvatar(path); 
            OnPropertyChanged("AvatarPath");
            OnPropertyChanged("Avatar");
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
