using Ild_Music.Core.Events.Signals;

using MediatR;

namespace Ild_Music.Core.Events.Notifications;

public class PlayerNotification : INotification
{
    public PlayerSignal PlayerSignal {get;}
    public Guid MetaId {get;}

    public PlayerNotification(PlayerSignal playerSignal)
    {
        PlayerSignal = playerSignal;
        MetaId = Guid.Empty;
    }

    public PlayerNotification(PlayerSignal playerSignal, Guid metaId)
    {
        PlayerSignal = playerSignal;
        MetaId = metaId;
    }
}
