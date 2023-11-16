using Ild_Music.Core.CQRS.Notifications;
using Ild_Music.Core.CQRS.Handlers.Delegatebag;

using MediatR;
namespace Ild_Music.Core.CQRS.Handlers;
public class CubeNotificationHandler<T> : INotificationHandler<CubeNotification>
{  
    private readonly DelegateBag _delegateBag;

    public CubeNotificationHandler(DelegateBag delegateBag)
    {
        _delegateBag = delegateBag;
    }

    public Task Handle(CubeNotification cubeNotification, CancellationToken token)
    {
        Func<int> action;
        if(_delegateBag.TryGetAction(cubeNotification.CubeSignal, out action))
        {
            action?.Invoke();
        }

        //TODO:dont forget add logging here
        return Task.CompletedTask;
    }
}
