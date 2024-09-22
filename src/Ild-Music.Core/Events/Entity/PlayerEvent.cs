using Ild_Music.Core.Events.Signals;

namespace Ild_Music.Core.Events.Entity;

public struct PlayerEvent
{
    public Guid MetaId {get;}
    public PlayerSignal PlayerSignal {get;}

    public PlayerEvent(PlayerSignal playerSignal)
    {
        MetaId = Guid.NewGuid();
        PlayerSignal = playerSignal;
    }

    public PlayerEvent(PlayerSignal playerSignal, Guid metaId)
    {
        PlayerSignal = playerSignal;
        MetaId = metaId;
    }
}
