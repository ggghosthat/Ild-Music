using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.ViewModel.Base;
using Ild_Music_MVVM_.ViewModel.VM;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.Services
{
    internal class ViewModelHolder
    {

        private static IDictionary<string, BaseViewModel> vmStorage = new Dictionary<string, BaseViewModel>();

        public event Action OnViewModelUpdate;

        public ViewModelHolder()
        {
            InitCore();
        }

        private void InitCore()
        {
            vmStorage.Add(ListViewModel.nameVM, new ListViewModel());
            vmStorage.Add(StartViewModel.nameVM, new StartViewModel());
            vmStorage.Add(StageViewModel.nameVM, new StageViewModel());
            vmStorage.Add(SubControlViewModel.nameVM, new StageViewModel());
            vmStorage.Add(FactoryContainerViewModel.nameVM, new FactoryContainerViewModel());
        }

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
