using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using ShareInstances;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;


using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels
{
    public class MainViewModel : Base.BaseViewModel
    {
        public string HugeName {get; set;}        
        #region Fields
        public bool VolumeSliderOpen {get; private set;} = false;
        #endregion

        #region VM id
        public static readonly string nameVM = "MainVM";
        public override string NameVM => nameVM;
        #endregion
        
        #region Services
        private static SupporterService supporter => (SupporterService)App.Stage.GetServiceInstance("SupporterService");
        private static PlayerService player => (PlayerService)App.Stage.GetServiceInstance("PlayerService");
        #endregion

        #region Player Scope
        public IPlayer _player;
        private bool PlayerToggle => _player.PlayerState;
        public TimeSpan TotalTime => _player.TotalTime;
        public TimeSpan CurrentTime => _player.CurrentTime;
        // public TimeSpan CurrentTime => TimeSpan.Zero;
        #endregion

        #region Commands Scope
        public CommandDelegator PreviousCommand { get; }
        public CommandDelegator NextCommand { get; }
        public CommandDelegator KickCommand { get; }
        public CommandDelegator StopCommand { get; }
        public CommandDelegator TimeChangedCommand { get; }

        public CommandDelegator VolumeSliderShowCommand {get;}
        #endregion

        #region Properties
        public BaseViewModel CurrentVM { get; set; }

        public Stack<BaseViewModel> WindowStack {get; private set;} = new();
        #endregion



        #region Ctor
        public MainViewModel()
        {
            _player = player.PlayerInstance;

            App.ViewModelTable.Add(StartViewModel.nameVM, new StartViewModel());
            App.ViewModelTable.Add(FactoryViewModel.nameVM, new FactoryViewModel());
            App.ViewModelTable.Add(ListViewModel.nameVM, new ListViewModel());
            App.ViewModelTable.Add(SettingViewModel.nameVM, new SettingViewModel());
            App.ViewModelTable.Add(ArtistViewModel.nameVM, new ArtistViewModel());
            App.ViewModelTable.Add(PlaylistViewModel.nameVM, new PlaylistViewModel());
            App.ViewModelTable.Add(TrackViewModel.nameVM, new TrackViewModel());
            App.ViewModelTable.Add(nameVM, this);

            KickCommand = new(KickPlayer, OnCanTogglePlayer);
            StopCommand = new(StopPlayer, OnCanTogglePlayer);
            PreviousCommand = new(PreviousSwipePlayer, OnCanSwipePlayer);
            NextCommand = new(NextSwipePlayer, OnCanSwipePlayer);
            TimeChangedCommand = new(TimeChangedPlayer, OnCanTogglePlayer);
            VolumeSliderShowCommand = new(VolumeSliderShow,null);

            CurrentVM = new SettingViewModel();
        }

        #endregion
        
        #region External API
        public void DefineNewPresentItem(BaseViewModel newItem) =>
            CurrentVM = newItem;

        public void PushVM(BaseViewModel prev, BaseViewModel next)
        {
            WindowStack.Push(prev);
            WindowStack.Push(next);
        }

        public BaseViewModel PopVM() =>
            WindowStack.Pop();

        public void ResolveWindowStack()
        {
            // if (WindowStack.Count > 0)
            CurrentVM = WindowStack.Pop();

            if (CurrentVM is ListViewModel listVM)
                listVM.UpdateProviders();
        }

        public void ResolveInstance(BaseViewModel source, ICoreEntity instance)
        {
            BaseViewModel instanceVM = null;
            if (instance is Artist artist)
            {
                instanceVM = (ArtistViewModel)App.ViewModelTable[ArtistViewModel.nameVM];
                ((ArtistViewModel)instanceVM).SetInstance(artist);
            }
            else if (instance is Playlist playlist) 
            {
                instanceVM = (PlaylistViewModel)App.ViewModelTable[PlaylistViewModel.nameVM];
                ((PlaylistViewModel)instanceVM).SetInstance(playlist);   
            }
            else if (instance is Track track)
            {
                instanceVM = (TrackViewModel)App.ViewModelTable[TrackViewModel.nameVM];
                ((TrackViewModel)instanceVM).SetInstance(track);
            }

            if (instanceVM != null)
            {
                PushVM(source, instanceVM);
                ResolveWindowStack();
            }
        }
        #endregion

        #region Predicates
        private bool OnCanTogglePlayer(object obj) => _player.IsEmpty == false;
        private bool OnCanSwipePlayer(object obj) => (_player.IsEmpty == false) && (_player.IsSwipe == true);
        #endregion

        #region Command Methods
        private void KickPlayer(object obj) =>
            _player.Pause_ResumePlayer();
        

        private void StopPlayer(object obj) =>
            _player.StopPlayer();

        private void PreviousSwipePlayer(object obj) =>
            _player.DropPrevious();

        private void NextSwipePlayer(object obj) =>
            _player.DropNext();

        private void TimeChangedPlayer(object obj)
        {
            if (obj is TimeSpan newTimeSpan)
                _player.CurrentTime = newTimeSpan;
        }

        private void ShuffleCollectionPlayer(object obj) =>
            _player.ShuffleTrackCollection();

        private void ChangeVolumePlayer(object obj)
        {
            if (obj is float volume)
                _player.ChangeVolume(volume);
        }

        private void VolumeSliderShow(object obj) =>
            VolumeSliderOpen ^= true;
        #endregion
    }
}