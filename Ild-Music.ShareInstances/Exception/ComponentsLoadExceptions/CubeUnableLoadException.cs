using System;

namespace ShareInstances.Exceptions.SynchAreaExceptions;
public class CubeUnableLoadException : System.Exception
{
    public CubeUnableLoadException()
    {
    }

    public CubeUnableLoadException(string message) : base(message)
    {
        
    }

    public CubeUnableLoadException(string message, Exception inner) : base(message, inner)
    {
    }
}
