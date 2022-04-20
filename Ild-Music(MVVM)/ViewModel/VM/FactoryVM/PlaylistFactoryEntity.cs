using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class PlaylistFactoryEntity : FactoryEntity
    {
        public override string Name { get; set; }
        public override string Description { get; set; }
        public IList<Artist> Artists { get; set; }
        public IList<Track> Tracks{ get; set; }

        public PlaylistFactoryEntity(string factoryName) :  base(factoryName)
        {

        }
    }
}
