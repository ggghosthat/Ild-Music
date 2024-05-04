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

namespace Ild_Music.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public static readonly string nameVM = "StartVM";
        public override string NameVM => nameVM;

        #region AsyncStaff
        private Task trackDropTask;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        #endregion
        
        #region Services
        private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
        public ObservableCollection<CommonInstanceDTO> Artists {get; set;} = new ();
        public ObservableCollection<CommonInstanceDTO> Playlists {get; set;} = new ();
        public ObservableCollection<CommonInstanceDTO> Tracks {get; set;} = new ();

        public Artist CurrentArtist { get; set; }
        public Playlist CurrentPlaylist { get; set; }
        public Track CurrentTrack { get; set; }
        #endregion

        #region Commands
        public CommandDelegator DropPlaylistCommand {get;}
        public CommandDelegator DropTrackCommand {get;}
        public CommandDelegator DropArtistCommand {get;}
        #endregion

        #region Ctor
        public StartViewModel()
        {
            DropPlaylistCommand = new(DropPlaylist, null);
            DropTrackCommand = new(DropTrack, null);
            DropArtistCommand = new(DropArtist, null);
        
            
            supporter.OnArtistsNotifyRefresh += RefreshArtists;
            supporter.OnPlaylistsNotifyRefresh += RefreshPlaylists;
            supporter.OnTracksNotifyRefresh += RefreshTracks;

            Task.Run(PopullateLists);
        }
        #endregion

        #region  Private Methods
        private async Task PopullateLists()
        {
            supporter.ArtistsCollection?.ToList().ForEach(a => Artists.Add(a));
            supporter.PlaylistsCollection?.ToList().ForEach(p => Playlists.Add(p));
            supporter.TracksCollection?.ToList().ForEach(t => Tracks.Add(t));         
        }

        private void RefreshArtists()
        {
            Artists.Clear();
            supporter.ArtistsCollection.ToList().ForEach(a => Artists.Add(a));
        }

        private void RefreshPlaylists()
        {
            Playlists.Clear();
            supporter.PlaylistsCollection.ToList().ForEach(p => Playlists.Add(p));
        }

        private void RefreshTracks()
        {
            Tracks.Clear();
            supporter.TracksCollection.ToList().ForEach(t => Tracks.Add(t));
        }
        #endregion

        #region Public Methods
        public async Task BrowseTracks(IEnumerable<string> paths)
        {
            paths.ToList()
                 .ForEach(path => factory.CreateTrack(path));
            RefreshTracks();
        }
        #endregion

        #region Command Methods        
        private void DropArtist(object obj)
        {
            if (obj is Artist artist)
                Task.Run(() => MainVM.ResolveArtistInstance(this, artist));
        }

        private void DropPlaylist(object obj)
        {
            if (obj is Playlist playlist)
                Task.Run(() => MainVM.DropPlaylistInstance(this, playlist)); 
        }

        private void DropTrack(object obj)
        {
            if (obj is Track track)
                Task.Run(() => MainVM.DropTrackInstance(this, track));
        }
        #endregion
    }
}
