using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services;
using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.Core.Services.Entities;
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

    public IPlayer? GetPlayer()
    {
        return PlayerInstance;
    }
    
    public void SwitchPlayer(ref IPlayer player)
    {
        PlayerInstance = player;
    }

}
