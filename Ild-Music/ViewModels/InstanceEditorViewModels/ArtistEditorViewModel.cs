using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Selection;

namespace Ild_Music.ViewModels;

public class ArtistEditorViewModel : BaseViewModel
{
    public static readonly string nameVM = "ListVM";        
    public override string NameVM => nameVM;
    
    #region Services
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
    #endregion
    
    public ArtistEditorViewModel()
    {}
}
