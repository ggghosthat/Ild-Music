using Ild_Music_MVVM_.Services;
using System.IO;
using MainStage.Stage;
using ShareInstances;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    internal class StageViewModel : Base.BaseViewModel
    {
        #region Fields
        private MainStageService platformService => (MainStageService)GetService("MainStageProvider");

        private static Platform _platform;
        #endregion

        #region Props
        public ObservableCollection<IPlayer> PlayerList { get; private set; } = new ();
        public ObservableCollection<ISynchArea> SynchList { get; private set; } = new ();


        public string PlayerRow { get; set; }
        public string SynchRow { get; set; }
        #endregion

        public StageViewModel()
        {
            _platform = platformService.Platform;
            FillUpCollections();
        }

        private void FillUpCollections()
        {
            _platform.listPlayers.ToList().ForEach(player => PlayerList.Add(player));
            _platform.listSynchAreas.ToList().ForEach(synch => SynchList.Add(synch));
        }
    }
}
