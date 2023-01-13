using ShareInstances.Services.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace ShareInstances.Services.Center
{
    public class ServiceCenter : ICenter
    {
        public bool IsCenterActive { get; set; } = false;

        private Dictionary<string, IService> serviceRegister = new();

        #region App services
        private Entities.SupporterService supporterService = new();
        private Entities.FactoryService factoryService = new();
        private Entities.PlayerService playerService = new();
        #endregion
        #region UI services
        private Entities.UIControlService<object> controlService = new ();
        private Entities.ViewModelHolder<object> holder = new();
        #endregion

        //I dont know what here happens,
        //but please dont remove constructor params
        public ServiceCenter()
        {
            OnCenterRegisterActivate();
        }

        public void OnCenterRegisterActivate()
        {
            RegistService((IService)supporterService);
            RegistService((IService)playerService);
            RegistService((IService)factoryService);
            RegistService((IService)controlService);
            RegistService((IService)holder);
            
            IsCenterActive = true;
        }

        public void RegistService(IService service) =>
            serviceRegister.Add(service.ServiceName, service);

        public void UpdateService(IService service) =>
            serviceRegister[service.ServiceName] = service;
        
        public IService GetService(string name)
        {
            if (serviceRegister.Keys.ToList().Contains(name))
                return serviceRegister[name];
            return null;
        }
        public IList<string> GetServices() =>
            serviceRegister.ToList().Select(x => x.Value.ServiceName).ToList();
        
    
        public void ResolveSupporter(ISynchArea synchArea)
        {
            var supporter = (SupporterService)GetService(((IService)supporterService).ServiceName);
            var factory = (FactoryService)GetService(((IService)factoryService).ServiceName);

            supporter.StartSynchArea(synchArea);
            factory.SupporterService = supporter;
        }

        public void ResolvePlayer(IPlayer _player)
        {
            var player = (PlayerService)GetService(((IService)playerService).ServiceName);
            player.EnablePlayer(_player);
            UpdateService(player);
        }
    }
}