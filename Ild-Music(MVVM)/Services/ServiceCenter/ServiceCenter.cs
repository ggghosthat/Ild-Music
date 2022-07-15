using Ild_Music_MVVM_.Services.Parents;
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

        #region Service Register
        private Dictionary<string, Service> services = new();
        #endregion

        #region Singleton Initialization
        public ServiceCenter() =>
            UpServices();

        

       
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
        /// Enable services
        /// </summary>
        private void UpServices()
        {
            var mainStage = new MainStageService();
            //var area = new Area();
            var supporter = new SupporterService();

            var factory = new FactoryService(supporter);

            var playerService = new PlayerService();

            var subControll = new SubControlService();



            RegistService(mainStage);
            RegistService(supporter);
            RegistService(factory);
            RegistService(playerService);
            RegistService(subControll);


            Debug.WriteLine("Service center is ready 2 use");
        }
        #endregion
    }
}
