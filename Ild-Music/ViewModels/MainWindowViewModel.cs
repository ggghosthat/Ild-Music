using Ild_Music.Core.Contracts;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.CQRS;
using Ild_Music.Core.Events.Signals;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ild_Music.ViewModels;

public class MainWindowViewModel : Base.BaseViewModel
{
    #region VM id
    public static readonly string nameVM = "MainVM";
    public override string NameVM => nameVM;
    #endregion

    #region Services
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static PlayerGhost playerGhost => (PlayerGhost)App.Stage.GetGhost(Ghosts.PLAYER);
    #endregion

    #region Player Scope
    public static IPlayer? _player = null;
    public bool PlayerState => _player?.ToggleState ?? false;
    public bool PlayerEmpty => _player?.IsEmpty ?? true;

    public Track? CurrentTrack => _player?.CurrentTrack;
    public Playlist? CurrentPlaylist => _player?.CurrentPlaylist;

    //private TimeSpan totalTime = TimeSpan.FromSeconds(1);
    public double TotalTime => _player.TotalTime.TotalSeconds;
    public double StartTime => TimeSpan.Zero.TotalSeconds;

    public double CurrentTime 
    {
        get => (double)(_player.CurrentTime.TotalSeconds);
        set => _player.CurrentTime = TimeSpan.FromSeconds(value);
    }

    public TimeSpan CurrentTimeDisplay => TimeSpan.FromSeconds(CurrentTime);
    public TimeSpan TotalTimeDisplay => _player.TotalTime;

    public float MaxVolume => _player.MaxVolume;
    public float MinVolume => _player.MinVolume;

    public float CurrentVolume
    {
        get => _player.CurrentVolume;
        set => _player.CurrentVolume = value;
    }

    public string Title => CurrentTrack?.Name.ToString();
    #endregion

    #region Commands Scope
    public CommandDelegator NavBarResolve { get; private set; }
    public CommandDelegator PreviousCommand { get; private set; }
    public CommandDelegator NextCommand { get; private set; }
    public CommandDelegator KickCommand { get; private set; }
    public CommandDelegator StopCommand { get; private set; }
    public CommandDelegator RepeatCommand { get; private set; }
    public CommandDelegator VolumeSliderShowCommand { get; private set; }
    public CommandDelegator ExitCommand { get; private set; }
    public CommandDelegator SwitchHomeCommand { get; private set; }
    public CommandDelegator SwitchListCommand { get; private set; }
    public CommandDelegator SwitchBrowseCommand { get; private set; }
    #endregion

    #region Fields
    private DispatcherTimer timer;
    public bool VolumeSliderOpen {get; private set;} = false;
    #endregion

    #region Properties
    public BaseViewModel CurrentVM { get; set; }

    public Stack<BaseViewModel> WindowStack {get; private set;} = new();


    public ObservableCollection<string> NavItems {get;} = new() {"Home","Collections", "Browse"};
    public char? NavItem {get; set;}
    #endregion

    public MainWindowViewModel()
    {
        //1. resolve player instance
        PresetPlayer();
        //2. resolve commands
        PresetCommands();
        //3. allocate view-models
        PresetViewModels();
        //4 preset special details
        PresetGlobalTimer();
    }

    #region start-up methods
    private void PresetPlayer()
    {
        _player = playerGhost.GetPlayer();

        var entityUpdateDelegate = () =>{
            OnPropertyChanged("CurrentEntity");
            OnPropertyChanged("PlayerState");
            OnPropertyChanged("TotalTime");
            OnPropertyChanged("TotalTimeDisplay");
            OnPropertyChanged("Title");
        };

        DelegateSwitch.RegisterPlayerDelegate(PlayerSignal.PLAYER_SET_TRACK, entityUpdateDelegate);
        DelegateSwitch.RegisterPlayerDelegate(PlayerSignal.PLAYER_SET_PLAYLIST,entityUpdateDelegate);
    }
    
