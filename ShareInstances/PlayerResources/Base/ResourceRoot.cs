using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances.PlayerResources.Base
{
    public class ResourceRoot : ICoreEntity
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
    }
}
