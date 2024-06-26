using Ild_Music.Core.Events.Signals;

using MediatR;

namespace Ild_Music.Core.Events.Notifications;

public class PlayerNotification : INotification
{
    public Guid MetaId {get;}
    public PlayerSignal PlayerSignal {get;}

    public PlayerNotification(PlayerSignal playerSignal)
    {
        MetaId = Guid.NewGuid();
        PlayerSignal = playerSignal;
    }

    public PlayerNotification(PlayerSignal playerSignal, Guid metaId)
    {
        PlayerSignal = playerSignal;
        MetaId = metaId;
    }
}
