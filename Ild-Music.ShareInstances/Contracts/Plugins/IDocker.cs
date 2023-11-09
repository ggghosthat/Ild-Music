using System.Threading.Tasks;

namespace ShareInstances.Contracts;
public interface IDocker
{
    public Task<int> Dock();
}
