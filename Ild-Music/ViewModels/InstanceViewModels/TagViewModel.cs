using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class TagViewModel : BaseViewModel
{
    public static readonly string nameVM = "TagVM";

    public TagViewModel()
    {
        BackCommand = new(BackSwap, null);
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];

    public Tag TagInstance { get; private set; }

    public ObservableCollection<CommonInstanceDTO> TagArtists { get; private set; } = new();
    public ObservableCollection<CommonInstanceDTO> TagPlaylists { get; private set; } = new();
    public ObservableCollection<CommonInstanceDTO> TagTracks { get; private set; } = new();

    public CommandDelegator BackCommand { get; }

    public async void SetInstance(Tag tag)
    {
        TagInstance = tag;
        OnPropertyChanged("AvatarSource");

        supporter.GetInstanceDTOsFromIds(TagInstance.Artists, EntityTag.ARTIST)
            .Result
            .ToList()
            .ForEach(a => TagArtists.Add(a));

        supporter.GetInstanceDTOsFromIds(TagInstance.Playlists, EntityTag.PLAYLIST)
            .Result
            .ToList()
            .ForEach(p => TagPlaylists.Add(p));

        supporter.GetInstanceDTOsFromIds(TagInstance.Tracks, EntityTag.TRACK)
            .Result
            .ToList()
            .ForEach(t => TagTracks.Add(t));
    }
    
    private void BackSwap(object obj)
    {
        TagArtists.Clear();
        TagPlaylists.Clear();
        TagTracks.Clear();
        MainVM.ResolveWindowStack();
    }
}
