
using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.ViewModel.Base;
using System;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    internal class ViewModelHolderService : Service
    {
        public override string ServiceType { get; init; } = "VMHolder";

        private static IDictionary<string, BaseViewModel> vmStorage = new Dictionary<string, BaseViewModel>();

        public event Action OnViewModelUpdate;


        public void AddViewModel(string name, BaseViewModel viewmodel)
        {
            if (!vmStorage.ContainsKey(name))
                vmStorage.Add(name, viewmodel);
            else
            {
                vmStorage[name] = null;
                vmStorage[name] = viewmodel;
            }
            OnViewModelUpdate?.Invoke();
        }

        public BaseViewModel GetViewModel(string name)
        {
            if (vmStorage.ContainsKey(name))
                return vmStorage[name];
            return null;
        }

        public void RemoveViewModel(string name)
        {
            if (vmStorage.ContainsKey(name))
                vmStorage.Remove(name);
        }

        public void CleanStorage() =>
            vmStorage.Clear();

    }
}
