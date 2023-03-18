using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
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
    public class PlaylistFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "PlaylistFactoryVM";
        public override string NameVM => nameVM;

        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        private StoreService store => (StoreService)base.GetService("StoreService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Commands
        public CommandDelegator CreatePlaylistCommand { get; }
        public CommandDelegator CancelCommand { get; }

        public CommandDelegator SelectPlaylistArtistCommand { get; }
        public CommandDelegator DeletePlaylistArtistCommand { get; }

        public CommandDelegator SelectPlaylistTrackCommand { get; }
        public CommandDelegator DeletePlaylistTrackCommand { get; }

        public CommandDelegator PlaylistArtistExplorerCommand {get;}
        public CommandDelegator PlaylistTrackExplorerCommand {get;}
        #endregion

        #region Playlist Factory Properties
        public string PlaylistName { get; set; }
        public string PlaylistDescription { get; set; }

        public static ObservableCollection<Artist> SelectedPlaylistArtists {get;set;} = new();
        public static ObservableCollection<Track> SelectedPlaylistTracks {get; set;} = new();
        #endregion

        #region Providers        
        public static ObservableCollection<Artist> ArtistProvider { get; set; } = new();
        public static ObservableCollection<Track> TrackProvider { get; set; } = new();
        #endregion
    
        #region Log Reply Properties
        public string PlaylistLogLine { get; set; }
        public bool PlaylistLogError { get; set; }
        #endregion

        #region Properties
        public bool IsEditMode {get; private set;} = false;
        public string ViewHeader {get; private set;} = "Playlist";
        #endregion

        #region Const
        public PlaylistFactoryViewModel()
        {
            CreatePlaylistCommand = new(CreatePlaylist, null);
            CancelCommand = new(Cancel, null);

            PlaylistArtistExplorerCommand = new(OpenPlaylistArtistExplorer, null);
            PlaylistTrackExplorerCommand = new(OpenPlaylistTrackExplorer, null);

            supporterService.OnArtistsNotifyRefresh += ArtistProviderUpdate;
            supporterService.OnTracksNotifyRefresh += TrackProviderUpdate;
           
            Task.Run(InitArtists);
            Task.Run(InitTracks); 
        }
        #endregion

        #region Private Methods
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
        
        private async Task InitTracks()
        {
            TrackProvider.ToList().Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));
        }

        private void ArtistProviderUpdate()
        {
            ArtistProvider.Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));            
        }

        private void TrackProviderUpdate()
        {
            TrackProvider.Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));            
        }

        private async Task FieldsClear()
        {
            PlaylistName = default;
            PlaylistDescription = default;  
            SelectedPlaylistArtists.Clear();
            SelectedPlaylistTracks.Clear();
            PlaylistLogLine = default;
        }

        private void OnItemsSelected()
        {
            if(ExplorerVM.Output.Count > 0)
            {
                if (ExplorerVM.Output[0] is Artist)
                {
                    SelectedPlaylistArtists.Clear();
                    ExplorerVM.Output.ToList().ForEach(i => SelectedPlaylistArtists.Add((Artist)i)); 
                }
                else if(ExplorerVM.Output[0] is Track)
                {
                    SelectedPlaylistTracks.Clear();
                    ExplorerVM.Output.ToList().ForEach(i => SelectedPlaylistTracks.Add((Track)i)); 
                }
            }
        }

        private void OpenPlaylistArtistExplorer(object obj)
        {
            ExplorerVM.OnSelected += OnItemsSelected;
            if (obj is IList<ICoreEntity> preSelected)
            {
                ExplorerVM.Arrange(0, preSelected); 
            }
            else
            {
                ExplorerVM.Arrange(0); 
            }

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
        }

        private void OpenPlaylistTrackExplorer(object obj)
        {
            ExplorerVM.OnSelected += OnItemsSelected;
            if (obj is IList<ICoreEntity> preSelected)
            {
                ExplorerVM.Arrange(2, preSelected); 
            }
            else
            {
                ExplorerVM.Arrange(2); 
            }

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
        }

        #endregion

        #region Public Methods
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
                        editPlaylist.Tracks.Clear();
                        tracks.ToList().ForEach(t => editPlaylist.Tracks.Add(t.Id));
                    }

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeletePlaylist(editPlaylist.Id));
                        artists.ToList().ForEach(a => a.AddPlaylist(editPlaylist.Id));
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

        public void DropInstance(ICoreEntity entity) 
        {
            Instance = entity;
            IsEditMode = true;
            if (entity is Playlist playlist)
            {
                PlaylistName = playlist.Name;
                PlaylistDescription = playlist.Description;

                supporterService.ArtistsCollection.Where(a => a.Playlists.ToEntity(supporterService.PlaylistsCollection).Contains(playlist))
                                                  .ToList()
                                                  .ForEach(a => SelectedPlaylistArtists.Add(a));
                
                store.StoreInstance.GetTracksById(playlist.Tracks)
                               .ToList()
                               .ForEach(t => SelectedPlaylistTracks.Add(t));
            }
        }
        #endregion


        #region Command Methods
        private void Cancel(object obj)
        {
            ExitFactory();
        }


        private void CreatePlaylist(object obj)
        {
            object[] values = { PlaylistName, PlaylistDescription, SelectedPlaylistTracks, SelectedPlaylistArtists };

            if (IsEditMode == false)
                CreatePlaylistInstance(values);
            else
                EditPlaylistInstance(values);
        }
        #endregion
    }
}
