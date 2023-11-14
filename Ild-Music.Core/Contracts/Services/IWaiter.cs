namespace Ild_Music.Core.Contracts.Services.Interfaces;
public interface IWaiter
{
    public Guid WaiterId {get; init;}
    public ReadOnlyMemory<char> WaiterName {get; init;}
}
