using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class FactoryViewModel : Base.BaseViewModel
    {
        public ObservableCollection<FactoryEntity> EntityFactories { get; set; } = new()
        {
            new ArtistFactoryEntity("ArtistFactory"),
            new PlaylistFactoryEntity("PlaylistFactory"),
            new TrackFactoryEntity("TrackFactory")
        };

        public FactoryEntity CurrentFactoryEntity { get; set; }

        public FactoryViewModel()
        {
            CurrentFactoryEntity = EntityFactories[0];
        }

    }
}
