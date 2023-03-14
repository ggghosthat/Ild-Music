using Ild_Music_CORE.Models.Core.Session_Structure;
using ShareInstances;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    /// <summary>
    /// This service allow use one player from multiple of them
    /// with this service you govern your players
    /// </summary>
    public class PlayerService : Parents.Service
    {
        //This dictionary represent player pool 
        //which hold player entities and indentify them by special name(key)
        #region PlayerPool
        private Dictionary<string, IPlayer> _playersPool = new();
        #endregion

        #region ServiceName
        public override string ServiceType { get; init; } = "PlayerService";
        #endregion

        #region ctor
        public PlayerService()
        {
            _playersPool.Add("naudio", new NAudioPlayer());
        }
        #endregion

        #region Public Methods
        public IPlayer GetPlayer(string name = "naudio")
        {
            if (_playersPool.ContainsKey(name))
                return _playersPool[name];
            return null;
        }

        public void AddPlayer(string name, IPlayer _player)
        {
            if (!_playersPool.ContainsKey(name))
                _playersPool.Add(name, _player);
        }

        public void DeletePlayer(string name)
        {
            if (!_playersPool.ContainsKey(name))
                _playersPool.Remove(name);
        }

        
        #endregion

    }
}
