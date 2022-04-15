using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.Services.Parents;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ild_Music_MVVM_.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Service Register
        Dictionary<string, Service> services = new();
        #endregion

        #region Constructor
        public BaseViewModel()
        {
            var area = new Area();
            var supporter = new SupporterService(area);

            var factory = new FactoryService(supporter);

            RegistService(supporter);
            RegistService(factory);
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods
        //Notify if any property changed
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //Add service to register
        protected void RegistService (Service service) => services.Add(service.ServiceType, service);

        //Return reference 2 special service (by type name)
        //return null type in case of absence
        protected Service GetService (string type) {

            if (services.Keys.ToList().Contains(type))
                return services[type];
            
            return null;
        }
        
        #endregion
    }
}
