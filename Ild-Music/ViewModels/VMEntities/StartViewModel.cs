using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using ShareInstances.Services.Entities;
using ShareInstances.PlayerResources;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        public static readonly string nameVM = "StartVM";
        public override string NameVM => nameVM;
        
        #region Services
        private SupporterService supporter => (SupporterService)App.Stage.GetServiceInstance("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
        public ObservableCollection<Artist> Artists {get; set;} = new ();
        public ObservableCollection<Playlist> Playlists {get; set;} = new ();
        public ObservableCollection<Track> Tracks {get; set;} = new ();

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

            Task.Run(PopullateLists);
            supporter.OnArtistsNotifyRefresh += RefreshArtists;
            supporter.OnPlaylistsNotifyRefresh += RefreshPlaylists;
            supporter.OnTracksNotifyRefresh += RefreshTracks;
        }
        #endregion

        #region  Private Methods
        private void PopullateLists()
        {
            supporter.ArtistsCollection.ToList().ForEach(a => Artists.Add(a));
            supporter.PlaylistsCollection.ToList().ForEach(p => Playlists.Add(p));
            supporter.TracksCollection.ToList().ForEach(t => Tracks.Add(t));         
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

        #region Command Methods
        private void DropPlaylist(object obj)
        {
            if (obj is Playlist playlist)
            {
                new Task(() => MainVM.DropInstance(this, playlist)).Start(); 
            }
        }

        private void DropTrack(object obj)
        {
            if (obj is Track track)
            {
                new Task(() => MainVM.DropInstance(this, track)).Start();
            }
        }

        private void DropArtist(object obj)
        {
            if (obj is Artist artist)
            {
                new Task(() => MainVM.DropInstance(this, artist));
            }
        }
        #endregion
    }
}
