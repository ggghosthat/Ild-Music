using Ild_Music_MVVM_.Services;
using Ild_Music_MVVM_.Services.Parents;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Ild_Music_MVVM_.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields
        
        private static ServiceCenter serviceCenter = App.serviceCenter;
        private static BackListService backListService = (BackListService)serviceCenter.GetService("BackListService");
        #endregion

        #region Constructor
        public BaseViewModel()
        {
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Methods

        //Notify if any property changed
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///Return reference 2 special service (by type name)
        ///return null type in case of absence
        /// </summary>
        /// <param name="type"> Define name of service</param>
        /// <returns>Returning service</returns>
        protected Service GetService(string type) => serviceCenter.GetService(type);


        protected void Add2BackList(BaseViewModel viewModel) =>
            backListService.Add(viewModel);

        protected BaseViewModel PekkFromBackList() =>
            backListService.Peek();
        #endregion
    }
}
