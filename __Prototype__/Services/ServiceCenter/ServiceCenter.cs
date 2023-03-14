﻿using Ild_Music_MVVM_.Services.Parents;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Ild_Music_MVVM_.Services
{
    //These class Hold all services and provide service supply 
    public class ServiceCenter
    {

        public bool IsActive { get; private set; } = false;

        #region Service Register
        private Dictionary<string, Service> services = new();
        #endregion

        #region Singleton Initialization
        public ServiceCenter() =>
            InitServices();           
        #endregion


        #region Methods
        /// <summary>
        ///Add service to register
        /// </summary>
        /// <param name="service">Define service 2 register</param>
        protected void RegistService(Service service) => services.Add(service.ServiceType, service);

        /// <summary>
        ///Return reference 2 special service (by type name)
        ///return null type in case of absence
        /// </summary>
        /// <param name="type"> Define name of service</param>
        /// <returns>Returning service</returns>
        public Service GetService(string type)
        {
            if (services.Keys.ToList().Contains(type))
                return services[type];

            return null;
        }


        /// <summary>
        /// Enable other platform dependened services
        /// </summary>
        public void InitServices()
        {
            var mainStageService = new MainStageService();

            var supporterService = new SupporterService();

            var factoryService = new FactoryService(supporterService);

            var subControllService = new SubControlService();

            var tabHolderService = new TabHolderService(subControllService);

            var playerService = new PlayerService();

            var mainWindowService = new MainWindowService();

            RegistService(mainStageService);
            RegistService(supporterService);
            RegistService(factoryService);
            RegistService(playerService);
            RegistService(subControllService);
            RegistService(tabHolderService);
            RegistService(mainWindowService);

            IsActive = true;
            Debug.WriteLine("platform dependened services ready touse");
        }
        #endregion
    }
}
