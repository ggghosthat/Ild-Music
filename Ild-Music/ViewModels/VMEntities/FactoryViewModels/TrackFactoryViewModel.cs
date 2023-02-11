using ShareInstances;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class TrackFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "TrackFactoryVM";
        public override string NameVM => nameVM;


        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Commands
        public CommandDelegator DefinePath { get; set; }
        public CommandDelegator CreateTrackCommand { get; }

        public CommandDelegator TrackArtistExplorerCommand {get;}
        #endregion

        #region Track Factory Proeprties
        public string TrackPath { get; set; }
        public string TracktName { get; set; }
        public string TrackDescription { get; set; }

        public Artist CurrentSelectedTrackArtist { get; set; }
        public Artist CurrentDeleteTrackArtist { get; set; }

        public static ObservableCollection<Artist> SelectedTrackArtists { get; set; } = new();
        #endregion

        #region Providers        
        public static ObservableCollection<Artist> ArtistProvider { get; set; } = new();
        #endregion

        #region Log Reply Properties        
        public string TrackLogLine { get; set; }
        public bool TrackLogError { get; set; }
        #endregion

        #region Properties
        public bool IsEditMode = false;
        #endregion

        #region Const
        public TrackFactoryViewModel()
        {
            DefinePath = new(DefineTrackPath, null);
            CreateTrackCommand = new(CreateTrack, null);
            TrackArtistExplorerCommand = new(OpenTrackArtistExplorer, null);
            supporterService.OnArtistsNotifyRefresh += ArtistProviderUpdate;
            Task.Run(InitArtists);
        }
        #endregion

        #region Private Methods
        private void OpenExplorer()
        {
            var explorer = (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
            
            MainVM.PushVM(this, explorer);
            MainVM.ResolveWindowStack();
        }

        private void ExitFactory()
        {
            FieldsClear();
            MainVM.ResolveWindowStack();
        }

        private async Task InitArtists()
        {
            ArtistProvider.ToList().Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));
        }

        private void ArtistProviderUpdate()
        {
            ArtistProvider.Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));        
        }

        private async Task FieldsClear()
        {
            TrackPath = default;
            TracktName = default;
            TrackDescription = default;
            SelectedTrackArtists.Clear();
            TrackLogLine = default;
        }
        #endregion

        #region Public Methods
        public void CreateTrackInstance(object[] values)
        {
            try
            {
                var path = (string)values[0];
                var name = (string)values[1];
                var description = (string)values[2];
                var artists = (IList<Artist>)values[3];

                if (!string.IsNullOrEmpty(name))
                {
                    factoryService.CreateTrack(path, name, description, artists);
                    TrackLogLine = "Successfully created!";
                
                    ExitFactory();
                }
            }
            catch(InvalidTrackException ex)
            {
                TrackLogLine = ex.Message;
            }
        }

        public void EditTrackInstance(object[] values)
        {
            try
            {
                var path = (string)values[0];
                var name = (string)values[1];
                var description = (string)values[2];
                var artists = (IList<Artist>)values[3];

                if (!string.IsNullOrEmpty(name))
                {
                    var editTrack = (Track)Instance;
                    editTrack.Pathway = path;
                    editTrack.Name = name;
                    editTrack.Description = description;

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeleteTrack(editTrack));
                        artists.ToList().ForEach(a => a.AddTrack(editTrack));
                    }

                    supporterService.EditInstance(editTrack);
                    supporterService.DumpState(); 

                    IsEditMode = false;
                    ExitFactory();   
                }
            }
            catch(InvalidTrackException ex)
            {
                TrackLogLine = ex.Message;
            }
        }

        public void DropInstance(ICoreEntity entity) 
        {
            Instance = entity;
            IsEditMode = true;
            if (entity is Track track)
            {
                TrackPath = track.Pathway;
                TracktName = track.Name;
                TrackDescription = track.Description;

                supporterService.ArtistsCollection.Where(a => a.Tracks.ToEntity(supporterService.TracksCollection).Contains(track))
                                                  .ToList()
                                                  .ForEach(a => SelectedTrackArtists.Add(a));
            }
        }
        #endregion

        #region Command Methods
        private async void DefineTrackPath(object obj)
        {
            OpenFileDialog dialog = new();
            string[] result = await dialog.ShowAsync(new Window());
            TrackPath = string.Join(" ", result);
        }

        private void CreateTrack(object obj)
        {
            object[] value = { TrackPath, TracktName, TrackDescription, SelectedTrackArtists };

            if (IsEditMode == false)
                CreateTrackInstance(value);
            else
                EditTrackInstance(value);
        }
        
        private void OnItemsSelected()
        {
            if(ExplorerVM.Output.Count > 0)
            {
                if (ExplorerVM.Output[0] is Artist)
                {
                    SelectedTrackArtists.Clear();
                    ExplorerVM.Output.ToList().ForEach(i => SelectedTrackArtists.Add((Artist)i)); 
                }
            }
        }


        private void OpenTrackArtistExplorer(object obj)
        {
           
            if (obj is IList<ICoreEntity> preSelected)
            {
                ExplorerVM.Arrange(0, preSelected); 
            }
            else
            {
                ExplorerVM.Arrange(0); 
            }
            ExplorerVM.OnSelected += OnItemsSelected;

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
        }
        #endregion
    }
}


