using Ild_Music.ViewModels.Base;
using Ild_Music.Models;
using Ild_Music.Command;
using Ild_Music.Core.Contracts;

using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
	public class SettingViewModel : BaseViewModel
	{
        public static readonly string nameVM = "SettingVM";        
        public override string NameVM => nameVM;
        
		#region Models
		public static Repository Repository {get; set;} = new();
		#endregion

		#region Properties
		public ObservableCollection<IPlayer> Players {get; set;}  = new();
		public ObservableCollection<ICube> Cubes {get; set;} = new();
		public IPlayer Player {get; set;}
		public ICube Cube {get; set;}
		#endregion

		#region Commands
        public CommandDelegator ApplyCommand { get; }
		#endregion

		#region Ctor
		public SettingViewModel()
		{
			ApplyCommand = new(Apply, null);
			Task.Run(CheckModel);
		}
		#endregion

		#region Private Methods
		private async void CheckModel()
		{
			Players.Clear();
			Cubes.Clear();

			await Task.Run(() => 
			{
				Repository.Players.ToList().ForEach(p => Players.Add(p));
				Repository.Cubes.ToList().ForEach(p => Cubes.Add(p)); 
			});

			Player = Repository.Player;
			Cube = Repository.Cube;
		}
		#endregion

		#region Command Methods
		private void Apply(object obj)
		{
            var playerId = Players.IndexOf(Player);
            var cubeId = Cubes.IndexOf(Cube);
			Task.Run(() => 
			{
				Repository.SwitchPlayerComponent(playerId);
				Repository.SwitchCubeComponent(cubeId);
			});
		}
		#endregion 
	}
}
