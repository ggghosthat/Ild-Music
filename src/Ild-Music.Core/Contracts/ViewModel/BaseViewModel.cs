using System.ComponentModel;
using System.Runtime.CompilerServices;

using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.Core.Contracts.ViewModel;
public class BaseViewModel : INotifyPropertyChanged
{   
    public virtual Guid ViewModelId {get; protected set;}

    public event PropertyChangedEventHandler PropertyChanged;

    public BaseViewModel() {}

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    
    // protected IGhost GetGhost(Ghosts ghostTag) =>
    //     default;
        // App.Stage.GetGhost(ghostTag);

    public virtual void Load()
    {}
}