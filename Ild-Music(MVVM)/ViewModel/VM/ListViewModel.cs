using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.ModelEntities;
using Ild_Music_MVVM_.ViewModel.ModelEntities.Basic;
using System.Collections.ObjectModel;
using System.Linq;
using Ild_Music_MVVM_.Command;
using System.Threading.Tasks;
using System;

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
        public ObservableCollection<EntityViewModel> ArtistsList { get; private set; } = new ();
        public ObservableCollection<EntityViewModel> PlaylistsList { get; private set; } = new ();
        public ObservableCollection<EntityViewModel> TracksList { get; private set; } = new ();

        public ObservableCollection<EntityViewModel> CurrentList { get; private set; } = new ();
        public string ListHeader { get; set; }
        #endregion

        #region Ctors
        //Postdefinning ListType Constructor
        public ListViewModel()
        { 

        }




        #endregion

        #region private methods
        //These method casts list structures from storable types 2 viewable types
        private async void CastListStructure()
        {
            supporterService.ArtistsSup.ToList().ForEach(a => ArtistsList.Add(new ArtistEntityViewModel(a) ) );
            supporterService.PlaylistSup.ToList().ForEach(p => PlaylistsList.Add(new PlaylistEntityViewModel(p) ) );
            supporterService.TrackSup.ToList().ForEach(t => TracksList.Add(new TrackEntityViewModel(t) ) );
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
                    CurrentList = PlaylistsList;
                    ListHeader = "Your playlists :";
                    break;
                case ListType.TRACKS:
                    CurrentList = (ObservableCollection<EntityViewModel>)TracksList.Cast<EntityViewModel>();
                    ListHeader = "Your tracks :";
                    break;
            }
            
        }


        //Call supporter service and cast lists structures
        public ListViewModel CallServiceAndCastLists()
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
            return this;
        }

        //Predefinning ListType
        public ListViewModel CallServiceAndCastLists(ListType listType)
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
            SetListType(listType);
            return this;
        }
        #endregion
    }
}