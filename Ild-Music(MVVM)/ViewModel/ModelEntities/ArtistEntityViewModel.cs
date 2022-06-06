using Ild_Music_CORE.Models.Core.Tracklist_Structure;

namespace Ild_Music_MVVM_.ViewModel.ModelEntities
{
    public class ArtistEntityViewModel : Basic.EntityViewModel
    {
        public ArtistEntityViewModel(string id, string name) : base(id, name)
        {

        }

        public ArtistEntityViewModel(Artist artist) : base(artist)
        {

        }

        
    }
}
