using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class ArtistViewModel : BaseViewModel
    {
        public static readonly string nameVM = "ArtistVM";   

        #region Services
        private SupporterService supporter => (SupporterService)base.GetService("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
    	public Artist ArtistInstance {get; private set;}
        public ObservableCollection<Playlist> ArtistPlaylists {get; private set;} = new();
        public ObservableCollection<Track> ArtistTracks {get; private set;} = new();      
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
        public void SetInstance(Artist artist)
        {
            ArtistInstance = artist;

            //Crutcal point
            ArtistInstance.Playlists.ToEntity(supporter.PlaylistsCollection).ForEach(p => ArtistPlaylists.Add(p));
            ArtistInstance.Tracks.ToEntity(supporter.TracksCollection).ToList().ForEach(t => ArtistTracks.Add(t));
        }
        #endregion

        #region Provate Methods
        private void BackSwap(object obj)
        {
            MainVM.ResolveWindowStack();
        }
        #endregion
    }
}