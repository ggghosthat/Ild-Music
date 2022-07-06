using Ild_Music_CORE.Models.Core.Session_Structure;
using Ild_Music_MVVM_.Command;
using ShareInstances.PlayerResources.Interfaces;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class MainViewModel : Base.BaseViewModel
    {
        public Base.BaseViewModel CurrenttViewModelItem { get; set; } = new FactoryContainerViewModel();

        public NAudioPlayer PlayerEntity { get; set; } = new NAudioPlayer();

        public ICoreEntity CoreEntity { get; set; }


        public CommandDelegater PreviousCommand { get; }
        public CommandDelegater NextCommand { get; }
        public CommandDelegater KickCommand { get; }
        public CommandDelegater StopCommand { get; }

        public MainViewModel() : base()
        {
            PreviousCommand = new CommandDelegater(PreviousPlayerCommand, OnCanSwipe);
            NextCommand = new CommandDelegater(NextPlayerCommand, OnCanSwipe);
            KickCommand = new CommandDelegater(PlayPlayerCommand, OnCanUsePlayer);
            KickCommand = new CommandDelegater(StopPlayerCommand, OnCanUsePlayer);
        }



        #region CommandPredicate
        private bool OnCanUsePlayer(object obj)  =>        
            PlayerEntity.isEmpty;


        private bool OnCanSwipe(object obj) =>
            PlayerEntity.isSwipe && PlayerEntity.isEmpty;
        #endregion

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
    }
}
