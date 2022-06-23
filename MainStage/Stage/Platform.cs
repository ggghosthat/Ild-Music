using ShareInstances;

namespace MainStage.Stage
{
    internal class Platform
    {
        //player fields
        private IPlayer _playerInstance;
        public IPlayer PlayerInstance => _playerInstance;


        private ISynchArea _synchArea;
        public ISynchArea SynchArea => _synchArea;
    }
}
