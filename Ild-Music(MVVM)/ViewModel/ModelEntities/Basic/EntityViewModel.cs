

namespace Ild_Music_MVVM_.ViewModel.ModelEntities.Basic
{
    public abstract class EntityViewModel : Base.BaseViewModel
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
