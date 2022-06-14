using System;
using System.Collections.Generic;
using System.Text;

namespace Ild_Music_CORE.Models.Core.Session_Structure.Interfaces
{
    public interface IPlayer
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }
    }
}
