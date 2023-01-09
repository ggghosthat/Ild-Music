using System;
using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances.PlayerResources.Base
{
    public class ResourceRoot : ICoreEntity
    {
        public virtual Guid Id { get; set; }

        public virtual string Name { get; set; }
    }
}
