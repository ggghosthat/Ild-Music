using Ild_Music.Core.Events;

namespace Ild_Music.Core.Contracts;

public interface IShare
{
    //add pub-sub supports for all components
    public void InjectEventBag(IEventBag eventBag);
}