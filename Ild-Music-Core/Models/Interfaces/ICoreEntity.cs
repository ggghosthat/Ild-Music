using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ild_Music_Core.Models.Interfaces
{
    public interface ICoreEntity
    {
        public string Id { get; }
        public string Name { get; }
    }
}
