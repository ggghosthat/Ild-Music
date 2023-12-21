using Ild_Music.VlcPlayer.CQRS.Signals;

using System;
using MediatR;
namespace Ild_Music.VlcPlayer.CQRS.Notifications;
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