    private void PresetCommands()
    {
        NavBarResolve = new(NavResolve, OnNavSelected);
        KickCommand = new(KickPlayer, OnCanTogglePlayer);
        StopCommand = new(StopPlayer, OnCanTogglePlayer);
        PreviousCommand = new(PreviousSwipePlayer, OnCanSwipePlayer);
        NextCommand = new(NextSwipePlayer, OnCanSwipePlayer);
        RepeatCommand = new(RepeatPlayer, OnCanTogglePlayer);
        VolumeSliderShowCommand = new(VolumeSliderShow,null);
        ExitCommand = new(Exit, null);

        SwitchHomeCommand = new(SwitchHome, null);
        SwitchListCommand = new(SwitchList, null);
        SwitchBrowseCommand = new(SwitchBrowse, null);
    }

    private void PresetViewModels()
    {
        // App.ViewModelTable.Add(StartViewModel.nameVM, new StartViewModel());
        // App.ViewModelTable.Add(FactoryContainerViewModel.nameVM, new FactoryContainerViewModel());
        // App.ViewModelTable.Add(ListViewModel.nameVM, new ListViewModel());
        // App.ViewModelTable.Add(SettingViewModel.nameVM, new SettingViewModel());
        // App.ViewModelTable.Add(ArtistViewModel.nameVM, new ArtistViewModel());
        // App.ViewModelTable.Add(PlaylistViewModel.nameVM, new PlaylistViewModel());
        // App.ViewModelTable.Add(TrackViewModel.nameVM, new TrackViewModel());
        // App.ViewModelTable.Add(InstanceExplorerViewModel.nameVM, new InstanceExplorerViewModel());
        // App.ViewModelTable.Add(BrowseViewModel.nameVM, new BrowseViewModel());
        // App.ViewModelTable.Add(nameVM, this);

        // CurrentVM = (BaseViewModel)App.ViewModelTable[StartViewModel.nameVM];
    }

    private void PresetGlobalTimer()
    {
        timer = new(TimeSpan.FromMilliseconds(300), DispatcherPriority.Normal, UpdateCurrentTime);
        timer.Start();
    }
    #endregion

    #region Private Methods
    private void UpdateCurrentTime(object sender, EventArgs e)
    {
        OnPropertyChanged("PlayerState");
        if (PlayerState)
        {
            OnPropertyChanged("CurrentTime");
            OnPropertyChanged("CurrentTimeDisplay");
        }
    }
    #endregion

    #region MainViewModel API Methods
    public void DefineNewPresentItem(BaseViewModel newItem)
    {
        CurrentVM = newItem;
    }

    public void PushVM(BaseViewModel prev, BaseViewModel next)
    {
        WindowStack.Push(prev);
        WindowStack.Push(next);
    }

    public BaseViewModel PopVM()
    {
        return WindowStack.Pop();
    }

    public void ResolveWindowStack()
    {
        if (WindowStack.Count > 0)
            CurrentVM = PopVM();

        // if (CurrentVM is ListViewModel listVM)
        //     listVM.UpdateProviders();
    }

    public async Task ResolveArtistInstance(
        BaseViewModel source,
        Artist artist)
    {
        // var artistVM = (ArtistViewModel)App.ViewModelTable[ArtistViewModel.nameVM];
        // artistVM?.SetInstance(artist);

        // if (artistVM != null)
        // {
        //     PushVM(source, artistVM);
        //     ResolveWindowStack();
        // }
    }   

    public async Task ResolvePlaylistInstance(
        BaseViewModel source,
        Playlist playlist)
    {
        // var playlistVM = (PlaylistViewModel)App.ViewModelTable[PlaylistViewModel.nameVM];
        // playlistVM?.SetInstance(playlist);   

        // if (playlistVM != null)
        // {
        //     PushVM(source, playlistVM);
        //     ResolveWindowStack();
        // }
    }
    
    public void DropPlaylistInstance(
        BaseViewModel source, 
        Playlist playlist,
        bool isResolved = false, 
        int playlistIndex = 0)
    {
        // _player?.Stop();
        // _player?.DropPlaylist(playlist);

        // OnPropertyChanged("CurrentTrack");
        // OnPropertyChanged("CurrentPlaylist");
        // OnPropertyChanged("TotalTime");
        // OnPropertyChanged("TotalTimeDisplay");
        // OnPropertyChanged("CurrentTime");
            
        // _player?.Toggle();

        // if (isResolved)
        // {
        //     var playlistVM = (PlaylistViewModel)App.ViewModelTable[PlaylistViewModel.nameVM];
        //     playlistVM?.SetInstance(playlist); 
            
        //     PushVM(source, playlistVM);
        //     ResolveWindowStack();
        // }
    }

