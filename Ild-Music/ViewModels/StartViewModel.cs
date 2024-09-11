using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Contracts;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class StartViewModel : BaseViewModel, IFileDropable
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;
   
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

    private static SupportGhost _supportGhost => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel mainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public ObservableCollection<CommonInstanceDTO> Artists {get; set;} = new ();

    public ObservableCollection<CommonInstanceDTO> Playlists {get; set;} = new ();
    
    public ObservableCollection<CommonInstanceDTO> Tracks {get; set;} = new ();

    public CommonInstanceDTO CurrentArtist { get; set; }
    
    public CommonInstanceDTO CurrentPlaylist { get; set; }
    
    public CommonInstanceDTO CurrentTrack { get; set; }
    
    public CommandDelegator DropPlaylistCommand {get;}
    
    public CommandDelegator DropTrackCommand {get;}
    
    public CommandDelegator DropArtistCommand {get;}
    
    public override void Load()
    {
        Task.Run(PopullateLists);
    }

    private async Task PopullateLists()
    {        
        Artists.Clear();
        Playlists.Clear();
        Tracks.Clear();
        
        using (var instancePool = await _supportGhost.GetInstancePool())
        {
           instancePool.ArtistsDTOs.ToList().ForEach(a => Artists.Add(a));
           instancePool.PlaylistsDTOs.ToList().ForEach(p => Playlists.Add(p));
           instancePool.TracksDTOs.ToList().ForEach(t => Tracks.Add(t));
        }
    }

    private void RefreshArtists()
    {
        using (var instancePool = _supportGhost.GetInstancePool().Result)
        {
            Artists.Clear();
            instancePool.ArtistsDTOs.ToList().ForEach(a => Artists.Add(a));    
        }
    }

    private void RefreshPlaylists()
    {
        using (var instancePool = _supportGhost.GetInstancePool().Result)
        {
            Playlists.Clear();
            instancePool.PlaylistsDTOs.ToList().ForEach(p => Playlists.Add(p));  
        }
    }

    private void RefreshTracks()
    {
        using (var instancePool = _supportGhost.GetInstancePool().Result)
        {
            Tracks.Clear();
            instancePool.TracksDTOs.ToList().ForEach(t => Tracks.Add(t));  
        }
    }

    public void DropFile(string filePath)
    {
        factory.CreateTrackBrowsed(filePath, true);
        RefreshTracks();
    }

    public void DropFiles(IEnumerable<string> filePaths)
    {
        Task.Run(() => filePaths.ToList().ForEach(path => factory.CreateTrackBrowsed(path, true)));
        RefreshTracks();
    }

    private void DropArtist(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.ARTIST)
            Task.Run(() => mainVM.ResolveInstance(this, instanceDTO));
    }

    private void DropPlaylist(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.PLAYLIST)
        {
            var playlist = _supportGhost.GetPlaylistAsync(instanceDTO).Result;
            mainVM.DropPlaylistInstance(this, playlist, false);
        }
    }

    private void DropTrack(object obj)
    {
        if (obj is CommonInstanceDTO instanceDTO && instanceDTO.Tag is EntityTag.TRACK)
        {
            var track = _supportGhost.GetTrackAsync(instanceDTO).Result;
            mainVM.DropTrackInstance(this, track, false);
        }            
    }
}