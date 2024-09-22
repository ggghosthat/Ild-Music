namespace Ild_Music.Core.Exceptions.ScopeCastleExceptions;
public sealed class ComponentRegisterException : Exception
{
    public ComponentRegisterException()
    {
    }

    public ComponentRegisterException(string message) : base(message)
    {
    }

    public ComponentRegisterException(string message, Exception inner) : base(message, inner)
    {
    }
}
