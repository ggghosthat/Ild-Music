using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.ModelEntities;
using Ild_Music_MVVM_.ViewModel.ModelEntities.Basic;
using System.Collections.ObjectModel;
using System.Linq;
using Ild_Music_MVVM_.Command;
using System.Threading.Tasks;
using System;
using System.Windows;
using System.Collections.Generic;

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
        public List<EntityViewModel> ArtistsList { get; private set; }
        public List<EntityViewModel> PlaylistsList { get; private set; }
        public List<EntityViewModel> TracksList { get; private set; }

        public static ObservableCollection<EntityViewModel> CurrentList { get; set; } = new();
        public object Icon { get; set; }

        #endregion

        #region Ctors
        //Postdefinning ListType Constructor

        public ListViewModel()
        {

        }
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
            //supporterService.ArtistsSup.ToList().ForEach(a => ArtistsList.Add(new ArtistEntityViewModel(a) ) );
            //supporterService.PlaylistSup.ToList().ForEach(p => PlaylistsList.Add(new PlaylistEntityViewModel(p) ) );
            //supporterService.TrackSup.ToList().ForEach(t => TracksList.Add(new TrackEntityViewModel(t) ) );

            ArtistsList = new List<EntityViewModel>{new ArtistEntityViewModel("123", "hello"), new ArtistEntityViewModel("123", "hello"), new ArtistEntityViewModel("123", "hello")};
            PlaylistsList = new List<EntityViewModel>{new PlaylistEntityViewModel("123", "Phello"), new PlaylistEntityViewModel("123", "Phello"), new PlaylistEntityViewModel("123", "Phello")};
            TracksList = new List<EntityViewModel>{new TrackEntityViewModel("123", "Thello"), new TrackEntityViewModel("123", "Thello"), new TrackEntityViewModel("123", "Thello")};
        }
        #endregion

        #region Public Methods
        //Define type of list to present in CurrentList
        public void SetListType(ListType listType)
        {
            CurrentList.Clear();
            switch (listType)
            {                
                case ListType.ARTISTS:                    
                    foreach(var a in ArtistsList)
                        CurrentList.Add(a);
                    Icon = Application.Current.FindResource("ArtistsIcon");
                    break;
                case ListType.PLAYLISTS:
                    foreach (var p in PlaylistsList)
                        CurrentList.Add(p);
                    Icon = Application.Current.FindResource("PlaylistIcon");
                    break;
                case ListType.TRACKS:
                    foreach (var t in TracksList)
                        CurrentList.Add(t);
                    Icon = Application.Current.FindResource("TracksIcon");
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