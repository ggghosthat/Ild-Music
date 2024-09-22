namespace Ild_Music.Core.Exceptions.CubeExceptions;
public class InvalidPlaylistException : System.Exception
{
    public InvalidPlaylistException()
    {
    }

    public InvalidPlaylistException(string message) : base(message)
    {
        
    }

    public InvalidPlaylistException(string message, Exception inner) : base(message, inner)
    {
    }
}
