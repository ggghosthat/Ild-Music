using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class TrackViewModel : BaseViewModel
    {
        public static readonly string nameVM = "TrackVM";   

        #region Services
        private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
    	public Track TrackInstance {get; private set;}
        public byte[] AvatarSource => TrackInstance.GetAvatar();
        
        public ObservableCollection<CommonInstanceDTO> TrackArtists {get; private set;} = new();
        #endregion

        #region Commands
        public CommandDelegator BackCommand { get; }
        #endregion

    	#region Ctor
    	public TrackViewModel()
    	{    		
            BackCommand = new(BackSwap, null);
    	}
    	#endregion

        #region Public Methods
        public async void SetInstance(Track track)
        {
            TrackInstance = track;
            OnPropertyChanged("AvatarSource");
            
            var trackArtists = await supporter.RequireInstances(EntityTag.ARTIST, 
                                                                TrackInstance.Artists);

            trackArtists.ToList()
                        .ForEach(a => TrackArtists.Add(a));
        }
        #endregion

        #region Provate Methods
        private void BackSwap(object obj)
        {
            TrackArtists.Clear();
            MainVM.ResolveWindowStack();            
        }
        #endregion
    }
}
