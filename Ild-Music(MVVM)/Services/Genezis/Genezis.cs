using Ild_Music_MVVM_.Services;
using System.Linq;
using Ild_Music_MVVM_.Services.Parents;
using MainStage.Stage;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ild_Music_MVVM_.ViewModel.VM;

namespace Ild_Music_MVVM_.Services.Genezis
{
    internal class Genezis
    {
        //lock object
        private object _locky = new object();

        //dictionary storage which store service instances
        private static IDictionary<string, Service> _serviceStorage = new Dictionary<string, Service>();

        //Platform single instance
        public static Platform StagePlatform { get; private set; }

        public static FactoryContainerViewModel FactoryContainer { get; private set; }

        private static MainStageService mainStageService;
        private static SupporterService supporterService;
        private static FactoryService factoryService;
        private static SubControlService subControlService;
        private static PlayerService playerService;
        private static MainWindowService mainWindowService;
        private static ViewModelHolderService viewModelHolderService;




        /// <summary>
        /// When Genezice is initializing, in thread safe mode for Platform object (tuny core of application) 
        /// it initialize Platform instance and critical important services
        /// after finishing thread locking it starts side services initialization asynchronously.
        /// </summary>
        public Genezis()
        {
            lock (_locky)
            {
                StagePlatform = new Platform().CallPlatform();
                InitServices();
            }

            Task.Run(SideServicesInit);

            FactoryContainer = new();
        }



        #region StorageMethods
        /// <summary>
        ///   Add service to service storage
        /// </summary>
        /// <param name="service">Define service 2 register</param>
        private static void RegistService(Service service) => _serviceStorage.Add(service.ServiceType, service);

        /// <summary>
        ///   Return reference 2 special service (by type name)
        ///   return null type in case of absence
        /// </summary>
        /// <param name="type"> Define name of service</param>
        /// <returns>Returning service</returns>
        public Service GetService(string type)
        {
            if (_serviceStorage.Keys.ToList().Contains(type))
                return _serviceStorage[type];

            return null;
        }
        #endregion

        /// <summary>
        /// In this method first of all initializing parent services,
        /// which hold the general functionality.
        /// 
        /// Than initializing child services, these services hold another 
        /// critical important functionality, wich based on parent services' functionality
        /// 
        /// Then launching side services, wich can be initialized in async mode
        /// </summary>
        private static void InitServices()
        {      
            ParentServicesInit();
            ChildServicesInit();
        }


        private static void ParentServicesInit()
        {
            mainStageService = new();
            supporterService = new();

            RegistService(mainStageService);
            RegistService(supporterService);
        }

        private static void ChildServicesInit()
        {
            factoryService = new(supporterService);
            subControlService = new(supporterService);
            playerService = new();

            RegistService(factoryService);
            RegistService(subControlService);
            RegistService(playerService);
        }

        private static void SideServicesInit()
        {
            mainWindowService = new();
            viewModelHolderService = new();

            RegistService(mainWindowService);
            RegistService(viewModelHolderService);
        }

    }
}
