using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System.Linq;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class PlaylistViewModel : BaseViewModel
    {
        public static readonly string nameVM = "PlaylistVM";   

        #region Services
        private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
        #endregion

        #region Properties
    	public Playlist PlaylistInstance {get; private set;}
        public byte[] AvatarSource => PlaylistInstance.GetAvatar();

        public ObservableCollection<CommonInstanceDTO> PlaylistArtists {get; private set;} = new();
        public ObservableCollection<CommonInstanceDTO> PlaylistTracks {get; private set;} = new();      
        #endregion

        #region Commands
        public CommandDelegator BackCommand { get; }
        #endregion

    	#region Ctor
    	public PlaylistViewModel()
    	{
            BackCommand = new(BackSwap, null);
    	}
    	#endregion

        #region Public Methods
        public async void SetInstance(CommonInstanceDTO instanceDTO)
        {
            PlaylistInstance = await supporter.GetPlaylistAsync(instanceDTO);
            OnPropertyChanged("AvatarSource");

            supporter.GetInstanceDTOsFromIds(PlaylistInstance.Artists, EntityTag.ARTIST)
                .Result
                .ToList()
                .ForEach(p => PlaylistArtists.Add(p));

            supporter.GetInstanceDTOsFromIds(PlaylistInstance.Tracky, EntityTag.TRACK)
                .Result
                .ToList()
                .ForEach(t => PlaylistTracks.Add(t)); 
        }
        #endregion

        #region Provate Methods
        private void BackSwap(object obj)
        {
            PlaylistArtists.Clear();
            PlaylistTracks.Clear();
            MainVM.ResolveWindowStack();
        }
        #endregion
    }
}
