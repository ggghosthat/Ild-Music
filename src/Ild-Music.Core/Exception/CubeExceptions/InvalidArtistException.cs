namespace Ild_Music.Core.Exceptions.CubeExceptions;
public class InvalidArtistException : System.Exception
{
    public InvalidArtistException()
    {
    }

    public InvalidArtistException(string message) : base(message)
    {
        
    }

    public InvalidArtistException(string message, Exception inner) : base(message, inner)
    {
    }
}
