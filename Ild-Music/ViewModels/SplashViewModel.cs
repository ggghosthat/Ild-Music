using Ild_Music.Core.Contracts;
using Ild_Music.Core.Events;
using Ild_Music.Core.Events.Signals;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;

namespace Ild_Music.ViewModels;

public class SplashScreenViewModel : Base.BaseViewModel
{
    public SplashScreenViewModel()
    {
        
    }
}