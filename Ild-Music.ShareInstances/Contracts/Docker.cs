using System.Threading.Tasks;
using System.Collections.Generic;
public interface IDocker
{
    public Task<int> Dock();
}
