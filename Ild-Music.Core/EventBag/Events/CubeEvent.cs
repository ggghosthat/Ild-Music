using Ild_Music.Core.EventBag.Signals;

namespace Ild_Music.Core.EventBag.Events;

public struct CubeEvent
{
    public CubeSignal CubeSignal {get;}
    public Guid MetaId {get;}

    public CubeEvent(CubeSignal cubeSignal)
    {
        CubeSignal = cubeSignal;
        MetaId = Guid.Empty;
    }

    public CubeEvent(CubeSignal cubeSignal, Guid metaId)
    {
        CubeSignal = cubeSignal;
        MetaId = metaId;
    }    
}
