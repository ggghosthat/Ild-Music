using Ild_Music.Core.Events.Signals;

using MediatR;

namespace Ild_Music.Core.Events.Notifications;

public class CubeNotification : INotification
{
    public CubeSignal CubeSignal {get;}
    public Guid MetaId {get;}

    public CubeNotification(CubeSignal cubeSignal)
    {
        CubeSignal = cubeSignal;
        MetaId = Guid.Empty;
    }

    public CubeNotification(CubeSignal cubeSignal, Guid metaId)
    {
        CubeSignal = cubeSignal;
        MetaId = metaId;
    }    
}
