using Ild_Music_MVVM_.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ild_Music_MVVM_.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Service Register
        Dictionary<string, IService> services = new();
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
        internal void RegistService (IService service) => services.Add(service.ServiceType, service);

        //Return reference 2 special service (by type name)
        //return null type in case of absence
        internal IService GetService (string type) {

            if (services.Keys.ToList().Contains(type))
                return services[type];
            
            return null;
        }
        
        #endregion
    }
}
