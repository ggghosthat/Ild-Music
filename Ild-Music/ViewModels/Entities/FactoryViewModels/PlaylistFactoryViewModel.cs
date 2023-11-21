using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Exceptions.SynchAreaExceptions;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;

using System;
using System.IO;
using Avalonia.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class PlaylistFactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "PlaylistFactoryVM";
        public override string NameVM => nameVM;

        #region Services
        private FactoryGhost factoryService => (FactoryGhost)base.GetService(Ghosts.FACTORY);
        private SupportGhost supporterService => (SupportGhost)base.GetService(Ghosts.SUPPORT);
        private StoreService store => (StoreService)base.GetService("StoreService");
        private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
        private InstanceExplorerViewModel ExplorerVM => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
        #endregion

        #region Instance
        public Playlist Instance { get; private set; } = default!;
        #endregion

        #region Avatar Avatar
        public byte[] AvatarSource {get; private set;} = default!;
        public string AvatarRaw {get; private set;} = default!;
        #endregion

        #region Commands
        public CommandDelegator CreatePlaylistCommand { get; }
        public CommandDelegator CancelCommand { get; }

        public CommandDelegator SelectAvatarCommand { get; }

        public CommandDelegator SelectPlaylistArtistCommand { get; }
        public CommandDelegator DeletePlaylistArtistCommand { get; }

        public CommandDelegator SelectPlaylistTrackCommand { get; }
        public CommandDelegator DeletePlaylistTrackCommand { get; }

        public CommandDelegator PlaylistArtistExplorerCommand {get;}
        public CommandDelegator PlaylistTrackExplorerCommand {get;}
        #endregion

        #region Playlist Factory Properties
        public string PlaylistName { get; set; } = default!;
        public string PlaylistDescription { get; set; } = default!;

        public static ObservableCollection<Artist> SelectedPlaylistArtists {get;set;} = new();
        public static ObservableCollection<Track> SelectedPlaylistTracks {get; set;} = new();
        #endregion

        #region Providers        
        public static ObservableCollection<Artist> ArtistProvider { get; set; } = new();
        public static ObservableCollection<Track> TrackProvider { get; set; } = new();
        #endregion
    
        #region Log Reply Properties
        public string PlaylistLogLine { get; set; }
        public bool PlaylistLogError { get; set; }
        #endregion

        #region Properties
        public bool IsEditMode {get; private set;} = false;
        public string ViewHeader {get; private set;} = "Playlist";
        #endregion

        #region Const
        public PlaylistFactoryViewModel()
        {
            CreatePlaylistCommand = new(CreatePlaylist, null);
            CancelCommand = new(Cancel, null);

            SelectAvatarCommand = new (SelectAvatar, null);

            PlaylistArtistExplorerCommand = new(OpenPlaylistArtistExplorer, null);
            PlaylistTrackExplorerCommand = new(OpenPlaylistTrackExplorer, null);

            supporterService.OnArtistsNotifyRefresh += ArtistProviderUpdate;
            supporterService.OnTracksNotifyRefresh += TrackProviderUpdate;
           
            Task.Run(InitArtists);
            Task.Run(InitTracks); 
        }
        #endregion

        #region Private Methods
        private async void ExitFactory()
        {
            await FieldsClear();
            MainVM.ResolveWindowStack();
        }

        private async Task InitArtists()
        {
            ArtistProvider.ToList().Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));
        }
        
        private async Task InitTracks()
        {
            TrackProvider.ToList().Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));
        }

        private void ArtistProviderUpdate()
        {
            ArtistProvider.Clear();
            supporterService.ArtistsCollection
                            .ToList().ForEach(artist => ArtistProvider.Add(artist));            
        }

        private void TrackProviderUpdate()
        {
            TrackProvider.Clear();
            supporterService.TracksCollection
                            .ToList().ForEach(track => TrackProvider.Add(track));            
        }

        private async Task FieldsClear()
        {
            PlaylistName = default;
            PlaylistDescription = default;  
            SelectedPlaylistArtists.Clear();
            SelectedPlaylistTracks.Clear();
            PlaylistLogLine = default;
            AvatarSource = default;
        }

        private void OnItemsSelected()
        {
            if(ExplorerVM.Output.Count > 0)
            {
                if (ExplorerVM.Output[0] is Artist)
                {
                    SelectedPlaylistArtists.Clear();
                    ExplorerVM.Output.ToList().ForEach(i => SelectedPlaylistArtists.Add((Artist)i)); 
                }
                else if(ExplorerVM.Output[0] is Track)
                {
                    SelectedPlaylistTracks.Clear();
                    ExplorerVM.Output.ToList().ForEach(i => SelectedPlaylistTracks.Add((Track)i)); 
                }
            }
        }

        private void OpenPlaylistArtistExplorer(object obj)
        {
            ExplorerVM.OnSelected += OnItemsSelected;
            if (obj is IList<Artist> preSelected)
            {
                ExplorerVM.Arrange(0, preSelected); 
            }
            else
            {
                ExplorerVM.Arrange(0); 
            }

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
        }

        private void OpenPlaylistTrackExplorer(object obj)
        {
            ExplorerVM.OnSelected += OnItemsSelected;
            if (obj is IList<Track> preSelected)
            {
                ExplorerVM.Arrange(2, preSelected); 
            }
            else
            {
                ExplorerVM.Arrange(2); 
            }

            MainVM.PushVM(this, ExplorerVM);
            MainVM.ResolveWindowStack();
        }

        private async Task<byte[]> LoadAvatar(string path)
        {
            byte[] result;

            using (FileStream fileStream = File.Open(path, FileMode.Open))
            {
                result = new byte[fileStream.Length];
                await fileStream.ReadAsync(result, 0, (int)fileStream.Length);
            }
            return result;
        }
        #endregion

        #region Public Methods
        public void CreatePlaylistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var year = (int)values[2];
                var avatar = (byte[])values[3];
                var tracks = (IList<Track>)values[4];
                var artists = (IList<Artist>)values[5];

                if (!string.IsNullOrEmpty(name))
                {
                    var avatarBase64 = (avatar is not null)?Convert.ToBase64String(avatar):null;
                    factoryService.CreatePlaylist(name, description, year, avatar, tracks, artists);
                    PlaylistLogLine = "Successfully created!";
                    ExitFactory(); 
                }
            }
            catch (InvalidPlaylistException ex)
            {
                PlaylistLogLine = ex.Message;
            }
        }
 
        public void EditPlaylistInstance(object[] values)
        {
            try
            {
                var name = (string)values[0];
                var description = (string)values[1];
                var avatar = (byte[])values[2];
                var tracks = (IList<Track>)values[3];
                var artists = (IList<Artist>)values[4];

                if(!string.IsNullOrEmpty(name))
                {
                    var editPlaylist = (Playlist)Instance; 
                    editPlaylist.Name = name.AsMemory();
                    editPlaylist.Description = description.AsMemory();
                    editPlaylist.AvatarSource = (avatar is not null)? avatar:null;

                    if(tracks != null && tracks.Count > 0)
                    {
                        editPlaylist.EraseTracks();
                        tracks.ToList().ForEach(t => editPlaylist.AddTrack(ref t));
                    }

                    if(artists != null && artists.Count > 0)
                    {
                        var clear_artists = ArtistProvider.ToList().Except(artists);
                        clear_artists.ToList().ForEach(a => a.DeletePlaylist(ref editPlaylist));
                        artists.ToList().ForEach(a => a.AddPlaylist(ref editPlaylist));
                    }

                    supporterService.EditPlaylistInstance(editPlaylist);

                    IsEditMode = false;
                    ExitFactory();
                }
            }
            catch (InvalidPlaylistException ex)
            {
                PlaylistLogLine = ex.Message;
            }
        }

        public void DropInstance(Playlist playlist) 
        {
            Instance = playlist;
            IsEditMode = true;
            PlaylistName = playlist.Name.ToString();
            PlaylistDescription = playlist.Description.ToString();
            AvatarSource = playlist.GetAvatar();

            supporterService.ArtistsCollection.Where(a => a.Playlists.ToEntity(supporterService.PlaylistsCollection)
                                                                    .Contains(playlist))
                                            .ToList()
                                            .ForEach(a => SelectedPlaylistArtists.Add(a));
                
            store.StoreInstance.GetTracksById(playlist.GetTracks())
                            .ToList()
                            .ForEach(t => SelectedPlaylistTracks.Add(t));
        }
        #endregion


        #region Command Methods
        private void Cancel(object obj)
        {
            ExitFactory();
        }


        private void CreatePlaylist(object obj)
        {
            object[] values = { PlaylistName, PlaylistDescription, AvatarSource, SelectedPlaylistTracks, SelectedPlaylistArtists };

            if (IsEditMode == false)
                CreatePlaylistInstance(values);
            else
                EditPlaylistInstance(values);
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
