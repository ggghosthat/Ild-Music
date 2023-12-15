namespace Ild_Music.Core.Exceptions.ScopeCastleExceptions;
public sealed class ComponentResolveException : Exception
{
    public ComponentResolveException()
    {
    }

    public ComponentResolveException(string message) : base(message)
    {
    }

    public ComponentResolveException(string message, Exception inner) : base(message, inner)
    {
    }
}
