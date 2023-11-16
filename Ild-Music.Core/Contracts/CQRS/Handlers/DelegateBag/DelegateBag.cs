using Ild_Music.Core.CQRS.Signals;

namespace Ild_Music.Core.CQRS.Handlers.Delegatebag;
public sealed class DelegateBag
{
    private static IDictionary<PlayerSignal, Func<int>> _playerBag = new Dictionary<PlayerSignal, Func<int>>();

    private static IDictionary<CubeSignal, Func<int>> _cubeBag = new Dictionary<CubeSignal, Func<int>>();


    public void Allocate(PlayerSignal signal, Func<int> action) =>
       _playerBag[signal] = action;

    public void Allocate(CubeSignal signal, Func<int> action) =>
       _cubeBag[signal] = action;


    public Task AllocateAsync(PlayerSignal signal, Func<int> action)
    {
       _playerBag[signal] = action;
       return Task.CompletedTask;
    }

    public Task AllocateAsync(CubeSignal signal, Func<int> action)
    {
       _cubeBag[signal] = action;
       return Task.CompletedTask;
    }


    public Func<int> GetAction(PlayerSignal signal) =>
        _playerBag[signal];

    public Func<int> GetAction(CubeSignal signal) =>
        _cubeBag[signal];


    public bool TryGetAction(PlayerSignal signal, out Func<int> action)
    {
        if(!_playerBag.ContainsKey(signal))
        {
            action = null;
            return true;
        }
        action = _playerBag[signal];
        return true;
    }

    public bool TryGetAction(CubeSignal signal, out Func<int> action)
    {
        if(!_cubeBag.ContainsKey(signal))
        {
            action = null;
            return true;
        }
        action = _cubeBag[signal];
        return true;
    }

}
