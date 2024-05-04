using Ild_Music.Core.Contracts;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
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
    public bool PlayerState => _player.ToggleState;
    public bool PlayerEmpty => _player.IsEmpty;

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


    public ObservableCollection<char> NavItems {get;} = new() {'a','b','d'};
    public char? NavItem {get; set;}
    #endregion

    public MainWindowViewModel()
    {
        
    }
}
