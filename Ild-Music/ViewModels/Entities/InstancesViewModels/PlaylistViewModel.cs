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
        private SupportGhost supporter => (SupportGhost)base.GetService(Ghosts.SUPPORT);
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
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
        public async void SetInstance(Playlist playlist)
        {
            PlaylistInstance = playlist;
            OnPropertyChanged("AvatarSource");

            var artistPlaylists = await supporter.RequireInstances(EntityTag.PLAYLIST,
                                                                   PlaylistInstance.Artists);
            artistPlaylists.ToList()
                           .ForEach(p => PlaylistArtists.Add(p));

            var artistTracks = await supporter.RequireInstances(EntityTag.TRACK,
                                                                PlaylistInstance.Tracky);
            artistTracks.ToList()
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
