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
    public static readonly string nameVM = "ListVM";        
    public override string NameVM => nameVM;
    
    public ListViewModel()
    {
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];

    public CommandDelegator AddCommand { get; }
    public CommandDelegator DeleteCommand { get; }
    public CommandDelegator EditCommand { get; }
    public CommandDelegator BackCommand { get; }
    public CommandDelegator ItemSelectCommand { get; }
    public CommandDelegator DefineListTypeCommand { get; }

    // public ListType ListType {get; private set;}
    public ObservableCollection<string> Headers { get; private set; } = new() {"Artists","Playlists","Tracks"};
    public string Header { get; set; }
    public ObservableCollection<CommonInstanceDTO> CurrentList { get; set; } = new();
    public CommonInstanceDTO? CurrentItem { get; set; } = null;

    public SelectionModel<object> HeaderSelection { get; }

    
    private void InitCurrentList(object obj)
    {
        Task.Run(async () => await DisplayProviders());
    }

    public async Task UpdateProviders()
    {
        await DisplayProviders();
    }

    private async Task DisplayProviders()
    {
        CurrentList.Clear();

        var instancesDto = Header switch
        {
            "Artists" => supporter.ArtistsCollection,
            "Playlists" => supporter.PlaylistsCollection,
            "Tracks" => supporter.TracksCollection,
        };

        instancesDto
            .ToList()
            .ForEach(i => CurrentList.Add(i));
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
             "Artists" => (BaseViewModel)App.ViewModelTable[ArtistEditorViewModel.nameVM],
             "Playlists" => (BaseViewModel)App.ViewModelTable[ArtistEditorViewModel.nameVM],
             "Tracks" =>  (BaseViewModel)App.ViewModelTable[ArtistEditorViewModel.nameVM]
        };
        
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
        };

        Task.Run( async () => await UpdateProviders());
    }
    
    private void Edit(object obj)
    {        
        BaseViewModel editor = null;

        switch(Header)
        {
            case "Artists":
                var artistEditor = (ArtistEditorViewModel)App.ViewModelTable[ArtistEditorViewModel.nameVM];
                artistEditor?.DropInstance(CurrentItem ?? default!);
                editor = artistEditor;
                break;
            case "Playlists": 
                var playlistEditor = (PlaylistEditorViewModel)App.ViewModelTable[PlaylistEditorViewModel.nameVM];
                playlistEditor?.DropInstance(CurrentItem ?? default!);
                editor = playlistEditor;
                break;
            case "Tracks":
                var trackEditor = (TrackEditorViewModel)App.ViewModelTable[TrackEditorViewModel.nameVM];
                trackEditor?.DropInstance(CurrentItem ?? default!);
                editor = trackEditor;
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

            if(Header.Equals("Artists"))
            {
                MainVM.ResolveInstance(this, currentItem).Start(); 
            }
            else if(Header.Equals("Playlists"))
            {
                PassPlaylistEntity(currentItem);

            }
            else if(Header.Equals("Tracks"))
            {
                PassTrackEntity(currentItem);
            }
        }
    }

    private void PassPlaylistEntity(CommonInstanceDTO playlistDto)
    {

        var mainPlaylistId = MainVM.CurrentPlaylist?.Id ?? Guid.Empty;
        if(mainPlaylistId.Equals(playlistDto.Id))
        {
            MainVM.ResolveInstance(this, playlistDto).Wait();
        }
        else
        {
            var playlist = supporter.GetPlaylistAsync(playlistDto).Result;
            Task.Run(() => MainVM.DropPlaylistInstance(this, playlist));
        }
    }

    private void PassTrackEntity(CommonInstanceDTO trackDto)
    {
        var mainPlaylistId = MainVM.CurrentTrack?.Id ?? Guid.Empty;
        if(mainPlaylistId.Equals(trackDto.Id))
        {
            MainVM.ResolveInstance(this, trackDto).Start();
        }
        else
        {
            var track = supporter.GetTrackAsync(trackDto).Result;
            Task.Run(() => MainVM.DropTrackInstance(this, track));
        }
    }
}