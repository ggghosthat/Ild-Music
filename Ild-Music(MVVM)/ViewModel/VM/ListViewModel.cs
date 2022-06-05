using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.ModelEntities;
using System.Collections.ObjectModel;
using System.Linq;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class ListViewModel : Base.BaseViewModel
    {
        //SupporterService wich provide entities supply 2 list representation
        private SupporterService supporterService;



        public ObservableCollection<ArtistEntityViewModel> ArtistsList { get; private set; }
        public ObservableCollection<PlaylistEntityViewModel> PlaylistsList { get; private set; }
        public ObservableCollection<TrackEntityViewModel> TracksList { get; private set; }
        public ListViewModel()
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
        }

        //These method casts list structures from storable types 2 viewable types
        private void CastListStructure()
        {
            ArtistsList = (ObservableCollection<ArtistEntityViewModel>)supporterService.ArtistsSup.Cast<ArtistEntityViewModel>();
            PlaylistsList = (ObservableCollection<PlaylistEntityViewModel>)supporterService.ArtistsSup.Cast<PlaylistEntityViewModel>();
            TracksList = (ObservableCollection<TrackEntityViewModel>)supporterService.ArtistsSup.Cast<TrackEntityViewModel>();
        }
    }
}
