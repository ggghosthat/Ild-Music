using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
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
        public static readonly string nameVM = "InstanceExplorerVM"
        public override string NameVM => nameVM;

        #region Services
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        #endregion

        #region Properties
        public ObservableCollection<ICoreEntity> Source {get; private set;} = new();
        public IEnumerable<ICoreEntity> Output {get; private set;}
        #endregion

        #region const
        public InstanceExplorerViewModel()
        {}
        #endregion
    }
}
