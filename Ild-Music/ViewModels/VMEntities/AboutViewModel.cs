using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;
public class AboutViewModel : BaseViewModel
{
    public static readonly string nameVM = "AboutVM";
    public override string NameVM => nameVM;

    public AboutViewModel()
    {
    }
}