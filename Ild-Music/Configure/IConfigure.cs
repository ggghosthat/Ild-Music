using System.Collections.Generic;

namespace Ild_Music.Configure
{
    public interface IConfigure
    {
        public string ComponentsFile {get; init;}
        
        public IEnumerable<string> Players {get; set;}
        public IEnumerable<string> Synches {get; set;}

        public void Parse();
    }
}
