using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Exceptions.CubeExceptions;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Ild_Music.ViewModels;

public class PlaylistEditorViewModel : BaseViewModel
{
    public static readonly string nameVM = "PlaylistEditorVM";        
    public override string NameVM => nameVM;
    
    public PlaylistEditorViewModel()
    {
        CreatePlaylistCommand = new(HandleChanges, null);
        SelectAvatarCommand = new(SelectAvatar, null);
        CancelCommand = new(Cancel, null);
        PlaylistArtistExplorerCommand = new(OpenPlaylistArtistExplorer, null);
        PlaylistTrackExplorerCommand = new(OpenPlaylistTrackExplorer, null);
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
    private static InstanceExplorerViewModel Explorer => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.nameVM];
   
    public Playlist PlaylistInstance { get; private set; } = default!;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Year { get; set; } = default!;
    public byte[] Avatar { get; set; } = default!;

    public ObservableCollection<CommonInstanceDTO> ArtistsProvider {get;set;} = new();
    public ObservableCollection<CommonInstanceDTO> TracksProvider {get; set;} = new();
    public ObservableCollection<CommonInstanceDTO> SelectedPlaylistArtists {get;set;} = new();
    public ObservableCollection<CommonInstanceDTO> SelectedPlaylistTracks {get; set;} = new();

    public string PlaylistLogLine { get; set; }
    public bool PlaylistLogError { get; set; } 
    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Playlist";

    public CommandDelegator CreatePlaylistCommand { get; }
    public CommandDelegator CancelCommand { get; }
    public CommandDelegator SelectAvatarCommand { get; }
    public CommandDelegator PlaylistArtistExplorerCommand {get;}
    public CommandDelegator PlaylistTrackExplorerCommand {get;}

    private void ExitFactory()
    {
        FieldsClear();
        MainVM.ResolveWindowStack();
    }
    
    private async Task InitProviders()
    {
        ArtistsProvider.Clear();
        supporter?.ArtistsCollection?.ToList()
            .ForEach(artist => ArtistsProvider.Add(artist));
        
        TracksProvider.Clear();
        supporter?.TracksCollection?.ToList()
            .ForEach(track => TracksProvider.Add(track));
    }
        
    private void ArtistProviderUpdate()
    {
        ArtistsProvider.Clear();
        supporter?.ArtistsCollection?.ToList()
            .ForEach(artist => ArtistsProvider.Add(artist));            
    }

    private void TrackProviderUpdate()
    {
        TracksProvider.Clear();
        supporter?.TracksCollection?.ToList()
            .ForEach(track => TracksProvider.Add(track));            
    }

    private void FieldsClear()
    {
        Name = default;
        Description = default;  
        Avatar = default;
        PlaylistLogLine = default;
        SelectedPlaylistArtists.Clear();
        SelectedPlaylistTracks.Clear();
    }
    
    private void OnItemsSelected()
    {
        if(Explorer.Output.Count > 0)
        {
            if (Explorer.Output[0].Tag is EntityTag.ARTIST)
            {
                SelectedPlaylistArtists.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                     
                supporter?
                    .ArtistsCollection?
                    .Where(a => outIds.Contains(a.Id))
                    .ToList()
                    .ForEach(i => SelectedPlaylistArtists.Add(i));
            }
            else if(Explorer.Output[0].Tag is EntityTag.TRACK)
            {
                SelectedPlaylistTracks.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                     
                supporter?
                    .TracksCollection?
                    .Where(t => outIds.Contains(t.Id))
                    .ToList()
                    .ForEach(i => SelectedPlaylistTracks.Add(i));
            }
        }
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

    private void OpenPlaylistArtistExplorer(object obj)
    {
        Explorer.OnSelected += OnItemsSelected;
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.ARTIST) 
            Explorer.Arrange(EntityTag.ARTIST, preSelected);         
        else
            Explorer.Arrange(EntityTag.ARTIST); 

        MainVM.PushVM(this, Explorer);
        MainVM.ResolveWindowStack();
    }

