using Ild_Music.Core.Contracts;
using Ild_Music.Core.Stage;

using System.Linq;
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

	public void SwitchPlayerComponent(int index) => 
		App.Stage.SwitchPlayer(index);

    public void SwitchCubeComponent(int index) => 
		App.Stage.SwitchCube(index);

}
