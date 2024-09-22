namespace Ild_Music.Core.Events;

public interface IEventBag
{
    public Delegate? GetAction(int eventId);
    public Task<Delegate?> GetActionAsync(int eventId);
    
    public void RegisterEvent(int eventId, Delegate action);
    public void ReleaseEvent(int eventId);
    public void UpdateEvent(int eventId, Delegate action);

    public Task RegisterEventAsync(int eventId, Delegate action);
    public Task ReleaseEventAsync(int eventId);
    public Task UpdateEventAsync(int eventId, Delegate action);
}