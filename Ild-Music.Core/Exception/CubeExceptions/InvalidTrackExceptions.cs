namespace Ild_Music.Core.Exceptions.SynchAreaExceptions;
public class InvalidTrackException : System.Exception
{
    public InvalidTrackException()
    {
    }

    public InvalidTrackException(string message) : base(message)
    {
        
    }

    public InvalidTrackException(string message, Exception inner) : base(message, inner)
    {
    }
}
