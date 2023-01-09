using System;

namespace ShareInstances.PlayerResources.Interfaces
{
    public interface ICoreEntity
    {
        public Guid Id { get; }
        public string Name { get; }
    }
}
