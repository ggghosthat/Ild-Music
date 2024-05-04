using Ild_Music.Core.Events.Signals;

using System;
using System.Collections.Generic;

namespace Ild_Music.CQRS;

internal static class DelegateSwitch
{
    //dictionary storage for delegates by cqrs signals
    private static IDictionary<PlayerSignal, Delegate> playerSwitch =
                    new Dictionary<PlayerSignal, Delegate>();

    private static IDictionary<CubeSignal, Delegate> cubeSwitch = 
                    new Dictionary<CubeSignal, Delegate>();

    
    //delegate registration methods
    public static void RegisterPlayerDelegate(PlayerSignal playerSignal,
                                              Delegate newDelegate) =>
        playerSwitch[playerSignal] = newDelegate;


    public static void RegisterCubeDelegate(CubeSignal cubeSignal,
                                            Delegate newDelegate) =>
        cubeSwitch[cubeSignal] = newDelegate;

    //delegate resolution methods
    public static Delegate ResolvePlayerDelegate(PlayerSignal playerSignal) =>
        (playerSwitch.ContainsKey(playerSignal))
            ?playerSwitch[playerSignal]:null;

    public static Delegate ResolveCubeDelegate(CubeSignal cubeSignal) =>
        (cubeSwitch.ContainsKey(cubeSignal))
            ?cubeSwitch[cubeSignal]:null;
}
