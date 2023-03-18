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
    public class TrackViewModel : BaseViewModel
    {
        public static readonly string nameVM = "TrackVM";   

        #region Services
        private SupporterService supporter => (SupporterService)base.GetService("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        #endregion

        #region Properties
    	public Track TrackInstance {get; private set;}
        public ObservableCollection<Artist> TrackArtists {get; private set;} = new();
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
        public void SetInstance(Track track)
        {
            TrackInstance = track;

            supporter.ArtistsCollection.ToList().Where(a => a.Tracks.Contains(TrackInstance.Id))
                     .ToList().ForEach(a => TrackArtists.Add(a));
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