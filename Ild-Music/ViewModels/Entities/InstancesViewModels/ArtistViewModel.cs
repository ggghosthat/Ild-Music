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
    public class ArtistViewModel : BaseViewModel
    {
        public static readonly string nameVM = "ArtistVM";   

        #region Services
        private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
    	public Artist ArtistInstance {get; private set;}
        public byte[] AvatarSource => ArtistInstance.GetAvatar();

        public ObservableCollection<CommonInstanceDTO> ArtistPlaylists {get; private set;} = new();
        public ObservableCollection<CommonInstanceDTO> ArtistTracks {get; private set;} = new();      
        #endregion
        
        #region Commands
        public CommandDelegator BackCommand { get; }
        #endregion

    	#region Ctor
    	public ArtistViewModel()
    	{
            BackCommand = new(BackSwap, null);
    	}
    	#endregion

        #region Public Methods
        public async void SetInstance(Artist artist)
        {
            ArtistInstance = artist;
            OnPropertyChanged("AvatarSource");

            var artistPlaylists = await supporter.RequireInstances(EntityTag.PLAYLIST,
                                                                   ArtistInstance.Playlists);
            artistPlaylists.ToList()
                           .ForEach(p => ArtistPlaylists.Add(p));

            var artistTracks = await supporter.RequireInstances(EntityTag.TRACK,
                                                                ArtistInstance.Tracks);
            artistTracks.ToList()
                        .ForEach(t => ArtistTracks.Add(t));
        }
        #endregion

        #region Private Methods
        private void BackSwap(object obj)
        {
            ArtistPlaylists.Clear();
            ArtistTracks.Clear();
            MainVM.ResolveWindowStack();
        }
        #endregion
    }
}
