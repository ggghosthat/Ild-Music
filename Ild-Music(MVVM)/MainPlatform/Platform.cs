using Ild_Music_CORE.Models.Core.Session_Structure.Interfaces;
using SynchronizationBlock.Models.SynchArea;

namespace Ild_Music_MVVM_.MainPlatform
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
