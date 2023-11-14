using Ild_Music.Core.CQRS.Notifications;
using Ild_Music.Core.CQRS.Handlers.Delegatebag;

using MediatR;
namespace Ild_Music.Core.CQRS.Handlers;
public class PlayerNotificationHandler<T> : INotificationHandler<PlayerNotification>
{  
    private readonly DelegateBag _delegateBag;

    public PlayerNotificationHandler(DelegateBag delegateBag)
    {
        _delegateBag = delegateBag;
    }

    public Task Handle(PlayerNotification playerNotification, CancellationToken token)
    {
        Func<int> action;
        if(_delegateBag.TryGetAction(playerNotification.PlayerSignal, out action))
        {
            action?.Invoke();
        }

        //TODO:dont forget add logging here
        return Task.CompletedTask;
    }
}
