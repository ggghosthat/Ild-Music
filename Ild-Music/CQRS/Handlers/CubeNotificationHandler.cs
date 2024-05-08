using Ild_Music.Core.Events.Notifications;

using System.Threading;
using System.Threading.Tasks;
using MediatR;
namespace Ild_Music.CQRS.Handlers;
public class CubeNotificationHandler<T> : INotificationHandler<CubeNotification>
{  

    public CubeNotificationHandler()
    {}

    public Task Handle(CubeNotification cubeNotification, CancellationToken token)
    {
        DelegateSwitch
            .ResolveCubeDelegate(cubeNotification.CubeSignal)?
            .DynamicInvoke();

        //TODO:dont forget add logging here
        return Task.CompletedTask;
    }
}
