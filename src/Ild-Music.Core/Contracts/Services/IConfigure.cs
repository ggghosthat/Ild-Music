using Ild_Music.Core.Configure;

namespace Ild_Music.Core.Contracts;

public interface IConfigure
{
    public ReadOnlyMemory<char> ComponentsFile {get; init;}
 	public Config ConfigSheet {get; set;}    

    public void Parse();
}
