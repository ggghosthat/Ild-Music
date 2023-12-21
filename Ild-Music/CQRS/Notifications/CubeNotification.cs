using Ild_Music.Core.CQRS.Signals;

using MediatR;
namespace Ild_Music.Core.CQRS.Notifications;
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
