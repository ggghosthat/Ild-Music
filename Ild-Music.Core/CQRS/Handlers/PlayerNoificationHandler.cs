using Ild_Music.Core.Events.Notifications;

using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Ild_Music.Core.Events.Handlers;

public class PlayerNotificationHandler<T> :
	INotificationHandler<T> where T : PlayerNotification
{
	public PlayerNotificationHandler()
	{}

	public async Task Handle(T playerNotification, CancellationToken cts)
	{
	}
}
