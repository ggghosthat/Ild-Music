using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class PlaylistViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    public PlaylistViewModel()
	{
        BackCommand = new(BackSwap, null);
	}

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

	public Playlist PlaylistInstance {get; private set;}
    
    public string Name => PlaylistInstance.Name.ToString();
    
    public string Description => PlaylistInstance.Description.ToString();
    
    public int Year => PlaylistInstance.Year;
    
    public string AvatarPath => PlaylistInstance.AvatarPath.ToString();

    public byte[] Avatar { get; private set; }

    public ObservableCollection<CommonInstanceDTO> PlaylistArtists {get; private set;} = new();
    
    public ObservableCollection<CommonInstanceDTO> PlaylistTracks {get; private set;} = new();      

    public CommandDelegator BackCommand { get; }

    public async void SetInstance(CommonInstanceDTO instanceDto)
    {
        PlaylistInstance = await supporter.GetPlaylistAsync(instanceDto);

        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            Avatar = new byte[fs.Length];
            await fs.ReadAsync(Avatar, 0, (int)fs.Length);
        }

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Artists, EntityTag.ARTIST)
            .Result
            .ToList()
            .ForEach(p => PlaylistArtists.Add(p));

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => PlaylistTracks.Add(t)); 
    }

    public async void SetInstance(Playlist playlist)
    {
        PlaylistInstance = playlist;

        if(File.Exists(AvatarPath))
        {
            using var fs= new FileStream(AvatarPath, FileMode.Open);
            Avatar = new byte[fs.Length];
            await fs.ReadAsync(Avatar, 0, (int)fs.Length);
        }
        
        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Artists, EntityTag.ARTIST)
            .Result
            .ToList()
            .ForEach(p => PlaylistArtists.Add(p));

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => PlaylistTracks.Add(t)); 
    }

    private void BackSwap(object obj)
    {
        PlaylistArtists.Clear();
        PlaylistTracks.Clear();
        Avatar = new byte[0];
        PlaylistInstance = default;
        MainVM.ResolveWindowStack();
    }
}
