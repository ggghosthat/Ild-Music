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

        public TrackViewModel()
    	{    		
            BackCommand = new(BackSwap, null);
    	}

        private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];

    	public Track TrackInstance {get; private set;}
        public byte[] AvatarSource => TrackInstance.AvatarSource.ToArray();
        
        public ObservableCollection<CommonInstanceDTO> TrackArtists {get; private set;} = new();
        public ObservableCollection<CommonInstanceDTO> TrackPlaylists {get; private set;} = new();

        public CommandDelegator BackCommand { get; }

        public async void SetInstance(CommonInstanceDTO instanceDto)
        {
            TrackInstance = await supporter.GetTrackAsync(instanceDto);
            OnPropertyChanged("AvatarSource");
            
            supporter.GetInstanceDTOsFromIds(TrackInstance.Artists, EntityTag.ARTIST)
                .Result.ToList().ForEach(a => TrackArtists.Add(a));

            supporter.GetInstanceDTOsFromIds(TrackInstance.Playlists, EntityTag.PLAYLIST)
                .Result.ToList().ForEach(p => TrackPlaylists.Add(p));
        }

        public async void SetInstance(Track track)
        {
            TrackInstance = track;
            OnPropertyChanged("AvatarSource");
            
            supporter.GetInstanceDTOsFromIds(TrackInstance.Artists, EntityTag.ARTIST)
                .Result.ToList().ForEach(a => TrackArtists.Add(a));

            supporter.GetInstanceDTOsFromIds(TrackInstance.Playlists, EntityTag.PLAYLIST)
                .Result.ToList().ForEach(p => TrackPlaylists.Add(p));
        }

        private void BackSwap(object obj)
        {
            TrackArtists.Clear();
            MainVM.ResolveWindowStack();            
        }
    }
}
