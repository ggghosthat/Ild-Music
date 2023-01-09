using Ild_Music;
using ShareInstances;
using ShareInstances.Stage;
using ShareInstances.Services.Center;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ild_Music.Models
{
	public class Repository
	{
		private static Stage stage => App.Stage;
		public IList<IPlayer> Players {get; private set;} = new List<IPlayer>();
		public IList<ISynchArea> Areas {get; private set;} = new List<ISynchArea>();

		public IPlayer Player {get; private set;}
		public ISynchArea Area {get; private set;}

		public Repository()
		{
			UpdateState();
		}

		public void UpdateState()
		{
			stage.Players.ToList().ForEach(p => Players.Add(p));
			stage.Areas.ToList().ForEach(a => Areas.Add(a));
		}

		public async Task SetComponents(IShare component) => 
			await App.Stage.ChangeComponentAsync(component);
	}
}