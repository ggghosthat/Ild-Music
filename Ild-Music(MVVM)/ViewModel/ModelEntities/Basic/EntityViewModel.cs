using Ild_Music_CORE.Models.Interfaces;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;

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

        public EntityViewModel(Tracklist tracklist)
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
