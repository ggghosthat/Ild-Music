using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
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
    public class PlaylistViewModel : BaseViewModel
    {
        public static readonly string nameVM = "PlaylistVM";   

        #region Services
        private SupporterService supporter => (SupporterService)base.GetService("SupporterService");
        private StoreService store => (StoreService)base.GetService("StoreService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
    	public Playlist PlaylistInstance {get; private set;}
        public ObservableCollection<Artist> PlaylistArtists {get; private set;} = new();
        public ObservableCollection<Track> PlaylistTracks {get; private set;} = new();      
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
        public void SetInstance(Playlist playlist)
        {
            PlaylistInstance = playlist;


            supporter.ArtistsCollection.Where(a => a.Playlists.Contains(PlaylistInstance.Id))
                                                  .ToList()
                                                  .ForEach(a => PlaylistArtists.Add(a));

            store.StoreInstance.GetTracksById(playlist.Tracks)
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