using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ild_Music_MVVM_.View.UISubControls.FactorySubControl
{
    /// <summary>
    /// Define SubControl by it's header
    /// </summary>
    interface IFactorySubControl
    {
        public string Header { get; init; }
    }
}
