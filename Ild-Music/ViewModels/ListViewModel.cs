using Ild_Music.ViewModels.Base;
using Ild_Music.Command;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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

    public static ObservableCollection<string> Headers { get; private set; } = new() {"Artists","Playlists","Tracks"};
    public string Header { get; set; } = Headers[0];

    public static ObservableCollection<CommonInstanceDTO> CurrentList { get; set; } = new();
    public CommonInstanceDTO? CurrentItem { get; set; }

    public override void Load()
    {
        DisplayProvidersAsync().Wait();
    }

    public async Task UpdateProviders()
    {
        await DisplayProvidersAsync();
    }
 
    public async Task BrowseTracks(IEnumerable<string> paths)
    {
        if(Header is "Tracks")
            paths.ToList().ForEach(path => factory.CreateTrack(path));
    }

    public Task ExtendCurrentList()
    {
        var items = supporter.PageForward().Result;
        
        if (items.Count() > 0)
            items.ToList().ForEach(i => CurrentList.Add(i));
        
        return Task.CompletedTask;
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

        if (CurrentItem is CommonInstanceDTO dto)
            CurrentList.Remove(dto);
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

    private void InitCurrentList(object obj)
    {
        DisplayProvidersAsync().Wait();
    }

    private void Back(object obj)
    {        
        MainVM.ResolveWindowStack();
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

    private Task DisplayProvidersAsync()
    {        
        using (var instancePool = supporter.GetInstancePool().Result)
        {
            CurrentList.Clear();
            
            switch(Header)
            {
                case "Artists":
                    supporter.ResolveMetaData(Core.Instances.EntityTag.ARTIST);
                    instancePool.ArtistsDTOs.ToList().ForEach(a => CurrentList.Add(a));
                    break;
                case "Playlists":
                    supporter.ResolveMetaData(Core.Instances.EntityTag.PLAYLIST);
                    instancePool.PlaylistsDTOs.ToList().ForEach(p => CurrentList.Add(p));
                    break;
                case "Tracks":
                    supporter.ResolveMetaData(Core.Instances.EntityTag.TRACK);
                    instancePool.TracksDTOs.ToList().ForEach(t => CurrentList.Add(t));
                    break;
                case "Tags":
                    supporter.ResolveMetaData(Core.Instances.EntityTag.TAG);
                    instancePool.TagsDTOs.ToList().ForEach(tag => CurrentList.Add(tag));
                    break;
            }
        }

        return Task.CompletedTask;
    } 
}
