using Ild_Music_Core.Models.Interfaces;

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
    }
}
