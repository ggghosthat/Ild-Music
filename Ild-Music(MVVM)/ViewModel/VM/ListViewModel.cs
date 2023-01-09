using Ild_Music_MVVM_.Services;
using System.Collections.ObjectModel;
using Ild_Music_MVVM_.Command;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.PlayerResources;
using System.Collections.Generic;
using System.Linq;
using ShareInstances;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using System.Threading.Tasks;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    //Types of Lists
    public enum List
    {
        ARTISTS,
        PLAYLISTS,
        TRACKS
    }

    public class ListViewModel : Base.BaseViewModel
    {
        #region Fields
        public static readonly string nameVM = "ListVM";
        private static IPlayer _playerInstance;

        //SupporterService wich provide entities supply 2 list representation
        private SupporterService supporterService => (SupporterService)GetService("Supporter");
        private MainWindowService _mainWindowAPI => (MainWindowService)GetService("MainWindowAPI");
        private TabHolderService tabHolderService => (TabHolderService)GetService("TabHolder");
        private PlayerService playerService => (PlayerService)GetService("PlayerService");
        private ViewModelHolder vmHolder => App.vmHolder;

        private List listType;
        #endregion

        #region Properties

        public CommandDelegater AddCommand { get; }
        public CommandDelegater DeleteCommand { get; }
        public CommandDelegater BackCommand { get; }
        public CommandDelegater ItemSelectCommand { get; }
        public CommandDelegater DropStaffCommand { get; }

        public List ListType => listType;

        private static BackList<IEnumerable<ICoreEntity>> _storage = new();
        public bool IsStackEmpty = _storage.Count == 0;

        public static ObservableCollection<ICoreEntity> CurrentList { get; set; } = new();
        public ICoreEntity SelectedItem { get; set; }

        
        public object Icon { get; set; }

        #endregion


        #region Ctors
        //Postdefinning ListType Constructor

        public ListViewModel()
        {
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);
            ItemSelectCommand = new(ItemSelect, null);
            DropStaffCommand = new(DropStaff, null);
        }
        #endregion

        #region private methods
        private void InitCurrentList(List listType)
        {
            CurrentList.Clear();
            switch (listType)
            {
                case List.ARTISTS:
                    foreach (var a in supporterService.ArtistSup)
                        CurrentList.Add(a);
                    break;
                case List.PLAYLISTS:
                    foreach (var p in supporterService.PlaylistSup)
                        CurrentList.Add(p);
                    break;
                case List.TRACKS:
                    foreach (var t in supporterService.TrackSup)
                        CurrentList.Add(t);
                    break;
            }
        }

        private FactorySubControlTab DisplayListType()
        {
            switch (listType)
            {
                case List.ARTISTS:
                    return tabHolderService.TabHolder.DisplayInstance(0);
                case List.PLAYLISTS:
                    return tabHolderService.TabHolder.DisplayInstance(1);
                case List.TRACKS:
                    return tabHolderService.TabHolder.DisplayInstance(2);
            }

            return null;
        }

        private void ProcessItemSelection()
        {
            switch (listType)
            {
                case List.ARTISTS:
                    ProcessArtistType();
                    break;
                case List.PLAYLISTS:
                    ProcessPlaylistType();
                    break;
                case List.TRACKS:
                    ProcessTrackType();
                    break;
                default:
                    break;
            }
        }

        private void ProcessArtistType()
        {
            if (SelectedItem is Artist artist)            
                ArtistInstanceProcess(artist);
            
            if (SelectedItem is Playlist playlist)
                PlaylistInstanceProcess(playlist);
            
            if (SelectedItem is Track track)
                TrackInstanceProcess(track);
            

        }

        private void ProcessPlaylistType()
        {
            var playlist = (Playlist)SelectedItem;
            PlaylistInstanceProcess(playlist);
        }

        private void ProcessTrackType()
        {
            var track = (Track)SelectedItem;
            TrackInstanceProcess(track);
        }

        #endregion

        #region Public Methods
        //Define type of list to present in CurrentList
        public void SetListType(List listType)
        {
            this.listType = listType;
            InitCurrentList(listType);
        }    
        #endregion

        #region Command Methods

        private void Add(object obj)
        {
            Task.Run(() =>
            {
                var tab = DisplayListType();
                var factoryVM = (FactoryContainerViewModel)vmHolder.GetViewModel(FactoryContainerViewModel.nameVM);

                factoryVM.OrderSingleTab(tab);
                _mainWindowAPI.MainWindow.ResetVM(factoryVM);
            });           
        }        

        private void Delete(object obj)
        {
            supporterService.RemoveInstanceObject(SelectedItem);
            SetListType(listType);
        }
        private void Back(object obj) 
        {
            if (_storage.Count > 0)
            {
                var previous = _storage.Peek();
                CurrentList.Clear();
                foreach (var prev in previous.ToList())
                    CurrentList.Add(prev);
            }
        }
        private void ItemSelect(object obj)
        {
            if (SelectedItem != null)
            {
                if (_storage.Count >= 0)
                {
                    var current = CurrentList;
                    _storage.Add(current.ToList());
                }
                ProcessItemSelection();
            }
        }
        private void DropStaff(object obj)
        {
            _playerInstance = playerService.GetPlayer();
            if (obj is Playlist playerPlaylist && playerPlaylist.Count > 0) 
            {
                _playerInstance.SetPlaylistInstance(playerPlaylist);
                _playerInstance.Play();
            }
            if (obj is Track playerTrack) 
            {
                _playerInstance.SetTrackInstance(playerTrack);
                _playerInstance.Play();
            }
        }
        #endregion


        #region InstanceProcess
        private void ArtistInstanceProcess(Artist artist)
        {
            if ((artist.Playlists.Count == 0) &&
                    (artist.Tracks.Count == 0))
            {
                CurrentList.Clear();
                return;
            }

            CurrentList.Clear();
            artist.Playlists.ToList().ForEach(artistPlaylist => CurrentList.Add(artistPlaylist));
            artist.Tracks.ToList().ForEach(artistTrack => CurrentList.Add(artistTrack));
        }

        private void PlaylistInstanceProcess(Playlist playlist)
        {
            var artists = supporterService.ArtistSup.Where((artist) => artist.Playlists.Contains(playlist))
                                                    .ToList();

            CurrentList.Clear();

            foreach (var artistInstance in artists.ToList())
                CurrentList.Add(artistInstance);

            playlist.Tracks.ToList().ForEach(track => CurrentList.Add(track));
        }

        private void TrackInstanceProcess(Track track)
        {
            CurrentList.Clear();
        }
        #endregion
    }
}