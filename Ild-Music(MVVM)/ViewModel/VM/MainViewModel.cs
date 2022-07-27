using Ild_Music_CORE.Models.Core.Session_Structure;
using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using ShareInstances.PlayerResources.Interfaces;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        #region Fields
        private static BackList<Base.BaseViewModel> _backList = new();
        #endregion

        #region Properties
        private ViewModelHolderService vmHolder => (ViewModelHolderService)GetService("VMHolder");
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new StartViewModel();

        public NAudioPlayer PlayerEntity { get; set; } = new NAudioPlayer();

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

            PreviousCommand = new CommandDelegater(PreviousPlayerCommand, OnCanSwipe);
            NextCommand = new CommandDelegater(NextPlayerCommand, OnCanSwipe);
            KickCommand = new CommandDelegater(PlayPlayerCommand, OnCanUsePlayer);
            KickCommand = new CommandDelegater(StopPlayerCommand, OnCanUsePlayer);
        }
        #endregion


        #region Public Methods
        public void SetVM(Base.BaseViewModel baseVM)
        {
            CurrenttViewModelItem = baseVM;
            if(baseVM is ListViewModel listVM)
            {
                vmHolder.AddViewModel(ListViewModel.NameVM, listVM);
            }
        }

        public void AddVM(Base.BaseViewModel baseVM)
        {
            _backList.Clear();
            _backList.Add(CurrenttViewModelItem);
            _backList.Add(baseVM);
            CurrenttViewModelItem = _backList.Peek();
        }

        public void SetPriviousVM()
        {
            CurrenttViewModelItem = _backList.Peek();
        }
        #endregion


        #region CommandPredicate
        private bool OnCanUsePlayer(object obj)  =>        
            PlayerEntity.isEmpty;


        private bool OnCanSwipe(object obj) =>
            PlayerEntity.isSwipe && PlayerEntity.isEmpty;
        #endregion

        #region Command Methods
        private void PreviousPlayerCommand(object obj = null) =>
            PlayerEntity.DropPrevious();


        private void NextPlayerCommand(object obj = null) =>
            PlayerEntity.DropNext();


        private void StopPlayerCommand(object obj = null) =>
            PlayerEntity.StopPlayer();


        //???
        private void ResumePausePlayerCommand(object obj) =>
            PlayerEntity.Pause_ResumePlayer();

        //???
        private void PlayPlayerCommand(object obj) =>
            PlayerEntity.Play();
        #endregion
    }
}
