using System;

namespace ShareInstances.Services.Interfaces;
public interface IGhost 
{
    public ReadOnlyMemory<char> GhostName { get; init; }
}
