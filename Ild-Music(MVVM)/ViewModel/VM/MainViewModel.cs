using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using ShareInstances;
using ShareInstances.PlayerResources.Interfaces;
using System.Diagnostics;
using System.Windows.Media;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        #region Properties
        private ViewModelHolderService vmHolder => (ViewModelHolderService)GetService("VMHolder");
        private PlayerService playerService => (PlayerService)GetService("PlayerService");
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new StartViewModel();

        public IPlayer PlayerEntity;

        private bool isPlayerActive;

        public bool IsPlayerActive => PlayerEntity.PlayerState;
    
        
            
        public ICoreEntity CoreEntity { get; set; }


        public CommandDelegater PreviousCommand { get; }
        public CommandDelegater NextCommand { get; }
        public CommandDelegater KickCommand { get; }
        public CommandDelegater StopCommand { get; }

        #endregion

        #region Ctor
        public MainViewModel() : base()
        {
            var mainWIndowServicew = (MainWindowService)GetService("MainWindowAPI");
            mainWIndowServicew.MainWindow = this;

            PlayerEntity = playerService.GetPlayer();
            PlayerEntity.SetNotifier(() => OnPropertyChanged("IsPlayerActive"));

            PreviousCommand = new CommandDelegater(PreviousPlayerCommand, OnCanSwipe);
            NextCommand = new CommandDelegater(NextPlayerCommand, OnCanSwipe);
            KickCommand = new CommandDelegater(ResumePausePlayerCommand, null);
            StopCommand = new CommandDelegater(StopPlayerCommand, OnCanUsePlayer);
        }
        #endregion


        #region Public Methods
        public void SetVM(Base.BaseViewModel baseVM)
        {
            vmHolder.CleanStorage();
            CurrenttViewModelItem = baseVM;

            if(baseVM is ListViewModel listVM)
                vmHolder.AddViewModel(ListViewModel.NameVM, listVM);
        }

        public void ResetVM(Base.BaseViewModel baseVM) =>
            CurrenttViewModelItem = baseVM;
        #endregion


        #region CommandPredicate
        private bool OnCanUsePlayer(object obj)  =>        
            PlayerEntity.IsEmpty == false;


        private bool OnCanSwipe(object obj) =>
            PlayerEntity.IsSwipe && PlayerEntity.IsEmpty;
        #endregion

        #region Command Methods
        private void PreviousPlayerCommand(object obj) =>
            PlayerEntity.DropPrevious();


        private void NextPlayerCommand(object obj = null) =>
            PlayerEntity.DropNext();


        private void StopPlayerCommand(object obj = null) =>
            PlayerEntity.StopPlayer();


        private void ResumePausePlayerCommand(object obj) =>
            PlayerEntity.Pause_ResumePlayer();

      
        #endregion
    }
}
