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
    public class PlaylistFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "PlaylistFactoryVM";
        public override string NameVM => nameVM;

        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Commands
        public CommandDelegator CreatePlaylistCommand { get; }

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
        public SelectionModel<Artist> SelectPlaylistArtistsSelection {get;} = new();
        public SelectionModel<Artist> DeclinePlaylistArtistsSelection {get;} = new();

        public static ObservableCollection<Track> SelectedPlaylistTracks {get; set;} = new();
        public SelectionModel<Track> SelectPlaylistTracksSelection {get;} = new();
        public SelectionModel<Track> DeclinePlaylistTracksSelection {get;} = new();
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
        public bool IsEditMode = false;
        #endregion

        #region Const
        public FactoryItemViewModel()
        {
            CreatePlaylistCommand = new(CreatePlaylist, null);

            SelectPlaylistArtistCommand = new(SelectPlaylistArtist, null);
            DeletePlaylistArtistCommand = new(DeletePlaylistArtist, null);

            SelectPlaylistTrackCommand = new(SelectPlaylistTrack, null);
            DeletePlaylistTrackCommand = new(DeletePlaylistTrack, null);

            PlaylistArtistExplorerCommand = new(OpenPlaylistArtistExplorer, null);
            PlaylistTrackExplorerCommand = new(OpenPlaylistTrackExplorer, null);
            TrackArtistExplorerCommand = new(OpenTrackArtistExplorer, null);

            supporterService.OnArtistsNotifyRefresh += ArtistProviderUpdate;
            supporterService.OnTracksNotifyRefresh += TrackProviderUpdate;

            Task.Run(InitArtists);
            Task.Run(InitTracks); 
        }
        #endregion

    }
}
