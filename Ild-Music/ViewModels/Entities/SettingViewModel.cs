using Ild_Music.ViewModels.Base;
using Ild_Music.Models;
using Ild_Music.Command;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Stage;
using Ild_Music.Core.Services;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Services.Castle;

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
		public ObservableCollection<ICube> Areas {get; set;} = new();
		public IPlayer Player {get; set;}
		public ICube Area {get; set;}
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
			Areas.Clear();

			await Task.Run(() => 
			{
				Repository.Players.ToList().ForEach(p => Players.Add(p));
				Repository.Cubes.ToList().ForEach(p => Areas.Add(p)); 
			});

			Player = Repository.Player;
			Area = Repository.Cube;
		}
		#endregion

		#region Command Methods
		private void Apply(object obj)
		{
			Task.Run(async () => 
			{
				await Repository.SetComponents(Player);
				await Repository.SetComponents(Area);
			});
		}
		#endregion 
	}
}
