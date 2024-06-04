using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
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
    public byte[] AvatarSource => PlaylistInstance.GetAvatar();

    public ObservableCollection<CommonInstanceDTO> PlaylistArtists {get; private set;} = new();
    public ObservableCollection<CommonInstanceDTO> PlaylistTracks {get; private set;} = new();      

    public CommandDelegator BackCommand { get; }

    public async void SetInstance(CommonInstanceDTO instanceDTO)
    {
        PlaylistInstance = await supporter.GetPlaylistAsync(instanceDTO);
        OnPropertyChanged("AvatarSource");

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Artists, EntityTag.ARTIST)
            .Result
            .ToList()
            .ForEach(p => PlaylistArtists.Add(p));

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Tracky, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => PlaylistTracks.Add(t)); 
    }

    public async void SetInstance(Playlist playlist)
    {
        PlaylistInstance = playlist;
        OnPropertyChanged("AvatarSource");

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Artists, EntityTag.ARTIST)
            .Result
            .ToList()
            .ForEach(p => PlaylistArtists.Add(p));

        supporter.GetInstanceDTOsFromIds(PlaylistInstance.Tracky, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => PlaylistTracks.Add(t)); 
    }

    private void BackSwap(object obj)
    {
        PlaylistArtists.Clear();
        PlaylistTracks.Clear();
        MainVM.ResolveWindowStack();
    }
}
