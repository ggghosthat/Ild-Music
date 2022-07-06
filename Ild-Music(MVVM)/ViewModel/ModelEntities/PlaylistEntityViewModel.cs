using ShareInstances.PlayerResources;

namespace Ild_Music_MVVM_.ViewModel.ModelEntities
{
    public sealed class PlaylistEntityViewModel : Basic.EntityViewModel
    {
        public PlaylistEntityViewModel(string id, string name) : base(id, name)
        {
        }

        public PlaylistEntityViewModel(Playlist tracklist) : base(tracklist)
        {
        }
    }
}
