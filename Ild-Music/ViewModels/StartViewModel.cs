using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class StartViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    private Task trackDropTask;
    private CancellationTokenSource tokenSource = new CancellationTokenSource();
    
    private static SupportGhost _supportGhost => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public ObservableCollection<CommonInstanceDTO> Artists {get; set;} = new ();
    public ObservableCollection<CommonInstanceDTO> Playlists {get; set;} = new ();
    public ObservableCollection<CommonInstanceDTO> Tracks {get; set;} = new ();

    public CommonInstanceDTO CurrentArtist { get; set; }
    public CommonInstanceDTO CurrentPlaylist { get; set; }
    public CommonInstanceDTO CurrentTrack { get; set; }
    public CommandDelegator DropPlaylistCommand {get;}
    public CommandDelegator DropTrackCommand {get;}
    public CommandDelegator DropArtistCommand {get;}

    public StartViewModel()
    {
        DropPlaylistCommand = new(DropPlaylist, null);
        DropTrackCommand = new(DropTrack, null);
        DropArtistCommand = new(DropArtist, null);
        
        _supportGhost.OnArtistsNotifyRefresh += RefreshArtists;
        _supportGhost.OnPlaylistsNotifyRefresh += RefreshPlaylists;
        _supportGhost.OnTracksNotifyRefresh += RefreshTracks;

        Task.Run(PopullateLists);
    }

    private async Task PopullateLists()
    {
        _supportGhost.ArtistsCollection?.ToList().ForEach(a => Artists.Add(a));
        _supportGhost.PlaylistsCollection?.ToList().ForEach(p => Playlists.Add(p));
        _supportGhost.TracksCollection?.ToList().ForEach(t => Tracks.Add(t));         
    }

    private void RefreshArtists()
    {
        Artists.Clear();
        _supportGhost.ArtistsCollection.ToList().ForEach(a => Artists.Add(a));
    }

    private void RefreshPlaylists()
    {
        Playlists.Clear();
        _supportGhost.PlaylistsCollection.ToList().ForEach(p => Playlists.Add(p));
    }

    private void RefreshTracks()
    {
        Tracks.Clear();
        _supportGhost.TracksCollection.ToList().ForEach(t => Tracks.Add(t));
    }

    public async Task BrowseTracks(IEnumerable<string> paths)
    {
        paths.ToList().ForEach(path => factory.CreateTrack(path));
        RefreshTracks();
    }

    private void DropArtist(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.ARTIST)
            Task.Run(() => MainVM.ResolveInstance(this, instanceDTO));
    }

    private void DropPlaylist(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.PLAYLIST)
        {
            var playlist = _supportGhost.GetPlaylistAsync(instanceDTO).Result;
            Task.Run(() => MainVM.DropPlaylistInstance(this, playlist));
        }
    }

    private void DropTrack(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.TRACK)
        {
            var track = _supportGhost.GetTrackAsync(instanceDTO).Result;
            Task.Run(() => MainVM.DropTrackInstance(this, track));
        }
    }
}
