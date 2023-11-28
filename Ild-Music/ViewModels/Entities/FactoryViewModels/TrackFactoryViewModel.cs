using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Exceptions.SynchAreaExceptions;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;

namespace Ild_Music.ViewModels
{
    public class TrackFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "TrackFactoryVM";
        public override string NameVM => nameVM;


        #region Services
        private static FactoryGhost factoryService => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
        private static SupportGhost supporterService => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public Track Instance { get; private set; }
        #endregion

        #region Avatar Avatar
        public byte[] AvatarSource {get; private set;}
        #endregion

        #region Commands
        public CommandDelegator SelectAvatarCommand { get; }
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
            SelectAvatarCommand = new(SelectAvatar, null);
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
                var year = (int)values[3];
                var avatar = (byte[])values[4];
                var artists = (IList<Artist>)values[5];

                if (!string.IsNullOrEmpty(path))
                {
                    factoryService.CreateTrack(path,
                                               name,
                                               description,
                                               year,
                                               avatar,
                                               artists);
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
                var avatar = (byte[])values[3];
                var artists = (IList<Artist>)values[4];

                if (!string.IsNullOrEmpty(path))
                {
                    var editTrack = (Track)Instance;
                    editTrack.Pathway = path.AsMemory();
                    editTrack.Name = name.AsMemory();
                    editTrack.Description = description.AsMemory();
                    editTrack.AvatarSource = (avatar is not null)?avatar:null;

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeleteTrack(ref editTrack));
                        artists.ToList().ForEach(a => a.AddTrack(ref editTrack));
                    }

                    supporterService.EditTrackInstance(editTrack);

                    IsEditMode = false;
                    ExitFactory();   
                }
            }
            catch(InvalidTrackException ex)
            {
                TrackLogLine = ex.Message;
            }
        }

        public async void DropInstance(Guid trackId) 
        {
            Instance = await supporterService.FetchTrack(trackId);
            IsEditMode = true;
            TrackPath = Instance.Pathway.ToString();
            TracktName = Instance.Name.ToString();
            TrackDescription = Instance.Description.ToString();
            AvatarSource = Instance.GetAvatar();

            supporterService.ArtistsCollection
                            .Where(a => Instance.Artists.Contains(a.Id))
                            .ToList()
                            .ForEach(a => SelectedTrackArtists.Add(a));
        }

        private void OnItemsSelected()
        {
            if(ExplorerVM.Output.Count > 0)
            {
                if (ExplorerVM.Output[0] is Artist)
                {
                    SelectedTrackArtists.Clear();
                    var outIds = ExplorerVM.Output.Select(o => o.Id);
                                     
                    supporterService.ArtistsCollection
                                    .Where(a => outIds.Contains(a.Id))
                                    .ToList()
                                    .ForEach(i => SelectedTrackArtists.Add(i));
                }
            }
        }

        private void OpenTrackArtistExplorer(object obj)
        {
            ExplorerVM.OnSelected += OnItemsSelected;
            if (obj is IList<Artist> preSelected)
            {
                ExplorerVM.Arrange(EntityTag.ARTIST,
                                   preSelected.ToCommonDTO()); 
            }
            else
            {
                ExplorerVM.Arrange(EntityTag.ARTIST); 
            }
            ExplorerVM.OnSelected += OnItemsSelected;

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
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
        
        private void Cancel(object obj)
        {
            ExitFactory();
        }

        private async void SelectAvatar(object obj)
        {
            //AvatarSource = await LoadAvatar(avatarPath);
        }
        #endregion
    }
}


