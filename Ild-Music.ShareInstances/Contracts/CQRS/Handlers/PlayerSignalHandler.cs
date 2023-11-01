using ShareInstances.CQRS.Notifications;
using ShareInstances.CQRS.Signals;
using ShareInstances.CQRS.Handlers.Delegatebag;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace ShareInstances.CQRS.Handlers;
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
