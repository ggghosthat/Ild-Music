using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class ArtistViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId; 

    public ArtistViewModel()
    {
        BackCommand = new(BackSwap, null);
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public Artist ArtistInstance {get; private set;}

    public string Name => ArtistInstance.Name.ToString();
    
    public string Description => ArtistInstance.Description.ToString();
    
    public int Year => ArtistInstance.Year;
    
    public string AvatarPath => ArtistInstance.AvatarPath.ToString();

    public byte[] Avatar { get; private set; }

    public ObservableCollection<CommonInstanceDTO> ArtistPlaylists { get; private set; } = new();

    public ObservableCollection<CommonInstanceDTO> ArtistTracks { get; private set; } = new();      
        
    public CommandDelegator BackCommand { get; }
  	    
    public async void SetInstance(CommonInstanceDTO instanceDto)
    {
        ArtistInstance = await supporter.GetArtistAsync(instanceDto);

        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            Avatar = new byte[fs.Length];
            await fs.ReadAsync(Avatar, 0, (int)fs.Length);
        }

        if (ArtistPlaylists.Count > 0)
            ArtistPlaylists.Clear();

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Playlists, EntityTag.PLAYLIST)
            .Result
            .ToList()
            .ForEach(p => ArtistPlaylists.Add(p));

        if (ArtistTracks.Count > 0)
            ArtistTracks.Clear();

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => ArtistTracks.Add(t));
    }

    public async void SetInstance(Artist artist)
    {
        ArtistInstance = artist;

        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            Avatar = new byte[fs.Length];
            await fs.ReadAsync(Avatar, 0, (int)fs.Length);
        }
        
        if (ArtistPlaylists.Count > 0)
            ArtistPlaylists.Clear();

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Playlists, EntityTag.PLAYLIST)
            .Result
            .ToList()
            .ForEach(p => ArtistPlaylists.Add(p));

        
        if (ArtistTracks.Count > 0)
            ArtistTracks.Clear();

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => ArtistTracks.Add(t));
    }

    private void BackSwap(object obj)
    {
        ArtistPlaylists.Clear();
        ArtistTracks.Clear();
        Avatar = new byte[0];
        ArtistInstance = default;
        MainVM.ResolveWindowStack();
    }
}
