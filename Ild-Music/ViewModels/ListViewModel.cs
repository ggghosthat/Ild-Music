
using Ild_Music.Command;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Contracts;
using Ild_Music.ViewModels.Base;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class ListViewModel : BaseViewModel, IFileDropable
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
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];

    public CommandDelegator AddCommand { get; }
    
    public CommandDelegator DeleteCommand { get; }
    
    public CommandDelegator EditCommand { get; }
    
    public CommandDelegator BackCommand { get; }
    
    public CommandDelegator ItemSelectCommand { get; }

    public CommandDelegator InitCurrentListCommand { get; }

    public static ObservableCollection<string> Headers { get; private set; } = new() {"Artists","Playlists","Tracks"};

    public static ObservableCollection<EntityTag> HeaderTags { get; private set; } = new() {EntityTag.ARTIST, EntityTag.PLAYLIST, EntityTag.TRACK};

    public string Header { get; set; } = Headers[0];

    public EntityTag HeaderTag { get; set; } = HeaderTags[0];

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
 
    public void DropFile(string filePath)
    {
        factory.CreateTrack(filePath);
        DisplayProvidersAsync().Wait();
    }

    public void DropFiles(IEnumerable<string> filePaths)
    {
        filePaths.ToList().ForEach(path => factory.CreateTrack(path));
        DisplayProvidersAsync().Wait();
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
        BaseViewModel editor = HeaderTag switch
        {
            EntityTag.ARTIST => (BaseViewModel)App.ViewModelTable[ArtistEditorViewModel.viewModelId],
            EntityTag.PLAYLIST => (BaseViewModel)App.ViewModelTable[PlaylistEditorViewModel.viewModelId],
            EntityTag.TRACK =>  (BaseViewModel)App.ViewModelTable[TrackEditorViewModel.viewModelId],
            EntityTag.TAG => (BaseViewModel)App.ViewModelTable[TagEditorViewModel.viewModelId],
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
        
        switch (HeaderTag)
        {
            case EntityTag.ARTIST:
                supporter.DeleteArtistInstance(id);
                break;
            case EntityTag.PLAYLIST: 
                supporter.DeletePlaylistInstance(id);
                break;
            case EntityTag.TRACK:
                supporter.DeleteTrackInstance(id);
                break;
            case EntityTag.TAG:
                supporter.DeleteTagInstance(id);
                break;
        };

        if (CurrentItem is CommonInstanceDTO dto)
            CurrentList.Remove(dto);
    }
    
    private void Edit(object obj)
    {        
        BaseViewModel editor = null;

        switch(HeaderTag)
        {
            case EntityTag.ARTIST:
                var artistEditor = (ArtistEditorViewModel)App.ViewModelTable[ArtistEditorViewModel.viewModelId];
                artistEditor?.DropInstance(CurrentItem ?? default).Wait();
                editor = artistEditor;
                break;
            case EntityTag.PLAYLIST: 
                var playlistEditor = (PlaylistEditorViewModel)App.ViewModelTable[PlaylistEditorViewModel.viewModelId];
                playlistEditor?.DropInstance(CurrentItem ?? default!);
                editor = playlistEditor;
                break;
            case EntityTag.TRACK:
                var trackEditor = (TrackEditorViewModel)App.ViewModelTable[TrackEditorViewModel.viewModelId];
                trackEditor?.DropInstance(CurrentItem ?? default!);
                editor = trackEditor;
                break;
            case EntityTag.TAG:
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

    public void ItemSelect(object obj)
    {
        if(CurrentItem is not null)
        {
            var currentItem = CurrentItem ?? default!; 

            switch (HeaderTag)
            {
                case EntityTag.ARTIST:
                    MainVM.ResolveInstance(this, currentItem);
                    break;
                case EntityTag.PLAYLIST:
                    PassPlaylistEntity(currentItem);
                    break;
                case EntityTag.TRACK:
                    PassTrackEntity(currentItem);
                    break;
                default:
                    break;
            }
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

    public Task DisplayProvidersAsync()
    {        
        using (var instancePool = supporter.GetInstancePool().Result)
        {
            CurrentList.Clear();
            
            switch(HeaderTag)
            {
                case EntityTag.ARTIST:
                    supporter.ResolveMetaData(Core.Instances.EntityTag.ARTIST);
                    instancePool.ArtistsDTOs.ToList().ForEach(a => CurrentList.Add(a));
                    break;
                case EntityTag.PLAYLIST:
                    supporter.ResolveMetaData(Core.Instances.EntityTag.PLAYLIST);
                    instancePool.PlaylistsDTOs.ToList().ForEach(p => CurrentList.Add(p));
                    break;
                case EntityTag.TRACK:
                    supporter.ResolveMetaData(Core.Instances.EntityTag.TRACK);
                    instancePool.TracksDTOs.ToList().ForEach(t => CurrentList.Add(t));
                    break;
                case EntityTag.TAG:
                    supporter.ResolveMetaData(Core.Instances.EntityTag.TAG);
                    instancePool.TagsDTOs.ToList().ForEach(tag => CurrentList.Add(tag));
                    break;
            }
        }

        return Task.CompletedTask;
    } 
}
