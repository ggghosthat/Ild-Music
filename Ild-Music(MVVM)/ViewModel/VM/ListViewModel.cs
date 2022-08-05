using Ild_Music_MVVM_.Services;
using System.Collections.ObjectModel;
using Ild_Music_MVVM_.Command;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.PlayerResources;
using System.Collections.Generic;
using System.Linq;
using ShareInstances;

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
        private static readonly string nameVM = "ListVM";
        private static IPlayer _playerInstance;

        //SupporterService wich provide entities supply 2 list representation
        private SupporterService supporterService;
        private MainWindowService _mainWindowAPI => (MainWindowService)GetService("MainWindowAPI");
        private FactoryService factoryService => (FactoryService)GetService("Factory");
        private ViewModelHolderService vmHolder => (ViewModelHolderService)GetService("VMHolder");
        private PlayerService playerService => (PlayerService)GetService("PlayerService");

        private List listType;
        #endregion

        #region Properties
        public static string NameVM => nameVM;


        public CommandDelegater AddCommand { get; }
        public CommandDelegater DeleteCommand { get; }
        public CommandDelegater BackCommand { get; }
        public CommandDelegater ItemSelectCommand { get; }
        public CommandDelegater DropStaffCommand { get; }


        private static Stack<IEnumerable<ICoreEntity>> _storage = new();
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

        public ListViewModel(List listType)
        {
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);
            ItemSelectCommand = new(ItemSelect, null);
            DropStaffCommand = new(DropStaff, null);

            supporterService = (SupporterService)GetService("Supporter");
            SetListType(listType);
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

        private void DisplayListType()
        {
            switch (listType)
            {
                case List.ARTISTS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(0);
                    break;
                case List.PLAYLISTS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(1);
                    break;
                case List.TRACKS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(2);
                    break;
            }
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
            var artist = (Artist)SelectedItem;

            if ((artist.Playlists.Count == 0) &&
                (artist.Tracks.Count == 0))
                return;

            CurrentList.Clear();
            artist.Playlists.ToList().ForEach(artistPlaylist => CurrentList.Add(artistPlaylist));
            artist.Tracks.ToList().ForEach(artistTrack => CurrentList.Add(artistTrack));
        }

        private void ProcessPlaylistType()
        {
            var playlist = (Playlist)SelectedItem;
            var artists = supporterService.ArtistSup.Where((artist) => artist.Playlists.Contains(playlist))
                                                    .ToList();

            

            CurrentList.Clear();

            foreach (var artist in artists.ToList())
                CurrentList.Add(artist);


            playlist.Tracks.ToList().ForEach(track => CurrentList.Add(track));
        }

        private void ProcessTrackType()
        {
            var track = (Track)SelectedItem;
            CurrentList.Clear();
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
            DisplayListType();
            _mainWindowAPI.MainWindow.ResetVM(factoryService.FactoryContainerViewModel);
            vmHolder.AddViewModel(nameVM, this);
        }        

        private void Delete(object obj)
        {
            supporterService.RemoveInstanceObject(SelectedItem);
            SetListType(listType);
        }

        private void Back(object obj) 
        {
            if (_storage.Count != 0)
            {
                var previous = _storage.Pop();
                CurrentList.Clear();
                previous.ToList().ForEach(prev => CurrentList.Add(prev));
            }
        }

        private void ItemSelect(object obj)
        {
            if (SelectedItem != null)
            {
                var current = CurrentList;
                _storage.Push(current);
                ProcessItemSelection();
            }
        }

        
        private void DropStaff(object obj)
        {
            _playerInstance = playerService.GetPlayer();
            if (obj is Playlist playerPlaylist) 
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
    }
}