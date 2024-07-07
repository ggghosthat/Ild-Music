using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Selection;

namespace Ild_Music.ViewModels;

public class ListViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;
    
    public ListViewModel()
    {
        AddCommand = new(Add, null);
        DeleteCommand = new(Delete, null);
        EditCommand = new(Edit, null);
        BackCommand = new(Back, null);
        ItemSelectCommand = new(ItemSelect, null);
        InitCurrentListCommand = new(InitCurrentList, null);

        Task.Run(async () => await DisplayProvidersAsync());

        supporter.OnArtistsNotifyRefresh += () => UpdateProvider(EntityTag.ARTIST);
        supporter.OnPlaylistsNotifyRefresh += () => UpdateProvider(EntityTag.PLAYLIST);
        supporter.OnTracksNotifyRefresh += () => UpdateProvider(EntityTag.TRACK);
        supporter.OnTagsNotifyRefresh += () => UpdateProvider(EntityTag.TAG);
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public CommandDelegator AddCommand { get; }
    public CommandDelegator DeleteCommand { get; }
    public CommandDelegator EditCommand { get; }
    public CommandDelegator BackCommand { get; }
    public CommandDelegator ItemSelectCommand { get; }
    public CommandDelegator InitCurrentListCommand { get; }

    // public ListType ListType {get; private set;}
    public static ObservableCollection<string> Headers { get; private set; } = new() {"Artists","Playlists","Tracks", "Tags"};
    public string Header { get; set; } = Headers[0];
    public static ObservableCollection<CommonInstanceDTO> CurrentList { get; set; } = new();
    public CommonInstanceDTO? CurrentItem { get; set; } = null;
    
    private void InitCurrentList(object obj)
    {
        DisplayProvidersAsync().Wait();
    }

    public void Back(object obj)
    {        
        MainVM.ResolveWindowStack();
    }

    public async Task UpdateProviders()
    {
        await DisplayProvidersAsync();
    }

    private void DisplayProviders()
    {
        CurrentList.Clear();

        switch(Header)
        {
            case "Artists":
                supporter.ArtistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Playlists":
                supporter.PlaylistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Tracks":
                supporter.TracksCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Tags":
                supporter.TagsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
        }
    }

    private async Task DisplayProvidersAsync()
    {
        CurrentList.Clear();

        switch(Header)
        {
            case "Artists":
                supporter.ArtistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Playlists":
                supporter.PlaylistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Tracks":
                supporter.TracksCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case "Tags":
                supporter.TagsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
        }

        OnPropertyChanged("CurrentList");
    }

    private async void UpdateProvider(EntityTag tag)
    {
        CurrentList.Clear();

        switch(tag)
        {
            case EntityTag.ARTIST:
                supporter.ArtistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case EntityTag.PLAYLIST:
                supporter.PlaylistsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case EntityTag.TRACK:
                supporter.TracksCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
            case EntityTag.TAG:
                supporter.TagsCollection.ToList().ForEach(i => CurrentList.Add(i));
                break;
        }
    }

    public async Task BrowseTracks(IEnumerable<string> paths)
    {
        if(Header is "Tracks")
        {
            paths.ToList().ForEach(path => factory.CreateTrack(path));
            await UpdateProviders();
        }
    }

    private void Add(object obj)
    {
        BaseViewModel editor = Header switch
        {
            "Artists" => (BaseViewModel)App.ViewModelTable[ArtistEditorViewModel.viewModelId],
            "Playlists" => (BaseViewModel)App.ViewModelTable[PlaylistEditorViewModel.viewModelId],
            "Tracks" =>  (BaseViewModel)App.ViewModelTable[TrackEditorViewModel.viewModelId],
            "Tags" => (BaseViewModel)App.ViewModelTable[TagEditorViewModel.viewModelId],
            _ => null
        };

        if (editor is null)
            return;

        MainVM.PushVM(this, editor);
        MainVM.ResolveWindowStack();
    }

    private void Delete(object obj) 
    {
        if(CurrentItem is null)
             return;

        var id = (Guid)CurrentItem?.Id;
        switch (Header)
        {
            case "Artists":
                supporter.DeleteArtistInstance(id);
                break;
            case "Playlists": 
                supporter.DeletePlaylistInstance(id);
                break;
            case "Tracks":
                supporter.DeleteTrackInstance(id);
                break;
            case "Tags":
                supporter.DeleteTagInstance(id);
                break;
        };
        
        DisplayProvidersAsync().Wait();
    }
    
    private void Edit(object obj)
    {        
        BaseViewModel editor = null;

        switch(Header)
        {
            case "Artists":
                var artistEditor = (ArtistEditorViewModel)App.ViewModelTable[ArtistEditorViewModel.viewModelId];
                artistEditor?.DropInstance(CurrentItem ?? default).Wait();
                editor = artistEditor;
                break;
            case "Playlists": 
                var playlistEditor = (PlaylistEditorViewModel)App.ViewModelTable[PlaylistEditorViewModel.viewModelId];
                playlistEditor?.DropInstance(CurrentItem ?? default!);
                editor = playlistEditor;
                break;
            case "Tracks":
                var trackEditor = (TrackEditorViewModel)App.ViewModelTable[TrackEditorViewModel.viewModelId];
                trackEditor?.DropInstance(CurrentItem ?? default!);
                editor = trackEditor;
                break;
            case "Tags":
                var tagEditor = (TagEditorViewModel)App.ViewModelTable[TagEditorViewModel.viewModelId];
                tagEditor?.DropInstance(CurrentItem ?? default!);
                editor = tagEditor;
                break;
            default:
                return;
        }

        MainVM.PushVM(this, editor);
        MainVM.ResolveWindowStack();
    }

    private async void ItemSelect(object obj)
    {
        if(CurrentItem is not null)
        {
            var currentItem = CurrentItem ?? default!; 

            if(Header.Equals("Artists") || Header.Equals("Tags"))
                MainVM.ResolveInstance(this, currentItem);
            else if(Header.Equals("Playlists"))
                PassPlaylistEntity(currentItem);
            else if(Header.Equals("Tracks"))
                PassTrackEntity(currentItem);
        }
    }

    private void PassPlaylistEntity(CommonInstanceDTO playlistDto)
    {
        var mainPlaylistId = MainVM.CurrentPlaylist?.Id ?? Guid.Empty;

        if(mainPlaylistId.Equals(playlistDto.Id))
        {
            Task.Run(() => MainVM.ResolveInstance(this, playlistDto));
        }
        else
        {
            var playlist = supporter.GetPlaylistAsync(playlistDto).Result;
            MainVM.DropPlaylistInstance(this, playlist, false);
        }
    }

    private void PassTrackEntity(CommonInstanceDTO trackDto)
    {
        var mainPlaylistId = MainVM.CurrentTrack?.Id ?? Guid.Empty;
        if(mainPlaylistId.Equals(trackDto.Id))
        {
            Task.Run(() => MainVM.ResolveInstance(this, trackDto));
        }
        else
        {
            var track = supporter.GetTrackAsync(trackDto).Result;
            MainVM.DropTrackInstance(this, track, false);
        }
    }
}
