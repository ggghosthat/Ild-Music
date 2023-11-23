using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Ild_Music.ViewModels;
public class InstanceExplorerViewModel : BaseViewModel
{
    public static readonly string nameVM = "InstanceExplorerVM";
    public override string NameVM => nameVM;

    #region Services
    private SupportGhost supporterService => (SupportGhost)base.GetService(Ghosts.SUPPORT);
    private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    #endregion

    #region Properties
    public ObservableCollection<CommonInstanceDTO> Source {get; private set;} = new();
    public IList<CommonInstanceDTO> Output {get; set;} = new List<CommonInstanceDTO>();
    #endregion

    
    #region Commands
    public CommandDelegator CloseExplorerCommand {get;}
    public CommandDelegator ExitExplorerCommand {get;}
    #endregion

    #region Events
    public event Action OnSelected;
    #endregion

    #region const
    public InstanceExplorerViewModel()
    {
        CloseExplorerCommand = new(CloseExplorer, null);
        ExitExplorerCommand = new(ExitExplorer, null);
    }
    #endregion

    #region Public Methods
    public async void Arrange(EntityTag entitytag,
                        IList<CommonInstanceDTO> preselected = null)
    {
        Source.Clear();
        Output.Clear();

        var required = await supporterService.RequireInstances(EntityTag.ARTIST);
        required.ToList()
                .ForEach(artist => Source.Add(artist));
       
        if (preselected != null)
            Output = preselected;
    }

    public void CloseExplorer(object obj)
    {
        OnSelected?.Invoke();
        MainVM.ResolveWindowStack();
    }

    public void ExitExplorer(object obj)
    {
        Source.Clear();
        Output.Clear();           
        MainVM.ResolveWindowStack();
    }

    #endregion
}
