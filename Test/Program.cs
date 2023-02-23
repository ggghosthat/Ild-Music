using ShareInstances.PlayerResources;
using NAudioPlayerCore.Models;

using System;
using System.Collections.Generic;

namespace Test
{
	public class  Program
	{
		public static void Main(string[] args)
		{
			var t = new Track("E:\\music\\2_5395688420926294564.mp3","killer");
			var t1 = new Track("E:\\music\\2_5267116294292899281.mp3","Airplanes");
			var t2 = new Track("E:\\music\\2_5321211098148373716.mp3","If i was a guy");
			var p = new Playlist(new List<Track>(){t,t1,t2});
			var player = new NAudioPlayer();

			player.SetInstance(p);


			while(true)
			{
				var x = Console.ReadLine();

				if (x == "play")
				{
					player.Pause_ResumePlayer();
				}
				else if (x == "next")
				{
					player.DropNext();
				}
				else if (x == "prev")
				{
					player.DropNext();
				}
				else if (x == "total")
				{
					Console.WriteLine(player.TotalTime);
				}
				else if (x == "current")
				{
					Console.WriteLine(player.CurrentEntity.Name);
				}
				else if (x == "rekick")
				{
					player.SetInstance(t1);
				}
				else if(x =="q")
				{
					break;
				}
			}
		}
	}	
}