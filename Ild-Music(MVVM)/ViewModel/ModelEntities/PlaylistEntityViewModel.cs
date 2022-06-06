using Ild_Music_CORE.Models.Core.Tracklist_Structure;

namespace Ild_Music_MVVM_.ViewModel.ModelEntities
{
    public sealed class PlaylistEntityViewModel : Basic.EntityViewModel
    {
        public PlaylistEntityViewModel(string id, string name) : base(id, name)
        {

        }

        public PlaylistEntityViewModel(Tracklist tracklist) : base(tracklist)
        {

        }
    }
}
