using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.ViewModel.ModelEntities.Basic;
using System.Collections.ObjectModel;
using Ild_Music_MVVM_.Command;
using System.Diagnostics;
using ShareInstances.PlayerResources.Base;
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


        public CommandDelegater AddCommand { get; }
        public CommandDelegater DeleteCommand { get; }
        public CommandDelegater BackCommand { get; }



        public static ObservableCollection<ICoreEntity> CurrentList { get; set; } = new();
        public ICoreEntity SelectedItem { get; set; }

        
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
                    foreach(var a in supporterService.ArtistSup)
                        CurrentList.Add(a);
                    break;
                case List.PLAYLISTS:
                    foreach (var p in supporterService.PlaylistSup)
                        CurrentList.Add(p);
                    break;
                case List.TRACKS:
                    foreach (var t in supporterService.TrackSup)
                        CurrentList.Add(t);
                    break;
            }            
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