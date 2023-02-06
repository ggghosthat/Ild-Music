using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class FactoryItemViewModel : BaseViewModel
    {
        public static readonly string nameVM = "FactoryItemVM";
        public override string NameVM => nameVM;

        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        #endregion

        #region Instance
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Commands
        public CommandDelegator DefinePath { get; set; }
        public CommandDelegator CreateArtistCommand { get; }
        public CommandDelegator CreatePlaylistCommand { get; }
        public CommandDelegator CreateTrackCommand { get; }

        public CommandDelegator SelectTrackArtistCommand { get; }
        public CommandDelegator DeleteTrackArtistCommand { get; }

        public CommandDelegator SelectTrackPlaylistCommand { get; }
        public CommandDelegator DeleteTrackPlaylistCommand { get; }



        public CommandDelegator SelectPlaylistArtistCommand { get; }
        public CommandDelegator DeletePlaylistArtistCommand { get; }

        public CommandDelegator SelectPlaylistTrackCommand { get; }
        public CommandDelegator DeletePlaylistTrackCommand { get; }
        #endregion

        #region Artist Factory Properties
        public string ArtistName {get; set; }
        public string ArtistDescription { get; set; }
        #endregion

        #region Playlist Factory Properties
        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }


        public static ObservableCollection<Artist> SelectedPlaylistArtists {get;set;} = new(); 
        public SelectionModel<Artist> SelectPlaylistArtistsSelection {get;} = new();
        public SelectionModel<Artist> DeclinePlaylistArtistsSelection {get;} = new();

        public static ObservableCollection<Track> SelectedPlaylistTracks {get; set;} = new();
        public SelectionModel<Track> SelectPlaylistTracksSelection {get;} = new();
        public SelectionModel<Track> DeclinePlaylistTracksSelection {get;} = new();
        #endregion

        #region Track Factory Proeprties
        public string TrackPath { get; set; }
        public string TracktName { get; set; }
        public string TrackDescription { get; set; }

        public Artist CurrentSelectedTrackArtist { get; set; }
        public Artist CurrentDeleteTrackArtist { get; set; }

        public static ObservableCollection<Artist> SelectedTrackArtists { get; set; } = new();
        public static ObservableCollection<Playlist> SelectedTrackPlaylists { get; set; } = new();
        #endregion

        #region Providers        
        public static ObservableCollection<Artist> ArtistProvider { get; set; } = new();
        public static ObservableCollection<Playlist> PlaylistProvider { get; set; } = new();
        public static ObservableCollection<Track> TrackProvider { get; set; } = new();
        #endregion
    
        #region Log Reply Properties
        public string ArtistLogLine { get; set; }
        public bool ArtistLogError { get; set; }

        public string PlaylistLogLine { get; set; }
        public bool PlaylistLogError { get; set; }

        public string TrackLogLine { get; set; }
        public bool TrackLogError { get; set; }
        #endregion

        #region Properties
        public bool IsEditMode = false;
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Const
        public FactoryItemViewModel()
        {
            DefinePath = new(DefineTrackPath, null);
            CreateArtistCommand = new(CreateArtist, null);
            CreatePlaylistCommand = new(CreatePlaylist, null);
            CreateTrackCommand = new(CreateTrack, null);

            SelectTrackArtistCommand = new(SelectTrackArtist, null);
            DeleteTrackArtistCommand = new(DeleteTrackArtist, null);

            SelectPlaylistArtistCommand = new(SelectPlaylistArtist, null);
            DeletePlaylistArtistCommand = new(DeletePlaylistArtist, null);

            SelectPlaylistTrackCommand = new(SelectPlaylistTrack, null);
            DeletePlaylistTrackCommand = new(DeletePlaylistTrack, null);

            supporterService.OnArtistsNotifyRefresh += ArtistProviderUpdate;
            supporterService.OnPlaylistsNotifyRefresh += PlaylistProviderUpdate;
            supporterService.OnTracksNotifyRefresh += TrackProviderUpdate;

            SelectPlaylistArtistsSelection.SelectionChanged += SelectPlaylistArtist;
            DeclinePlaylistArtistsSelection.SelectionChanged += DeclinePlaylistArtist;
            DeclinePlaylistTracksSelection.SelectionChanged += DeclinePlaylistTrack;

            Task.Run(InitArtists);
            Task.Run(InitPlaylists);
            Task.Run(InitTracks); 
        }
        #endregion

        #region Private Methods
        private void SelectPlaylistArtist(object sender, SelectionModelSelectionChangedEventArgs e)
        {
            var collection = e.SelectedItems.ToList();
           
            if (collection.Count > 0)
            {
                SelectedPlaylistArtists.Clear();
                collection.ForEach(a => SelectedPlaylistArtists.Add((Artist)a));
                OnPropertyChanged("SelectedPlaylistArtists");
            }
        }


        private void DeclinePlaylistArtist(object sender, SelectionModelSelectionChangedEventArgs e)
        {
            var collection = e.SelectedIndexes.ToList();
            Console.WriteLine(SelectedPlaylistArtists.Count);

            if (collection.Count == 1 && SelectedPlaylistArtists.Count == 1)
            {
                //SelectedPlaylistArtists.Clear();
                 OnPropertyChanged("SelectedPlaylistArtists");
            }
            else if (collection.Count > 0)
            {
                //collection.ForEach(a => SelectedPlaylistArtists.RemoveAt(a-1));
                OnPropertyChanged("SelectedPlaylistArtists");
            }

        }

        private void DeclinePlaylistTrack(object sender, SelectionModelSelectionChangedEventArgs e)
        {
            
        }


        private void ExitFactory()
        {
            FieldsClear();
            MainVM.ResolveWindowStack();
        }

        private async Task InitArtists()
        {
            ArtistProvider.ToList().Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));
        }

        private async Task InitPlaylists()
        {
            PlaylistProvider.ToList().Clear();
            supporterService.PlaylistsCollection
                            .ToList().ForEach(playlist => PlaylistProvider.Add(playlist));
        }

        private async Task InitTracks()
        {
            TrackProvider.Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));
        }


        private void ArtistProviderUpdate()
        {
            ArtistProvider.Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));            
        }

        private void PlaylistProviderUpdate()
        {
            PlaylistProvider.Clear();
            supporterService.PlaylistsCollection
                            .ToList().ForEach(playlist => PlaylistProvider.Add(playlist));
        }

        private void TrackProviderUpdate()
        {
            TrackProvider.Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));            
        }

        private async Task FieldsClear()
        {
            ArtistName = default;
            ArtistDescription = default;
            ArtistLogLine = default;

            PlaylistName = default;
            PlaylistDescription = default;  
            SelectedPlaylistArtists.Clear();
            SelectedPlaylistTracks.Clear();
            PlaylistLogLine = default;

            TrackPath = default;
            TracktName = default;
            TrackDescription = default;
            SelectedTrackArtists.Clear();
            TrackLogLine = default;
        }
        #endregion
    
        #region Public Methods
        public void CreateArtistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
             
                if (!string.IsNullOrEmpty(name))
                {
                    factoryService.CreateArtist(name, description);
                    ArtistLogLine = "Successfully created!";
                    ExitFactory();
                }
            }
            catch (InvalidArtistException ex)
            {
                ArtistLogLine = ex.Message;
            }
        }

        public void EditArtistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];

                if (!string.IsNullOrEmpty(name))
                {
                    var editArtist = (Artist)Instance;
                    editArtist.Name = name;
                    editArtist.Description = description;
                    supporterService.EditInstance(editArtist); 

                    IsEditMode = false;
                    ExitFactory();                }
            }
            catch(Exception exm)
            {
                ArtistLogLine = exm.Message;
            }
        }

        public void CreatePlaylistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var tracks = (IList<Track>)values[2];
                var artists = (IList<Artist>)values[3];

                if (!string.IsNullOrEmpty(name))
                {
                    factoryService.CreatePlaylist(name, description, tracks, artists);
                    PlaylistLogLine = "Successfully created!";
                    ExitFactory(); 
                }
            }
            catch (InvalidPlaylistException ex)
            {
                PlaylistLogLine = ex.Message;
            }
        }
 
        public void EditPlaylistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var tracks = (IList<Track>)values[2];
                var artists = (IList<Artist>)values[3];

                if(!string.IsNullOrEmpty(name))
                {
                    var editPlaylist = (Playlist)Instance;
                    editPlaylist.Name = name;
                    editPlaylist.Description = description;

                    if(tracks != null && tracks.Count > 0)
                    {
                        editPlaylist.Tracks = tracks;
                    }

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeletePlaylist(editPlaylist));
                        artists.ToList().ForEach(a => a.AddPlaylist(editPlaylist));
                    }

                    supporterService.EditInstance(editPlaylist);
                    supporterService.DumpState(); 

                    IsEditMode = false;
                    ExitFactory();
                }
            }
            catch (InvalidPlaylistException ex)
            {
                PlaylistLogLine = ex.Message;
            }
        }

        public void CreateTrackInstance(object[] values)
        {
            try
            {
                var path = (string)values[0];
                var name = (string)values[1];
                var description = (string)values[2];
                var artists = (IList<Artist>)values[3];

                if (!string.IsNullOrEmpty(name))
                {
                    factoryService.CreateTrack(path, name, description, artists);
                    TrackLogLine = "Successfully created!";
                
                    ExitFactory();
                }
            }
            catch(InvalidTrackException ex)
            {
                TrackLogLine = ex.Message;
            }
        }

        public void EditTrackInstance(object[] values)
        {
            try
            {
                var path = (string)values[0];
                var name = (string)values[1];
                var description = (string)values[2];
                var artists = (IList<Artist>)values[3];

                if (!string.IsNullOrEmpty(name))
                {
                    var editTrack = (Track)Instance;
                    editTrack.Pathway = path;
                    editTrack.Name = name;
                    editTrack.Description = description;

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeleteTrack(editTrack));
                        artists.ToList().ForEach(a => a.AddTrack(editTrack));
                    }

                    supporterService.EditInstance(editTrack);
                    supporterService.DumpState(); 

                    IsEditMode = false;
                    ExitFactory();   
                }
            }
            catch(InvalidTrackException ex)
            {
                TrackLogLine = ex.Message;
            }
        }

        public void DropInstance(ICoreEntity entity) 
        {
            Instance = entity;
            IsEditMode = true;
            if (entity is Artist artist)
            {
                ArtistName = artist.Name;
                ArtistDescription = artist.Description;
            }
            else if (entity is Playlist playlist)
            {
                PlaylistName = playlist.Name;
                PlaylistDescription = playlist.Description;

                supporterService.ArtistsCollection.Where(a => a.Playlists.ToEntity(supporterService.PlaylistsCollection).Contains(playlist))
                                                  .ToList()
                                                  .ForEach(a => SelectedPlaylistArtists.Add(a));

                playlist.Tracks.ToList()
                               .ForEach(t => SelectedPlaylistTracks.Add(t));
            }
            else if (entity is Track track)
            {
                TrackPath = track.Pathway;
                TracktName = track.Name;
                TrackDescription = track.Description;

                supporterService.ArtistsCollection.Where(a => a.Tracks.ToEntity(supporterService.TracksCollection).Contains(track))
                                                  .ToList()
                                                  .ForEach(a => SelectedPlaylistArtists.Add(a));
            }
        }   
        #endregion


        #region Command Methods
        private async void DefineTrackPath(object obj)
        {
            OpenFileDialog dialog = new();
            string[] result = await dialog.ShowAsync(new Window());
            TrackPath = string.Join(" ", result);
        }

        private void CreateArtist(object obj)
        {
            object[] value = { ArtistName, ArtistDescription };

            if (IsEditMode == false)
                CreateArtistInstance(value);
            else 
                EditArtistInstance(value);
            Task.Run(ArtistProviderUpdate);
        }

        private void CreatePlaylist(object obj)
        {
            object[] value = { PlaylistName, PlaylistDescription, SelectedPlaylistTracks, SelectedPlaylistArtists };

            if (IsEditMode == false)
                CreatePlaylistInstance(value);
            else
                EditPlaylistInstance(value);
            Task.Run(PlaylistProviderUpdate);
        }

        private void CreateTrack(object obj)
        {
            object[] value = { TrackPath, TracktName, TrackDescription, SelectedTrackArtists };

            if (IsEditMode == false)
                CreateTrackInstance(value);
            else
                EditTrackInstance(value);
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

        private void SelectPlaylistTrack(object obj)
        {
            if (obj is Track track)
                SelectedPlaylistTracks.Add(track);
        }

        private void DeletePlaylistTrack(object obj)
        {
            if (obj is Track track)
                SelectedPlaylistTracks.Remove(track);
        }
        #endregion
    }
}
