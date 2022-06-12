using Ild_Music_CORE.Models.Interfaces;

namespace Ild_Music_CORE.Models.Core.Tracklist_Structure
{
    public class ResourceRoot : ICoreEntity
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
    }
}
