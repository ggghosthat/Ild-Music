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
using System.Diagnostics;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    //Types of Lists
    public enum List
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

        public CommandDelegater EditCommand { get; }
        public CommandDelegater AddCommand { get; }
        public CommandDelegater DeleteCommand { get; }
        public CommandDelegater BackCommand { get; }


        public static ObservableCollection<EntityViewModel> CurrentList { get; set; } = new();
        public EntityViewModel SelectedItem { get; set; }
        public object Icon { get; set; }

        #endregion

        #region Ctors
        //Postdefinning ListType Constructor

        public ListViewModel()
        {
            EditCommand = new(Edit, null);
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);
        }

        public ListViewModel(List listType)
        {
            EditCommand = new(Edit, null);
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);

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
        public void SetListType(List listType)
        {
            CurrentList.Clear();
            switch (listType)
            {                
                case List.ARTISTS:                    
                    foreach(var a in ArtistsList)
                        CurrentList.Add(new ArtistEntityViewModel(a.Id, a.Name));
                    break;
                case List.PLAYLISTS:
                    foreach (var p in PlaylistsList)
                        CurrentList.Add(new PlaylistEntityViewModel(p.Id, p.Name));
                    break;
                case List.TRACKS:
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
        public ListViewModel CallServiceAndCastLists(List listType)
        {
            supporterService = (SupporterService)GetService("Supporter");
            CastListStructure();
            SetListType(listType);
            return this;
        }
        #endregion

        #region Command Methods
        private void Edit(object obj) =>
            Debug.WriteLine("This is dummy list control editting");

        private void Add(object obj) =>
            Debug.WriteLine("This is dummy list control addition");

        private void Delete(object obj) =>
            Debug.WriteLine("This is dummy list control removing");

        private void Back (object obj) =>
            Debug.WriteLine("This is dummy list control going back");
        #endregion
    }
}