namespace Ild_Music.Core.Exceptions.ComponentLoadExceptions;
public class PlayerUnableLoadException : System.Exception
{
    public PlayerUnableLoadException()
    {
    }

    public PlayerUnableLoadException(string message) : base(message)
    {
        
    }

    public PlayerUnableLoadException(string message, Exception inner) : base(message, inner)
    {
    }
}
