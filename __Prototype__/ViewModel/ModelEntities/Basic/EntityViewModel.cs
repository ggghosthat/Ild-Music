using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;

namespace Ild_Music_MVVM_.ViewModel.ModelEntities.Basic
{
    public abstract class EntityViewModel : Base.BaseViewModel, ICoreEntity
    {
        public string Id { get; }
        public string Name { get; }

        public EntityViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public EntityViewModel(Artist artist)
        {
            Id = artist.Id;
            Name = artist.Name;
        }

        public EntityViewModel(Playlist tracklist)
        {
            Id = tracklist.Id;
            Name = tracklist.Name;
        }
        public EntityViewModel(Track track)
        {
            Id = track.Id;
            Name = track.Name;
        }
    }
}
