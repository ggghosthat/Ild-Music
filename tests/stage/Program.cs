using ShareInstances.Stage;

using System.Threading;

class Program
{
	private static Stage stage;
	public async static Task Main(string[] args)
	{
		stage = new ();
		stage.OnInitialized += () => Console.WriteLine(stage.CompletionResult);
		Console.WriteLine("Please, await stage completion ...");
		Console.WriteLine($"Current thread id: {Thread.CurrentThread.ManagedThreadId}");

		Task.Factory.StartNew(async () => await stage.ObserveLoading("../../dependencies/Ild-Music.NAudioPlayerCore/bin/Release/net6.0/Ild-Music-Core.dll",
	 						 "../../dependencies/Ild-Music.SynchronizationBlock/bin/Release/net6.0/SynchronizationBlock.dll"));
	}
}
