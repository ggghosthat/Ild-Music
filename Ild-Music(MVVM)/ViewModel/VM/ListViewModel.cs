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
using ShareInstances.PlayerResources.Interfaces;

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
        private static readonly string nameVM = "ListVM";


        //SupporterService wich provide entities supply 2 list representation
        private SupporterService supporterService;
        private MainWindowService _mainWindowAPI => (MainWindowService)GetService("MainWindowAPI");
        private FactoryService factoryService => (FactoryService)GetService("Factory");
        private ViewModelHolderService vmHolder => (ViewModelHolderService)GetService("VMHolder");

        private List listType;
        #endregion

        #region Properties
        public static string NameVM => nameVM;

        public List<ICoreEntity> ArtistsList { get; private set; } = new();
        public List<ICoreEntity> PlaylistsList { get; private set; } = new();
        public List<ICoreEntity> TracksList { get; private set; } = new ();


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
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);
        }

        public ListViewModel(List listType)
        {
            AddCommand = new(Add, null);
            DeleteCommand = new(Delete, null);
            BackCommand = new(Back, null);

            supporterService = (SupporterService)GetService("Supporter");
            SetListType(listType);
        }
        #endregion

        #region private methods
        //These method casts list structures from storable types 2 viewable types
        private void CastListStructure()
        {
            ArtistsList.AddRange(supporterService.ArtistSup);
            PlaylistsList.AddRange(supporterService.PlaylistSup);
            TracksList.AddRange(supporterService.TrackSup);
        }
        #endregion

        #region Public Methods
        //Define type of list to present in CurrentList
        public void SetListType(List listType)
        {
            this.listType = listType;
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
        

        private void Add(object obj)
        {
            factoryService.FactoryContainerViewModel = new();

            switch (listType)
            {
                case List.ARTISTS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(0);
                    break;
                case List.PLAYLISTS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(1);
                    break;
                case List.TRACKS:
                    factoryService.FactoryContainerViewModel.DisplayInstance(2);
                    break;
            }
            _mainWindowAPI.MainWindow.AddVM(factoryService.FactoryContainerViewModel);
            vmHolder.AddViewModel(nameVM, this);
        }


        private void Delete(object obj) =>
            Debug.WriteLine("This is dummy list control removing");

        private void Back (object obj) =>
            Debug.WriteLine("This is dummy list control going back");


        #endregion
    }
}