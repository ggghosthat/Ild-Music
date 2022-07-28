using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class SubControlViewModel : Base.BaseViewModel
    {
        #region Fields
        private FactoryService factoryService => (FactoryService)GetService("Factory");
        public SupporterService Supporter => (SupporterService)GetService("Supporter");
        #endregion

        #region  Properties
        public ICoreEntity Instance { get; private set; } = null;

        public CommandDelegater CreateArtistCommand { get; }
        public CommandDelegater CreatePlaylistCommand { get; }
        public CommandDelegater CreateTrackCommand { get; }

        public CommandDelegater SelectTrackArtistCommand { get; }
        public CommandDelegater SelectTrackPlaylistCommand { get; }

        public CommandDelegater DeleteTrackArtistCommand { get; }
        public CommandDelegater DeleteTrackPlaylistCommand { get; }

        public CommandDelegater SelectPlaylistArtistCommand { get; }
        public CommandDelegater DeletePlaylistArtistCommand { get; }
        #endregion

        #region Artist Factory Properties
        public string ArtistName { get; set; }
        public string ArtistDescription { get; set; }
        #endregion

        #region Playlist Factory Properties
        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }


        public Artist CurrentSelectedPlaylistArtist { get; set; }
        public Artist CurrentDeletePlaylistArtist { get; set; }

        public static ObservableCollection<Artist> SelectedPlaylistArtists { get; set; } = new();
        #endregion

        #region Track Factory Proeprties
        public string TrackPath { get; set; }
        public string TracktName { get; set; }
        public string TrackDescription { get; set; }

        public Artist CurrentSelectedTrackArtist { get; set; }
        public Playlist CurrentSelectedTrackPlaylist { get; set; }

        public Artist CurrentDeleteTrackArtist { get; set; }
        public Playlist CurrentDeleteTrackPlaylist { get; set; }

        public static ObservableCollection<Artist> SelectedTrackArtists { get; set; } = new();
        public static ObservableCollection<Playlist> SelectedTrackPlaylists { get; set; } = new();
        #endregion

        public static ObservableCollection<Playlist> PlaylistProvider { get; set; } = new();
        public static ObservableCollection<Artist> ArtistProvider { get; set; } = new();

        #region Const
        public SubControlViewModel()
        {
            TestTrackPlaylist();
            CreateArtistCommand = new(CreateArtist, null);
            CreatePlaylistCommand = new(CreatePlaylist, null);
            CreateTrackCommand = new(CreateTrack, null);

            SelectTrackArtistCommand = new(SelectTrackArtist, null);
            SelectTrackPlaylistCommand = new(SelectTrackPlaylist, null);

            DeleteTrackArtistCommand = new(DeleteTrackArtist, null);
            DeleteTrackPlaylistCommand = new(DeleteTrackPlaylist, null);

            SelectPlaylistArtistCommand = new(SelectPlaylistArtist, null);
            DeletePlaylistArtistCommand = new(DeletePlaylistArtist, null);
        }
        #endregion


        #region Public Methods
        public void CreateArtistInstance(object[] values)
        {
            var name = (string)values[0];
            var description = (string)values[1];
            factoryService.CreateArtist(name, description);
        }

        public void CreatePlaylistInstance(object[] values)
        {
            var name = (string)values[0];
            var description = (string)values[1];
            var tracks = (values.Length == 2) ? (IList<Artist>)values[2] : null;
            factoryService.CreatePlaylist(name, description, tracks);
        }

        public void CreateTrackInstance(object[] values)
        {
            var path = (string)values[0];
            var name = (string)values[1];
            var description = (string)values[2];
            var artists =(values[3] != null) ? (IList<Artist>)values[3] : null;
            var playlists = (values[4] != null) ? (IList<Playlist>)values[4] : null;
            factoryService.CreateTrack(path, name, description, artists, playlists);
        }

        public void DropInstance(ICoreEntity instance) =>
            Instance = instance;
        #endregion

        #region Command Methods
        private void CreateArtist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription};

            foreach (var item in value)
            {
                Debug.WriteLine(item.ToString());
            }

            //CreateArtistInstance(value);
        }

        private void CreatePlaylist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription, SelectedPlaylistArtists };

            foreach (var item in value)
            {
                Debug.WriteLine(item.ToString());
            }
            //CreatePlaylistInstance(value);
        }

        private void CreateTrack(object obj)
        {
            object[] value = { TrackPath, TracktName, TrackDescription, SelectedTrackArtists, SelectedTrackPlaylists };

            foreach (var item in value)
            {
                Debug.WriteLine(item.ToString());
            }
            //CreateTrackInstance(value);
        }



        private void SelectTrackArtist(object obj)
        {
            if (obj is Artist artist)
                SelectedTrackArtists.Add(artist);
        }

        private void SelectTrackPlaylist(object obj)
        {
            if (obj is Playlist playlist)
                SelectedTrackPlaylists.Add(playlist);            
        }

        private void DeleteTrackArtist(object obj)
        {
            if (obj is Artist artist)
                SelectedTrackArtists.Remove(artist);
        }

        private void DeleteTrackPlaylist(object obj)
        {
            if (obj is Playlist playlist)
                SelectedTrackPlaylists.Remove(playlist);
        }


        private void SelectPlaylistArtist(object obj)
        {
            if (obj is Artist artist)
                SelectedPlaylistArtists.Add(artist);
        }

        private void DeletePlaylistArtist(object obj)
        {
            if (obj is Artist artist)
                SelectedPlaylistArtists.Remove(artist);
        }
        #endregion

        #region Test Methods
        private void TestTrackPlaylist()
        {
            PlaylistProvider.Add(new Playlist("P_Name_one", "***"));
            PlaylistProvider.Add(new Playlist("P_Name_two", "***"));
            PlaylistProvider.Add(new Playlist("P_Name_three", "***"));

            ArtistProvider.Add(new Artist("A_Name_one", "***"));
            ArtistProvider.Add(new Artist("A_Name_one", "***"));
            ArtistProvider.Add(new Artist("A_Name_one", "***"));
        }
        #endregion
    }
}
