using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class ArtistViewModel : BaseViewModel
{
    public static readonly string nameVM = "ArtistVM";   

    public ArtistViewModel()
    {
        BackCommand = new(BackSwap, null);
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];

    public Artist ArtistInstance {get; private set;}
    public byte[] AvatarSource => ArtistInstance.AvatarSource.ToArray();

    public ObservableCollection<CommonInstanceDTO> ArtistPlaylists {get; private set;} = new();
    public ObservableCollection<CommonInstanceDTO> ArtistTracks {get; private set;} = new();      
        
    public CommandDelegator BackCommand { get; }
  	
    public async void SetInstance(CommonInstanceDTO artistInstanceDto)
    {
        ArtistInstance = await supporter.GetArtistAsync(artistInstanceDto);
        OnPropertyChanged("AvatarSource");

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Playlists, EntityTag.PLAYLIST)
            .Result
            .ToList()
            .ForEach(p => ArtistPlaylists.Add(p));

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => ArtistTracks.Add(t));
    }

    public async void SetInstance(Artist artist)
    {
        ArtistInstance = artist;
        OnPropertyChanged("AvatarSource");

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Playlists, EntityTag.PLAYLIST)
            .Result
            .ToList()
            .ForEach(p => ArtistPlaylists.Add(p));

        supporter.GetInstanceDTOsFromIds(ArtistInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => ArtistTracks.Add(t));
    }

    private void BackSwap(object obj)
    {
        ArtistPlaylists.Clear();
        ArtistTracks.Clear();
        MainVM.ResolveWindowStack();
    }
}
