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
        public EntityViewModel SelectedItem { get; set; }
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
            ArtistsList = new List<EntityViewModel>{new ArtistEntityViewModel("122", "hello"), new ArtistEntityViewModel("123", "hello"), new ArtistEntityViewModel("123", "hello")};
            PlaylistsList = new List<EntityViewModel>{new PlaylistEntityViewModel("122", "Phelo"), new PlaylistEntityViewModel("13", "Pllo")};
            TracksList = new List<EntityViewModel>{new TrackEntityViewModel("122", "Thello"), new TrackEntityViewModel("123", "Thello"), new TrackEntityViewModel("123", "Thello")};
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
                        CurrentList.Add(new ArtistEntityViewModel(a.Id, a.Name));
                    break;
                case ListType.PLAYLISTS:
                    foreach (var p in PlaylistsList)
                        CurrentList.Add(new PlaylistEntityViewModel(p.Id, p.Name));
                    break;
                case ListType.TRACKS:
                    foreach (var t in TracksList)
                        CurrentList.Add(new TrackEntityViewModel(t.Id, t.Name));
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