    public async Task ResolveTrackInstance(
        BaseViewModel source,
        CommonInstanceDTO track)
    {
        // if(track.Tag != EntityTag.TRACK)
        //     return;

        // var trackVM = (TrackViewModel)App.ViewModelTable[TrackViewModel.nameVM];
        // trackVM?.SetInstance(track);


        // if (trackVM != null)
        // {
        //     PushVM(source, trackVM);
        //     ResolveWindowStack();
        // }
    }

    public void DropTrackInstance(
        BaseViewModel source, 
        Track track,
        bool isResolved = false)
    {
        
        // _player?.Stop();
        // _player?.DropTrack(track);

        // OnPropertyChanged("CurrentTrack");
        // OnPropertyChanged("TotalTime");
        // OnPropertyChanged("TotalTimeDisplay");
        // OnPropertyChanged("CurrentTime");

        // _player?.Toggle();
 
        // if (isResolved)
        // {
        //     var trackVM = (TrackViewModel)App.ViewModelTable[TrackViewModel.nameVM];
        //     trackVM?.SetInstance(track);

        //     PushVM(source, trackVM);
        //     ResolveWindowStack();
        // }
    } 

    //Don't ready to use
    public void HitTemps(IEnumerable<Track> musicFiles)
    {
        _player?.Stop();

        OnPropertyChanged("CurrentTrack");
        OnPropertyChanged("TotalTime");
        OnPropertyChanged("TotalTimeDisplay");
        OnPropertyChanged("CurrentTime");
            
        _player?.Toggle();
    }
    #endregion

    #region Predicates
    private bool OnNavSelected(object obj)
    {
        return (NavItem is not null);
    }

    private bool OnCanTogglePlayer(object obj) 
    {
        return _player.IsEmpty == false;
    }

    private bool OnCanSwipePlayer(object obj)
    {
        return (_player.IsEmpty == false) && (_player.IsSwipe == true);
    }
    #endregion

    #region Command Methods
    private void NavResolve(object obj)
    {
        switch(NavItem)
        {
            // case 'a':
            //     DefineNewPresentItem((BaseViewModel)App.ViewModelTable[StartViewModel.nameVM]);
            //     break;
            // case 'b':
            //     DefineNewPresentItem((BaseViewModel)App.ViewModelTable[ListViewModel.nameVM]);
            //     break;
            // case 'c':
            //     DefineNewPresentItem((BaseViewModel)App.ViewModelTable[SettingViewModel.nameVM]);
            //     break;
            // case 'd':
            //     DefineNewPresentItem((BaseViewModel)App.ViewModelTable[BrowseViewModel.nameVM]);
            //     break;
            default:
                break;
        }
    }

    private void KickPlayer(object obj) 
    {
        _player?.Toggle();
        OnPropertyChanged("PlayerState");   
    }

    private void StopPlayer(object obj) 
    {
        _player?.Stop();
    }

    private void PreviousSwipePlayer(object obj) 
    {
        OnPropertyChanged("CurrentEntity");
        _player?.SkipPrev();
    }

    private void NextSwipePlayer(object obj) 
    {
        OnPropertyChanged("CurrentEntity");
        _player?.SkipNext();
    }

    private void RepeatPlayer(object obj)
    {
        _player?.Repeat();
    }

    private void ShuffleCollectionPlayer(object obj) 
    {
        _player?.Shuffle();
    }

    private void VolumeSliderShow(object obj)
    {
        VolumeSliderOpen ^= true;
    }
    #endregion
    
    #region User interface windows switch methods
    public void SwitchHome(object obj)
    {
        // DefineNewPresentItem((BaseViewModel)App.ViewModelTable[StartViewModel.nameVM]);
    }

    public void SwitchList(object obj)
    {
        // DefineNewPresentItem((BaseViewModel)App.ViewModelTable[ListViewModel.nameVM]);
    }

    public void SwitchBrowse(object obj)
    {
        // DefineNewPresentItem((BaseViewModel)App.ViewModelTable[BrowseViewModel.nameVM]);
    }

    public void Exit(object obj)
    {
        if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.Shutdown();
    }
    #endregion
}
