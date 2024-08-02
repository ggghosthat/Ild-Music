using Ild_Music.Core.Contracts;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Signals;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia;
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
    
    private static SupportGhost _supporterGhost => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static PlayerGhost _playerGhost => (PlayerGhost)App.Stage.GetGhost(Ghosts.PLAYER);
    private static IEventBag _eventBag => (EventBag)App.Stage.GetEventBag();

    public CommandDelegator NavBarResolve { get; private set; }
    public CommandDelegator PreviousCommand { get; private set; }
    public CommandDelegator NextCommand { get; private set; }
    public CommandDelegator KickCommand { get; private set; }
    public CommandDelegator StopCommand { get; private set; }
    public CommandDelegator RepeatCommand { get; private set; }
    public CommandDelegator VolumeSliderShowCommand { get; private set; }
    public CommandDelegator SearchAreaShowCommand { get; private set; }
    public CommandDelegator SearchAreaToggleCommand { get; private set; }
    public CommandDelegator SearchAreaHideCommand { get; private set; }
    public CommandDelegator SearchCommand { get; private set; }
    public CommandDelegator SelectSearchItemCommand { get; private set; }
    public CommandDelegator ExitCommand { get; private set; }
    public CommandDelegator SwitchHomeCommand { get; private set; }
    public CommandDelegator SwitchListCommand { get; private set; }
    public CommandDelegator SwitchBrowseCommand { get; private set; }
    
    public BaseViewModel CurrentVM { get; set; }
    public bool VolumeSliderOpen { get; private set; } = false;
    public bool SearchAreaOpen { get; private set; } = false;

    public Stack<Guid> WindowStack { get; private set; } = new();
    public ObservableCollection<string> NavItems => new() {"Home","Collections", "Browse"};
    public string? NavItem { get; set; }

    public ObservableCollection<CommonInstanceDTO?> SearchItems { get; set; } = new();
    public CommonInstanceDTO? SearchItem { get; set; }
    public string SearchPhrase { get; set; }
    
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
        _player = _playerGhost?.GetPlayer();

        var entityUpdateDelegate = () =>{
            OnPropertyChanged("CurrentTrack");
            OnPropertyChanged("CurrentPlaylist");
            OnPropertyChanged("PlayerState");
            OnPropertyChanged("TotalTime");
            OnPropertyChanged("TotalTimeDisplay");
            OnPropertyChanged("Title");
            Console.WriteLine(Title);
        };

        _eventBag.RegisterEvent((int)PlayerSignal.PLAYER_SET_TRACK, entityUpdateDelegate);
        _eventBag.RegisterEvent((int)PlayerSignal.PLAYER_SET_PLAYLIST, entityUpdateDelegate);
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
        SearchAreaShowCommand = new(SearchAreaShow, null);
        SearchAreaToggleCommand = new(SearchAreaToggle, null);
        SearchAreaHideCommand = new(SearchAreaHide, null);
        SearchCommand = new(Search, null);
        SelectSearchItemCommand = new(SelectSearchItem, null);
        ExitCommand = new(Exit, null);

        SwitchHomeCommand = new(SwitchHome, null);
        SwitchListCommand = new(SwitchList, null);
        SwitchBrowseCommand = new(SwitchBrowse, null);
    }

    private void PresetViewModel()
    {   
        App.ViewModelTable.Add(MainWindowViewModel.viewModelId, this);
        CurrentVM = (BaseViewModel)App.ViewModelTable[BrowserViewModel.viewModelId];
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

    public void PushVM(BaseViewModel prev, BaseViewModel next)
    {
        WindowStack.Push(prev.ViewModelId);
        WindowStack.Push(next.ViewModelId);
    }
     
    public void DefineNewPresentItem(Guid viewModelId)
    {
        var viewModel = (BaseViewModel)App.ViewModelTable[viewModelId];
        viewModel.Load();
        CurrentVM = viewModel;
    }

    public void ResolveWindowStack()
    {
        if (WindowStack.Count == 0)
            return;

        var viewModelId = WindowStack.Pop();
        DefineNewPresentItem(viewModelId); 
    }

    public void ResolveInstance(
        BaseViewModel source,
        CommonInstanceDTO instanceDto)
    {
        BaseViewModel viewModel = null;

        switch (instanceDto.Tag)
        {
            case (EntityTag.ARTIST):
                var artistViewModel = (ArtistViewModel)App.ViewModelTable[ArtistViewModel.viewModelId];
                artistViewModel?.SetInstance(instanceDto);
                viewModel = artistViewModel;
                break;
            case (EntityTag.PLAYLIST):
                var playlistViewModel = (PlaylistViewModel)App.ViewModelTable[PlaylistViewModel.viewModelId];
                playlistViewModel?.SetInstance(instanceDto);
                viewModel = playlistViewModel;
                break;
            case (EntityTag.TRACK):
                var trackViewModel = (TrackViewModel)App.ViewModelTable[TrackViewModel.viewModelId];
                trackViewModel?.SetInstance(instanceDto);
                viewModel = trackViewModel;
                break;            
            case (EntityTag.TAG):
                var tagViewModel = (TagEditorViewModel)App.ViewModelTable[TagEditorViewModel.viewModelId];
                var tag = _supporterGhost.GetTagAsync(instanceDto.Id).Result;
                tagViewModel?.DropInstance(tag).Wait();
                viewModel = tagViewModel;
                break;
        }

        if (viewModel is null)
            return;

        PushVM(source, viewModel);
        ResolveWindowStack();
    }   


    public void DropPlaylistInstance(
        BaseViewModel source, 
        Playlist playlist,
        bool isResolved = true, 
        int playlistIndex = 0)
    {
        _player?.Stop();
        _player?.DropPlaylist(playlist);

        OnPropertyChanged("CurrentPlaylist");
        OnPropertyChanged("CurrentTrack");
        OnPropertyChanged("Title");
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
        OnPropertyChanged("Title");
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

    public void SearchItemUp()
    {
        if (SearchItem is CommonInstanceDTO dto)
        {
            int count = SearchItems.Count - 1;
            int index = SearchItems.IndexOf(dto);

            if (index == 0)
                SearchItem = SearchItems[count];
            else
                SearchItem = SearchItems[index - 1];
        }
        else
        {
            SearchItem = SearchItems[SearchItems.Count - 1];
        }
    }

    public void SearchItemDown()
    {
        if (SearchItem is CommonInstanceDTO dto)
        {
            int count = SearchItems.Count - 1;
            int index = SearchItems.IndexOf(dto);
            
            if (index == count)
                SearchItem = SearchItems[0];
            else
                SearchItem = SearchItems[index + 1];
        }
        else
        {
            SearchItem = SearchItems[0];
        }
    }

    private bool OnNavSelected(object obj)
    {
        return (NavItem is not null);
    }

    private bool OnCanTogglePlayer(object obj) 
    {
        return _player?.IsEmpty == false;
    }

    private bool OnCanSwipePlayer(object obj)
    {
        return (_player?.IsEmpty == false) && (_player.IsSwipe == true);
    }

    private void NavResolve(object obj)
    {
        var navItemName = NavItem switch
        {
            "Home" => StartViewModel.viewModelId,
            "Collections" => ListViewModel.viewModelId,
            "Browse" => BrowserViewModel.viewModelId,
            _ => Guid.Empty
        };

        if (navItemName == Guid.Empty)
            return;

        DefineNewPresentItem(navItemName);
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
    
    private void SearchAreaShow(object obj)
    {
        SearchAreaOpen = true;
    }

    private void SearchAreaToggle(object obj)
    {
        SearchAreaOpen ^= true;
    }

    private void SearchAreaHide(object obj)
    {
        SearchPhrase = String.Empty;
        SearchAreaOpen = false;
    }

    private void SelectSearchItem(object obj)
    {
        if (SearchItem is CommonInstanceDTO searchItem)
            ResolveInstance(CurrentVM, searchItem);
    }

    private void Search(object obj)
    {
        if (String.IsNullOrEmpty(SearchPhrase))
            return;
     
        _supporterGhost?.Search(SearchPhrase).Result
            .ToList().ForEach(i => SearchItems.Add(i)); 
        
        SearchAreaOpen = true;
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
