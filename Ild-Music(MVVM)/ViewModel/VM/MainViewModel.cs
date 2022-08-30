using Ild_Music_MVVM_.Command;
using Ild_Music_MVVM_.Services;
using ShareInstances;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        StartViewModel startVM = new ();

        #region Services Properties
        private ViewModelHolderService vmHolder => (ViewModelHolderService)GetService("VMHolder");
        private PlayerService playerService => (PlayerService)GetService("PlayerService");
        public Base.BaseViewModel CurrenttViewModelItem { get; set; }
        #endregion

        #region Player Properties
        public IPlayer PlayerEntity;
        public bool IsPlayerActive => PlayerEntity.PlayerState;
        public string TotalTime => PlayerEntity.TotalTime.ToString("mm':'ss");
        public string CurrentTime => PlayerEntity.CurrentTime.ToString("mm':'ss");
        #endregion

        #region Command Propertoes
        public CommandDelegater PreviousCommand { get; }
        public CommandDelegater NextCommand { get; }
        public CommandDelegater KickCommand { get; }
        public CommandDelegater StopCommand { get; }
        public CommandDelegater TrackTimeChangedCommand { get; }
        #endregion

        #region Ctor
        public MainViewModel() : base()
        {
            ((MainWindowService)GetService("MainWindowAPI")).MainWindow = this;
            vmHolder.AddViewModel("StartVM", startVM);
            CurrenttViewModelItem = vmHolder.GetViewModel("StartVM");

            PlayerEntity = playerService.GetPlayer();
            PlayerEntity.SetNotifier(() => OnPropertyChanged("IsPlayerActive"));

            PreviousCommand = new CommandDelegater(PreviousPlayerCommand, OnCanSwipe);
            NextCommand = new CommandDelegater(NextPlayerCommand, OnCanSwipe);
            KickCommand = new CommandDelegater(ResumePausePlayerCommand, OnCanUsePlayer);
            StopCommand = new CommandDelegater(StopPlayerCommand, OnCanUsePlayer);
            TrackTimeChangedCommand = new CommandDelegater(TrackTimeChanged, null);


            Task.Run(() =>
            {
                while (true)
                {
                    OnPropertyChanged("TotalTime");
                    OnPropertyChanged("CurrentTime");
                }
            });
        }
        #endregion

        #region StartWindow API Methods
        public void SetVM(Base.BaseViewModel baseVM)
        {
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

        private void TrackTimeChanged(object obj) =>
            Debug.WriteLine("value changed");
        #endregion
    }
}
