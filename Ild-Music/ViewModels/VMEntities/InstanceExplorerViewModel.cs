using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace Ild_Music.ViewModels
{
    public class InstanceExplorerViewModel : BaseViewModel
    {
        public static readonly string nameVM = "InstanceExplorerVM";
        public override string NameVM => nameVM;

        #region Services
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        #endregion

        #region Properties
        public ObservableCollection<ICoreEntity> Source {get; private set;} = new();
        public IList<ICoreEntity> Output {get; set;} = new List<ICoreEntity>();

        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
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
        public void Supply(IList<ICoreEntity> sourceInput, IList<ICoreEntity> preselected)
        {
            Source.Clear();
            Output.Clear();
            
            sourceInput.ToList().ForEach(i => Source.Add(i));

            if(new HashSet<ICoreEntity>(Source).IsSupersetOf(preselected))
            {
                preselected.ToList().ForEach(i => Output.Add(i));
            }
        }

        public void Arrange(int i, IList<ICoreEntity> preselected = null)
        {
            Source.Clear();
            Output.Clear();
            if (i == 0)
            {
                supporterService.ArtistsCollection
                                .ToList().ForEach(artist => Source.Add(artist));
            }
            else if (i == 1)
            {
                supporterService.PlaylistsCollection
                                .ToList().ForEach(playlist => Source.Add(playlist));
            }
            else if (i == 2)
            {
                supporterService.TracksCollection
                                .ToList().ForEach(track => Source.Add(track));
            }
            
            if (preselected != null)
            {
                Output = preselected;
            }
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
}
