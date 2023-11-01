using ShareInstances.CQRS.Signals;

using System;
using MediatR;
namespace ShareInstances.CQRS.Notifications;
public class PlayerNotification : INotification
{
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

    public PlayerSignal PlayerSignal {get;}
    public Guid MetaId {get;}
}
