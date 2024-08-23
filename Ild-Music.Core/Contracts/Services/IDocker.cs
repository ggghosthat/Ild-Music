using Ild_Music.Core.Exceptions.Flag;

namespace Ild_Music.Core.Contracts;

public interface IDocker
{
    public IList<ErrorFlag> Errors { get; }
    public ValueTask<int> Dock();
}