    private void OpenPlaylistTrackExplorer(object obj)
    {
        Explorer.OnSelected += OnItemsSelected;
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.TRACK) 
            Explorer.Arrange(EntityTag.TRACK, preSelected);         
        else
            Explorer.Arrange(EntityTag.TRACK); 

        MainVM.PushVM(this, Explorer);
        MainVM.ResolveWindowStack();
    } 

    public void CreatePlaylistInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidPlaylistException();
            
            Playlist playlist = default;  
            var artists = SelectedPlaylistArtists.Select(a => supporter.GetArtistAsync(a).Result).ToList();
            var tracks = SelectedPlaylistTracks.Select(t => supporter.GetTrackAsync(t).Result).ToList();
            
            factory?.CreatePlaylist(Name, Description, Year, Avatar, tracks, artists, out playlist);
            PlaylistInstance = playlist;
            PlaylistLogLine = "Successfully created!";
            ExitFactory(); 
        }
        catch (InvalidPlaylistException ex)
        {
            PlaylistLogLine = ex.Message;
        }
    }
 
    public void EditPlaylistInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidPlaylistException();
            
            var editPlaylist = PlaylistInstance; 
            editPlaylist.Name = Name.AsMemory();
            editPlaylist.Description = Description.AsMemory();
            editPlaylist.Year = Year;
            editPlaylist.AvatarSource = (Avatar is not null)? Avatar:null;

            if(SelectedPlaylistTracks != null && SelectedPlaylistTracks.Count > 0)
            {
                editPlaylist.EraseTracks();
                SelectedPlaylistTracks.ToList()
                    .ForEach(async t =>
                    {
                        var trackInstance = await supporter.GetTrackAsync(t);
                        editPlaylist.AddTrack(ref trackInstance);
                    });
            }

            if(SelectedPlaylistArtists != null && SelectedPlaylistArtists.Count > 0)
            {
                //remove playlist from no-needed artists
                ArtistsProvider.ToList()
                    .Except(SelectedPlaylistArtists).ToList()
                    .ForEach(async a => 
                    {
                        var artistInstance = await supporter.GetArtistAsync(a);
                        artistInstance.DeletePlaylist(ref editPlaylist);
                    });
                //add playlist to needed artists
                SelectedPlaylistArtists.ToList()
                    .ForEach(async a =>
                    {
                        var artistInstance = await supporter.GetArtistAsync(a);
                        artistInstance.AddPlaylist(ref editPlaylist);
                    });
            }

            supporter?.EditPlaylistInstance(editPlaylist);

            IsEditMode = false;
            ExitFactory();
        }
        catch (InvalidPlaylistException ex)
        {
            PlaylistLogLine = ex.Message;
        }
    }

    public async void DropInstance(CommonInstanceDTO instanceDto) 
    {
        PlaylistInstance = await supporter.GetPlaylistAsync(instanceDto);
        IsEditMode = true;

        Name = PlaylistInstance.Name.ToString();
        Description = PlaylistInstance.Description.ToString();
        Year = PlaylistInstance.Year;
        Avatar = PlaylistInstance.GetAvatar();

        supporter?.ArtistsCollection?
            .Where(a => PlaylistInstance.Artists.Contains(a.Id))
            .ToList()
            .ForEach(a => SelectedPlaylistArtists.Add(a));
            
        supporter?.TracksCollection?
            .Where(t => PlaylistInstance.Tracky.Contains(t.Id))
            .ToList()
            .ForEach(a => SelectedPlaylistTracks.Add(a));
    }

    private void Cancel(object obj)
    {
        ExitFactory();
    }

    private void HandleChanges(object obj)
    {
        if (IsEditMode)
            EditPlaylistInstance();
        else
            CreatePlaylistInstance();
    }

    private static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

    public Window? FindWindowByViewModel(INotifyPropertyChanged viewModel) =>
        Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext));

    private async void SelectAvatar(object obj)
    {
        OpenFileDialog dialog = new();
        string[] result = await dialog.ShowAsync(FindWindowByViewModel(this));
        if(result != null && result.Length > 0)
        {
            var avatarPath = string.Join(" ", result);
            Avatar = await LoadAvatar(avatarPath);
            OnPropertyChanged("AvatarSource");
        }
    }
}
