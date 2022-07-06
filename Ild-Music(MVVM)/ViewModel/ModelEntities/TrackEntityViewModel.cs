using ShareInstances.PlayerResources;

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
