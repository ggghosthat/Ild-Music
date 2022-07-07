using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.Services.Parents;
using MainStage.Stage;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    internal class StageViewModel : Base.BaseViewModel
    {
        #region Fields
        private MainStageService platformService => (MainStageService)GetService("MainStageProvider");

        private static Platform _platform;
        #endregion

        #region Props
        public string PlayerRow { get; set; }

        public string SynchRow { get; set; }
        #endregion

        public StageViewModel()
        {
            _platform = platformService.Platform;
        }
    }
}
