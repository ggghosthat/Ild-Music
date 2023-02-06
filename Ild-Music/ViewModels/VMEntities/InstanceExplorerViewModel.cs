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
        public static readonly string nameVM = "InstanceExplorerVM";
        public override string NameVM => nameVM;

        #region Services
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        #endregion

        #region Properties
        public ObservableCollection<ICoreEntity> Source {get; private set;} = new();
        public IList<ICoreEntity> Output {get; set;}
        #endregion

        #region const
        public InstanceExplorerViewModel()
        {
            Source.Add(new Artist("Eminem"));
            Source.Add(new Artist("Dr. Dre"));
        }
        #endregion

        #region Public Methods
        public void Arrange(int i, IList<ICoreEntity> preselected = null)
        {
            Source.Clear();
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
        #endregion
    }
}
