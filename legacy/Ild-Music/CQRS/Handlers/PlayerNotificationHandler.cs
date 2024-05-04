using Ild_Music.Core.Events.Notifications;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Ild_Music.CQRS.Handlers;
public class PlayerNotificationHandler<T> : INotificationHandler<PlayerNotification>
{  

    public PlayerNotificationHandler()
    {}

    public Task Handle(PlayerNotification playerNotification, CancellationToken token)
    {
        DelegateSwitch
            .ResolvePlayerDelegate(playerNotification.PlayerSignal)?
            .DynamicInvoke();

        //TODO:dont forget add logging here
        return Task.CompletedTask;
    }
}
