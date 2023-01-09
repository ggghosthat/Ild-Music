using System;

namespace ShareInstances.Exceptions.SynchAreaExceptions
{
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
}