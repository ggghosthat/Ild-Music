using Ild_Music_MVVM_.Services;
using MainStage.Stage;
using ShareInstances;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    internal class StageViewModel : Base.BaseViewModel
    {
        #region Fields
        public static readonly string nameVM = "StageVM";

        private MainStageService platformService => (MainStageService)GetService("MainStageProvider");

        #endregion

        #region Props
        public ObservableCollection<IPlayer> PlayerList { get; private set; } = new();
        public ObservableCollection<ISynchArea> SynchList { get; private set; } = new();

        public static Platform Platform { get; private set; }
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

        private void FillUpPlayersCollection()
        {
            PlayerList.Clear();
            Platform.listPlayers.ToList().ForEach(player => PlayerList.Add(player));
        }

        private void FillUpSynchCollection()
        {
            SynchList.Clear();
            Platform.listSynchAreas.ToList().ForEach(synch => SynchList.Add(synch));
        }
        #endregion

        #region Public Methods
        public async void SetPlayerPath(string playerPath)
        {
            await Task.Run(() => Platform.InitPlayer(playerPath));
            FillUpPlayersCollection();
        }

        public async void SetSynchPath(string synchPath)
        {
            await Task.Run(() => Platform.InitSynch(synchPath));
            FillUpSynchCollection();
        }
        #endregion

    }
}
