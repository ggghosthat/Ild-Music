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
        private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
        #endregion

        #region Properties
    	public Track TrackInstance {get; private set;}
        public byte[] AvatarSource => TrackInstance.AvatarSource.ToArray();
        
        public ObservableCollection<CommonInstanceDTO> TrackArtists {get; private set;} = new();
        public ObservableCollection<CommonInstanceDTO> TrackPlaylists {get; private set;} = new();
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
        public async void SetInstance(CommonInstanceDTO instanceDto)
        {
            TrackInstance = await supporter.GetTrackAsync(instanceDto);
            OnPropertyChanged("AvatarSource");
            
            supporter.GetInstanceDTOsFromIds(TrackInstance.Artists, EntityTag.ARTIST)
                .Result
                .ToList()
                .ForEach(a => TrackArtists.Add(a));

            supporter.GetInstanceDTOsFromIds(TrackInstance.Playlists, EntityTag.PLAYLIST)
                .Result
                .ToList()
                .ForEach(p => TrackPlaylists.Add(p));
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
