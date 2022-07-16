using Ild_Music_MVVM_.Services;
using System.IO;
using MainStage.Stage;
using ShareInstances;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Ild_Music_MVVM_.Command;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    internal class StageViewModel : Base.BaseViewModel
    {
        #region Fields
        private MainStageService platformService => (MainStageService)GetService("MainStageProvider");

        #endregion

        #region Props
        public ObservableCollection<IPlayer> PlayerList { get; private set; } = new();
        public ObservableCollection<ISynchArea> SynchList { get; private set; } = new();

        public Platform Platform { get; private set; }
        #endregion


        #region Constructor
        public StageViewModel()
        {
            Platform = platformService.Platform;
            FillUpCollections();
        }
        #endregion

        #region Private Methods
        private void FillUpCollections()
        {
            Platform.listPlayers.ToList().ForEach(player => PlayerList.Add(player));
            Platform.listSynchAreas.ToList().ForEach(synch => SynchList.Add(synch));
        }
        #endregion
    }
}
