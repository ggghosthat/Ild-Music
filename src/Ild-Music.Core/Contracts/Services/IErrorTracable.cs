using Ild_Music.Core.Exceptions.Flag;

namespace Ild_Music.Core.Contracts;

public interface IErrorTracable
{
    public List<ErrorFlag> Errors { get; }

    public bool CheckErrors(List<ErrorFlag> errorList);
}