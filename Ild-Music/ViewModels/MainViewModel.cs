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
        private PlayerService player => (PlayerService)App.Stage.GetServiceInstance("PlayerService");
        #endregion

        #region Player Scope
        public IPlayer _player;
        private bool PlayerToggle => _player.PlayerState;
        public TimeSpan TotalTime => _player.TotalTime;
        public TimeSpan CurrentTime => _player.CurrentTime;
        #endregion

        #region Commands Scope
        public CommandDelegator PreviousCommand { get; }
        public CommandDelegator NextCommand { get; }
        public CommandDelegator KickCommand { get; }
        public CommandDelegator StopCommand { get; }
        public CommandDelegator TrackTimeChangedCommand { get; }

        public CommandDelegator VolumeSliderShowCommand {get;}
        #endregion

        #region Properties
        public BaseViewModel CurrentVM { get; set; }

        public Stack<BaseViewModel> WindowStack {get;} = new();
        #endregion



        #region Ctor
        public MainViewModel()
        {
            App.ViewModelTable.Add(StartViewModel.nameVM, new StartViewModel());
            App.ViewModelTable.Add(FactoryViewModel.nameVM, new FactoryViewModel());
            App.ViewModelTable.Add(ListViewModel.nameVM, new ListViewModel());
            App.ViewModelTable.Add(SettingViewModel.nameVM, new SettingViewModel());
            App.ViewModelTable.Add(ArtistViewModel.nameVM, new ArtistViewModel());
            App.ViewModelTable.Add(PlaylistViewModel.nameVM, new PlaylistViewModel());
            App.ViewModelTable.Add(TrackViewModel.nameVM, new TrackViewModel());
            App.ViewModelTable.Add(nameVM, this);

            PreviousCommand = new();
            NextCommand = new();
            KickCommand = new();
            StopCommand = new();
            TrackTimeChangedCommand = new();
            VolumeSliderShowCommand = new(VolumeSliderShow,null);

            CurrentVM = new SettingViewModel();
            _player = player.PlayerInstance;
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


        #region Command Methods
        private void VolumeSliderShow(object obj) => VolumeSliderOpen ^= true;
        #endregion
    }
}