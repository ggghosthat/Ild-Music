using ShareInstances.Contracts;
using ShareInstances.Contracts.Services;
using ShareInstances.Contracts.Services.Interfaces;

using System;
namespace ShareInstances.Services.Entities;
public sealed class PlayerGhost : IGhost
{
	public ReadOnlyMemory<char> GhostName {get; init;} = "PlayerGhost".AsMemory(); 

	public static IPlayer PlayerInstance {get; private set;}


    public PlayerGhost(IPluginBag pluginBag)
    {
        PlayerInstance = pluginBag.GetCurrentPlayer();
    }

	public void Init(ref IPlayer player)
    {
		PlayerInstance = player;    	
    }

    public void SwitchPlayer(ref IPlayer player)
    {
        PlayerInstance = player;
    }
}
