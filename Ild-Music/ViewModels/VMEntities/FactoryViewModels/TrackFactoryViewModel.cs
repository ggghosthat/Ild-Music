using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Exceptions.SynchAreaExceptions;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Selection;
using TagLib;

namespace Ild_Music.ViewModels
{
    public class TrackFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "TrackFactoryVM";
        public override string NameVM => nameVM;


        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        private StoreService store => (StoreService)base.GetService("StoreService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public ICoreEntity Instance { get; private set; }
        #endregion

        #region Avatar Avatar
        public byte[] AvatarSource {get; private set;}
        public string AvatarRaw {get; private set;}
        #endregion

        #region Commands
        public CommandDelegator DefinePath { get; set; }
        public CommandDelegator CreateTrackCommand { get; }
        public CommandDelegator CancelCommand { get; }

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
        public bool IsEditMode {get; private set;} = false;
        public string ViewHeader {get; private set;} = "Track";
        #endregion

        #region Const
        public TrackFactoryViewModel()
        {
            DefinePath = new(DefineTrackPath, null);
            CreateTrackCommand = new(CreateTrack, null);
            CancelCommand = new(Cancel,null);

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
            AvatarSource = default;
        }

        private async Task<byte[]> LoadAvatar(string path)
        {
            byte[] result;

            using (FileStream fileStream = System.IO.File.Open(path, FileMode.Open))
            {
                result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
            }
            AvatarRaw = Convert.ToBase64String(result);
            return result;
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
                var avatar = (string)values[3];
                var artists = (IList<Artist>)values[4];

                if (!string.IsNullOrEmpty(path))
                {
                    factoryService.CreateTrack(path, name, description, avatar, artists);
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
                var avatar = (string)values[3];
                var artists = (IList<Artist>)values[4];

                if (!string.IsNullOrEmpty(path))
                {
                    var editTrack = (Track)Instance;
                    editTrack.Pathway = path;
                    editTrack.Name = name;
                    editTrack.Description = description;
                    editTrack.DefineAvatar(avatar);

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeleteTrack(editTrack.Id));
                        artists.ToList().ForEach(a => a.AddTrack(editTrack.Id));
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
                AvatarSource = track.GetAvatar();

                supporterService.ArtistsCollection.Where(a => a.Tracks.ToEntity(supporterService.TracksCollection).Contains(track))
                                                  .ToList()
                                                  .ForEach(a => SelectedTrackArtists.Add(a));
            }
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
        #endregion

        #region Command Methods
        private async void DefineTrackPath(object obj)
        {
            OpenFileDialog dialog = new();
            string[] result = await dialog.ShowAsync(new Window());
            if(result != null && result.Length > 0)
            {
                TrackPath = string.Join(" ", result);
                using( var taglib = TagLib.File.Create(TrackPath))
                TracktName = (!string.IsNullOrEmpty(taglib.Tag.Title))?taglib.Tag.Title:"Unknown";
            }
        }

        private void Cancel(object obj)
        {
            ExitFactory();
        }

        private void CreateTrack(object obj)
        {
            object[] value = { TrackPath, TracktName, TrackDescription, AvatarSource, SelectedTrackArtists };

            if (IsEditMode == false)
            {
                CreateTrackInstance(value);
            }
            else
            {
                EditTrackInstance(value);
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

        private async void SelectAvatar(object obj)
        {
            OpenFileDialog dialog = new();
            string[] result = await dialog.ShowAsync(new Window());
            if(result != null && result.Length > 0)
            {
                var avatarPath = string.Join(" ", result);
                AvatarSource = await LoadAvatar(avatarPath);
                OnPropertyChanged("AvatarSource");
            }
        }
        #endregion
    }
}


