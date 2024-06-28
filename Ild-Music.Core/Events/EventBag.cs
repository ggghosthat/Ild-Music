namespace Ild_Music.Core.Events;

public class EventBag : IEventBag
{
    private static Dictionary<int, Delegate> _store = [];

    public Delegate? GetAction(int eventId)
    {
        if(_store.ContainsKey(eventId))
            return null;

        return _store[eventId];
    }
  
    public void RegisterEvent(int eventId, Delegate action)
    {
        if (_store.ContainsKey(eventId))
            return;

        _store[eventId] = action;
    }

    public void ReleaseEvent(int eventId)
    {
        if(_store.ContainsKey(eventId))
            _store.Remove(eventId);
    }

    public void UpdateEvent(int eventId, Delegate action)
    {
        if(_store.ContainsKey(eventId))
            _store[eventId] = action;
    }
   
    public async Task<Delegate?> GetActionAsync(int eventId)
    {
        if(_store.ContainsKey(eventId))
            return null;

        return _store[eventId];
    }

    public async Task RegisterEventAsync(int eventId, Delegate action)
    {
        if (_store.ContainsKey(eventId))
            return;

        _store[eventId] = action;
    }

    public async Task ReleaseEventAsync(int eventId)
    {
        if(_store.ContainsKey(eventId))
            _store.Remove(eventId);
    }

    public async Task UpdateEventAsync(int eventId, Delegate action)
    {
        if(_store.ContainsKey(eventId))
            _store[eventId] = action;
    }
}