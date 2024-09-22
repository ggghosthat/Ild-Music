namespace Ild_Music.Core.Exceptions.ScopeCastleExceptions;
public sealed class ServiceItemResolveException : Exception
{
    public ServiceItemResolveException()
    {
    }

    public ServiceItemResolveException(string message) : base(message)
    {
    }

    public ServiceItemResolveException(string message, Exception inner) : base(message, inner)
    {
    }
}
