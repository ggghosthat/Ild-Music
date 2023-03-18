using System.ComponentModel;
using System.Runtime.CompilerServices;

using Ild_Music;
using ShareInstances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Center;
using ShareInstances.Services.Interfaces;
using ShareInstances.Stage;

namespace Ild_Music.ViewModels.Base;
public class BaseViewModel : INotifyPropertyChanged
{   
    public virtual string NameVM {get; protected set;}
    private ServiceCenter serviceCenter => App.Stage.serviceCenter;
    // private ViewModelHolder<BaseViewModel> vmHolder => (ViewModelHolder<BaseViewModel>)serviceCenter.GetService("HolderService");

    public event PropertyChangedEventHandler PropertyChanged;
    //Notify if any property changed
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    public BaseViewModel()
    {
    }

    /// <summary>
    ///Return reference 2 special service (by type name)
    ///return null type in case of absence
    /// </summary>
    /// <param name="type"> Define name of service</param>
    /// <returns>Returning service</returns>
    protected IService GetService(string name) => 
        serviceCenter.GetService(name);

    protected Stage GetPlatform() => App.Stage;
}
