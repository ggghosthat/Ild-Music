using Ild_Music.Core.Contracts;
using Ild_Music.Core.Stage;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ild_Music.Models;
public class Repository
{
	private static Stage stage => App.Stage;
	public IList<IPlayer> Players {get; private set;} = new List<IPlayer>();
	public IList<ICube> Cubes {get; private set;} = new List<ICube>();

	public IPlayer Player {get; private set;}
	public ICube Cube {get; private set;}

	public Repository()
	{
		UpdateState();
	}

	public void UpdateState()
	{
		stage.GetPlayers().ToList().ForEach(p => Players.Add(p));
		stage.GetCubes().ToList().ForEach(c => Cubes.Add(c));
	}

	public async Task SetComponents(IShare component) => 
		await App.Stage.ChangeComponentAsync(component);
}
