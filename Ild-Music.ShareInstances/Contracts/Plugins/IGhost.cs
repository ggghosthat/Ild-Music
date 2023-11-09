using System;

namespace ShareInstances.Contracts.Services.Interfaces;
public interface IGhost 
{
    public ReadOnlyMemory<char> GhostName { get; init; }
}
