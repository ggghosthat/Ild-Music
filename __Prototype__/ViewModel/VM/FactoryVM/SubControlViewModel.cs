using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class SubControlViewModel : Base.BaseViewModel
    {
        #region Fields

        public static readonly string nameVM = "SubControlVM";

        public SupporterService Supporter => (SupporterService)GetService("Supporter");
        private FactoryService factoryService => (FactoryService)GetService("Factory");
        private ViewModelHolder vmHolder => App.vmHolder;

        private ListViewModel listVM => (ListViewModel)vmHolder.GetViewModel(ListViewModel.nameVM);
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

        public event Action OnInitProvider;

        #region Const
        public SubControlViewModel()
        {
            CreateArtistCommand = new(CreateArtist, null);
            CreatePlaylistCommand = new(CreatePlaylist, null);
            CreateTrackCommand = new(CreateTrack, null);

            SelectTrackArtistCommand = new(SelectTrackArtist, null);
            SelectTrackPlaylistCommand = new(SelectTrackPlaylist, null);

            DeleteTrackArtistCommand = new(DeleteTrackArtist, null);
            DeleteTrackPlaylistCommand = new(DeleteTrackPlaylist, null);

            SelectPlaylistArtistCommand = new(SelectPlaylistArtist, null);
            DeletePlaylistArtistCommand = new(DeletePlaylistArtist, null);

            Task.Run(InitArtists);
            Task.Run(InitPlaylists);
        }
        #endregion


        #region Private Methods
        private void InitPlaylists()
        {
            PlaylistProvider.ToList().Clear();
            Supporter.PlaylistSup.ToList().ForEach(playlist => PlaylistProvider.Add(playlist));
        }

        private void InitArtists()
        {
            ArtistProvider.Clear();
            Supporter.ArtistSup.ToList().ForEach(artist => ArtistProvider.Add(artist));
        }


        private void PlaylistProviderUpdate()
        {
            PlaylistProvider.ToList().Clear();
            Supporter.PlaylistSup.ToList().ForEach(playlist => PlaylistProvider.ToList().Add(playlist));
        }

        private void ArtistProviderUpdate()
        {
            ArtistProvider.Clear();
            Supporter.ArtistSup.ToList().ForEach(artist => ArtistProvider.ToList().Add(artist));            
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
            var tracks = (IList<Artist>)values[2];
            factoryService.CreatePlaylist(name, description, tracks);
        }

        public void CreateTrackInstance(object[] values)
        {
            var path = (string)values[0];
            var name = (string)values[1];
            var description = (string)values[2];
            var artists = (IList<Artist>)values[3];
            var playlists = (IList<Playlist>)values[4];
            factoryService.CreateTrack(path, name, description, artists, playlists);
        }

        public void DropInstance(ICoreEntity instance) =>
            Instance = instance;
        #endregion

        #region Command Methods
        private void CreateArtist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription};


            CreateArtistInstance(value);

            listVM.SetListType(List.ARTISTS);
            Task.Run(ArtistProviderUpdate);
        }

        private void CreatePlaylist(object obj)
        {
            object[] value = { PlaylistName, PlaylistDescription, SelectedPlaylistArtists };

            CreatePlaylistInstance(value);
            listVM.SetListType(List.PLAYLISTS);
            Task.Run(PlaylistProviderUpdate);
        }

        private void CreateTrack(object obj)
        {
            object[] value = { TrackPath, TracktName, TrackDescription, SelectedTrackArtists, SelectedTrackPlaylists };

            CreateTrackInstance(value);
            listVM.SetListType(List.TRACKS);
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

    }
}
