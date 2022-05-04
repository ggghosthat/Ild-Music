using Ild_Music_MVVM_.Services.Parents;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ild_Music_MVVM_.Services
{
    public class ServiceCenter
    {
        #region Singleton fields
        private static ServiceCenter serviceCenter = null;
        private static readonly object padlock = new object();
        #endregion

        #region Service Register
        Dictionary<string, Service> services = new();
        #endregion

        #region Singleton Initialization
        private ServiceCenter()
        {
            UpServices();
        }

        /// <summary>
        /// Property 2 get instance of this class (because it's singleton)
        /// </summary>
        public static ServiceCenter Instance
        {
            get 
            {
                if (serviceCenter == null)
                {
                    lock (padlock)
                    {
                        serviceCenter = new ServiceCenter();
                        return serviceCenter;
                    }
                }
                return serviceCenter;
            }
        }
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
            var area = new Area();
            var supporter = new SupporterService(area);

            var factory = new FactoryService(supporter);

            var controlHandler = new ControlHandlerService();


            RegistService(supporter);
            RegistService(factory);
            RegistService(controlHandler);
        }
        #endregion
    }
}
