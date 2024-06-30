namespace Ild_Music.Core.Events.Signals;

public enum PlayerSignal : int
{
    PLAYER_ON = 0,
    PLAYER_OFF = 1,
    PLAYER_PAUSE = 2,
    PLAYER_SET_TRACK = 3,
    PLAYER_SET_PLAYLIST = 4,
    PLAYER_REPEAT = 5,
    PLAYER_SHIFT_LEFT = 6,
    PLAYER_SHIFT_RIGHT = 7
}
