using System.ComponentModel;
using System.Runtime.CompilerServices;

using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.ViewModels.Base;
public class BaseViewModel : INotifyPropertyChanged
{   
    public virtual string NameVM {get; protected set;}

    public event PropertyChangedEventHandler PropertyChanged;


    public BaseViewModel() {}


    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    

    protected IGhost GetGhost(Ghosts ghostTag)
    {
        return App.Stage.GetGhost(ghostTag);
    }

    protected IWaiter GetWaiter(string waiterName)
    {
        return App.Stage.GetWaiter(ref waiterName); 
    }
}
