using ShareInstances.PlayerResources;
using NAudioPlayerCore.Models;

namespace Test
{
	public class  Program
	{
		public static void Main(string[] args)
		{
			var t = new Track("E:\\music\\2_5395688420926294564.mp3","killer");

			var player = new NAudioPlayer();
			player.SetInstance(t);


			while(true)
			{
				var x = Console.ReadLine();

				if (x == "play")
				{
					player.Pause_ResumePlayer();
				}
				else if(x =="q")
				{
					break;
				}
			}
		}
	}	
}