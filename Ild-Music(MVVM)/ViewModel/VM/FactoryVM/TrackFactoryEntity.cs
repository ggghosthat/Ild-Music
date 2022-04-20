using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.ViewModel.VM.FactoryVM
{
    public class TrackFactoryEntity : FactoryEntity
    {
        public override string Name { get; set; }
        public override string Description { get; set; }

        public string Path { get; set; }
        public IList<Artist> ArtistCollection { get; set; }

        public TrackFactoryEntity(string factoryName) : base(factoryName)
        {

        }
    }
}
