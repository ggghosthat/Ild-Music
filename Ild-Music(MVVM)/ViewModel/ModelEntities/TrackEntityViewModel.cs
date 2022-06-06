using Ild_Music_CORE.Models.Core.Tracklist_Structure;

namespace Ild_Music_MVVM_.ViewModel.ModelEntities
{
    public sealed class TrackEntityViewModel : Basic.EntityViewModel
    {
        public TrackEntityViewModel(string id, string name) : base(id, name)
        {

        }

        public TrackEntityViewModel(Track track) : base(track)
        {

        }
    }
}
