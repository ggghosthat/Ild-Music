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
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace Ild_Music.ViewModels;

public class MainWindowViewModel : Base.BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    private DispatcherTimer timer;

    public MainWindowViewModel()
    {
        PresetPlayer();
        PresetCommands();
        PresetViewModel();
        PresetGlobalTimer();
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static PlayerGhost playerGhost => (PlayerGhost)App.Stage.GetGhost(Ghosts.PLAYER);

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
    
    public BaseViewModel CurrentVM { get; set; }
    public bool VolumeSliderOpen { get; private set; } = false;

    public Stack<Guid> WindowStack { get; private set; } = new();
    public ObservableCollection<string> NavItems => new() {"Home","Collections", "Browse"};
    public char? NavItem { get; set; }

    public static IPlayer? _player = null;
    public bool PlayerState => _player?.ToggleState ?? false;
    public bool PlayerEmpty => _player?.IsEmpty ?? true;

    public Track? CurrentTrack => _player?.CurrentTrack;
    public string Title => CurrentTrack?.Name.ToString();
    public Playlist? CurrentPlaylist => _player?.CurrentPlaylist;

    //private TimeSpan totalTime = TimeSpan.FromSeconds(1);
    public double TotalTime => _player?.TotalTime.TotalSeconds ?? 1d;
    public double StartTime => TimeSpan.Zero.TotalSeconds;
   
    public TimeSpan CurrentTimeDisplay => TimeSpan.FromSeconds(CurrentTime);
    public TimeSpan TotalTimeDisplay => _player?.TotalTime ?? TimeSpan.Zero;

    public float MaxVolume => _player.MaxVolume;
    public float MinVolume => _player.MinVolume;

    public double CurrentTime 
    {
        get => (double)(_player?.CurrentTime.TotalSeconds ?? 0);
        set
        {
            if (_player is not null)
                _player.CurrentTime = TimeSpan.FromSeconds(value);
        }
    }

    public float CurrentVolume
    {
        get => _player?.CurrentVolume ?? 0;
        set
        {
            if (_player is not null)
                _player.CurrentVolume = value;
        }
    }

    private void PresetPlayer()
    {
        _player = playerGhost?.GetPlayer();

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

    private void PresetViewModel()
    {   
        App.ViewModelTable.Add(MainWindowViewModel.viewModelId, this);
        CurrentVM = (BaseViewModel)App.ViewModelTable[ListViewModel.viewModelId];
    }

    private void PresetGlobalTimer()
    {
        timer = new(TimeSpan.FromMilliseconds(300), DispatcherPriority.Normal, UpdateCurrentTime);
        timer.Start();
    }

    private void UpdateCurrentTime(object sender, EventArgs e)
    {
        OnPropertyChanged("PlayerState");
        if (PlayerState)
        {
            OnPropertyChanged("CurrentTime");
            OnPropertyChanged("CurrentTimeDisplay");
        }
    }

    public void DefineNewPresentItem(Guid viewModelId)
    {
        CurrentVM = (BaseViewModel)App.ViewModelTable[viewModelId];
    }

    public void PushVM(BaseViewModel prev, BaseViewModel next)
    {
        WindowStack.Push(prev.ViewModelId);
        WindowStack.Push(next.ViewModelId);
    }

    public BaseViewModel PopVM()
    {
        Guid viewModelId = WindowStack.Pop();
        return (BaseViewModel)App.ViewModelTable[viewModelId];
    }

    public void ResolveWindowStack()
    {
        if (WindowStack.Count > 0)
            CurrentVM = PopVM();

        if (CurrentVM is ListViewModel listVM)
            listVM.UpdateProviders();
    }

    public async Task ResolveInstance(
        BaseViewModel source,
        CommonInstanceDTO instanceDto)
    {
        var viewModel = instanceDto.Tag switch 
        {
           EntityTag.ARTIST  => (BaseViewModel)App.ViewModelTable[ArtistViewModel.viewModelId],
           EntityTag.PLAYLIST => (BaseViewModel)App.ViewModelTable[PlaylistViewModel.viewModelId],
           EntityTag.TRACK => (BaseViewModel)App.ViewModelTable[TrackViewModel.viewModelId],
           EntityTag.TAG => null,
           _ => null
        };

        SetInstanceToViewModel(viewModel, instanceDto);
        PushVM(source, viewModel);
        ResolveWindowStack();
    }   

    private void SetInstanceToViewModel(
        BaseViewModel viewModel,
        CommonInstanceDTO instanceDTO)
    {
        if (viewModel is ArtistViewModel artistVM)
        {
            var artist = supporter.GetArtistAsync(instanceDTO).Result;
            artistVM.SetInstance(artist);
        }
        else if (viewModel is PlaylistViewModel playlistVM)
        {
            var playlist = supporter.GetPlaylistAsync(instanceDTO).Result;
            playlistVM.SetInstance(playlist);
        }
        else if (viewModel is TrackViewModel trackVM)
        {
            var track = supporter.GetTrackAsync(instanceDTO).Result;
            trackVM.SetInstance(track);
        }
    }

    public void DropPlaylistInstance(
        BaseViewModel source, 
        Playlist playlist,
        bool isResolved = true, 
        int playlistIndex = 0)
    {
        _player?.Stop();
        _player?.DropPlaylist(playlist);

        OnPropertyChanged("CurrentTrack");
        OnPropertyChanged("CurrentPlaylist");
        OnPropertyChanged("TotalTime");
        OnPropertyChanged("TotalTimeDisplay");
        OnPropertyChanged("CurrentTime");
            
        _player?.Toggle();

        if (!isResolved)
        {
            var playlistVM = (PlaylistViewModel)App.ViewModelTable[PlaylistViewModel.viewModelId];
            playlistVM?.SetInstance(playlist); 
           
            PushVM(source, playlistVM);
            ResolveWindowStack();
        }
    }

    public void DropTrackInstance(
        BaseViewModel source, 
        Track track,
        bool isResolved = true)
    {
        
        _player?.Stop();
        _player?.DropTrack(track);

        OnPropertyChanged("CurrentTrack");
        OnPropertyChanged("TotalTime");
        OnPropertyChanged("TotalTimeDisplay");
        OnPropertyChanged("CurrentTime");

        _player?.Toggle();
 
        if (!isResolved)
        {
            var trackVM = (TrackViewModel)App.ViewModelTable[TrackViewModel.viewModelId];
            trackVM?.SetInstance(track);

            PushVM(source, trackVM);
            ResolveWindowStack();
        }
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

    private void NavResolve(object obj)
    {
        switch(NavItem)
        {
            case 'a':
                DefineNewPresentItem(StartViewModel.viewModelId);
                break;
            case 'b':
                DefineNewPresentItem(ListViewModel.viewModelId);
                break;
            case 'c':
                //DefineNewPresentItem((BaseViewModel)App.ViewModelTable[SettingViewModel.viewModelId]);
                break;
            case 'd':
                //DefineNewPresentItem((BaseViewModel)App.ViewModelTable[BrowseViewModel.viewModelId]);
                break;
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
    
    public void SwitchHome(object obj)
    {
        DefineNewPresentItem(StartViewModel.viewModelId);
    }

    public void SwitchList(object obj)
    {
        DefineNewPresentItem(ListViewModel.viewModelId);
    }

    public void SwitchBrowse(object obj)
    {
        //DefineNewPresentItem(BrowseViewModel.viewModelId);
    }

    public void Exit(object obj)
    {
        if(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.Shutdown();
    }
}
