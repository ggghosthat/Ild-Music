using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.ModelEntities;
using Ild_Music_MVVM_.ViewModel.ModelEntities.Basic;
using System.Collections.ObjectModel;
using System.Linq;
using Ild_Music_MVVM_.Command;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    //Types of Lists
    public enum ListType
    {
        ARTISTS,
        PLAYLISTS,
        TRACKS
    }

    public class ListViewModel : Base.BaseViewModel
    {
        #region Fields
        //SupporterService wich provide entities supply 2 list representation
        private SupporterService supporterService;
        #endregion

        #region Properties
        public ObservableCollection<ArtistEntityViewModel> ArtistsList { get; private set; }
        public ObservableCollection<PlaylistEntityViewModel> PlaylistsList { get; private set; }
        public ObservableCollection<TrackEntityViewModel> TracksList { get; private set; }

        public ObservableCollection<EntityViewModel> CurrentList { get; private set; }
        public string ListHeader { get; private set; }
        #endregion

        #region Ctors
        //Postdefinning ListType Constructor
        public ListViewModel()
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
        }

        //Predefinning ListType Constructor
        public ListViewModel(ListType listType)
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
            SetListType(listType);
        }
        #endregion

        #region private methods
        //These method casts list structures from storable types 2 viewable types
        private void CastListStructure()
        {
            ArtistsList = (ObservableCollection<ArtistEntityViewModel>)supporterService.ArtistsSup.Cast<ArtistEntityViewModel>();
            PlaylistsList = (ObservableCollection<PlaylistEntityViewModel>)supporterService.ArtistsSup.Cast<PlaylistEntityViewModel>();
            TracksList = (ObservableCollection<TrackEntityViewModel>)supporterService.ArtistsSup.Cast<TrackEntityViewModel>();
        }
        #endregion

        #region Public Methods
        //Define type of list to present in CurrentList
        public void SetListType(ListType listType)
        {
            switch (listType)
            {
                case ListType.ARTISTS:
                    CurrentList = (ObservableCollection<EntityViewModel>)ArtistsList.Cast<EntityViewModel>();
                    ListHeader = "Your artists :";
                    break;
                case ListType.PLAYLISTS:
                    CurrentList = (ObservableCollection<EntityViewModel>)PlaylistsList.Cast<EntityViewModel>();
                    ListHeader = "Your playlists :";
                    break;
                case ListType.TRACKS:
                    CurrentList = (ObservableCollection<EntityViewModel>)TracksList.Cast<EntityViewModel>();
                    ListHeader = "Your tracks :";
                    break;
            }
        }
        #endregion
    }
}