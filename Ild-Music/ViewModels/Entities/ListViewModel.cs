using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.ViewModels;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Avalonia.Controls.Selection;

namespace Ild_Music.ViewModels;
//Types of Lists
public enum ListType {
    ARTISTS,
    PLAYLISTS,
    TRACKS
}

public class ListViewModel : BaseViewModel
{
    public static readonly string nameVM = "ListVM";        
    public override string NameVM => nameVM;
    
    #region Services
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    //private ViewModelHolder<BaseViewModel> holder => (ViewModelHolder<BaseViewModel>)base.GetService("HolderService");
    private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    #endregion

    #region Commands
    public CommandDelegator AddCommand { get; }
    public CommandDelegator DeleteCommand { get; }
    public CommandDelegator EditCommand { get; }
    public CommandDelegator BackCommand { get; }
    public CommandDelegator ItemSelectCommand { get; }
    public CommandDelegator DefineListTypeCommand { get; }
    #endregion

    #region Properties
    public ListType ListType {get; private set;}
    public static ObservableCollection<string> Headers { get; private set; } = new() {"Artists","Playlists","Tracks"};
    public static string Header { get; set; }
    public static ObservableCollection<CommonInstanceDTO> CurrentList { get; set; } = new();
    public CommonInstanceDTO? CurrentItem { get; set; } = null;

    public SelectionModel<object> HeaderSelection { get; }
    #endregion

    #region Ctor
    public ListViewModel()
    {
        AddCommand = new(Add, null);
        DeleteCommand = new(Delete, null);
        EditCommand = new(Edit, null);
        ItemSelectCommand = new(ItemSelect, null);
        DefineListTypeCommand = new(InitCurrentList, null);

        Header = Headers.FirstOrDefault();
        DisplayProviders();
    }
    #endregion

    #region List view model scoped methods
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
        switch (Header)
        {
            case "Artists":
                var artistDTO = await supporter.RequireInstances(EntityTag.ARTIST);
                artistDTO.ToList().ForEach(a => CurrentList.Add(a));
                break;
            case "Playlists":
                var playlistDTO = await supporter.RequireInstances(EntityTag.PLAYLIST);
                playlistDTO.ToList().ForEach(p => CurrentList.Add(p));
                break;
            case "Tracks":
                var trackDTO = await supporter.RequireInstances(EntityTag.TRACK);
                trackDTO.ToList().ForEach(t => CurrentList.Add(t));
                break;
        }      
    }

    public async Task BrowseTracks(IEnumerable<string> paths)
    {
        if(Header is "Tracks")
        {
            paths.ToList()
                 .ForEach(path => factory.CreateTrack(path));

            await UpdateProviders();
        }
    }
    #endregion

    #region CommandMethods
    private void Add(object obj)
    {
        var factory = (FactoryContainerViewModel)App.ViewModelTable[FactoryContainerViewModel.nameVM];

        EntityTag entityTag = Header switch
        {
            "Artists" => EntityTag.ARTIST,
            "Playlists" => EntityTag.PLAYLIST,
            "Tracks" => EntityTag.TRACK
        };

        factory.SetSubItem(entityTag);

        MainVM.PushVM(this, factory);
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
        if(CurrentItem is null)
            return;
       
        var entityId = (Guid)CurrentItem?.Id;
        var factory = (FactoryContainerViewModel)App.ViewModelTable[FactoryContainerViewModel.nameVM];
        
        EntityTag entityTag = Header switch
        {
            "Artists" => EntityTag.ARTIST,
            "Playlists" => EntityTag.PLAYLIST,
            "Tracks" => EntityTag.TRACK
        };

        factory.SetEditableItem(entityTag, entityId);

        MainVM.PushVM(this, factory);
        MainVM.ResolveWindowStack();
    }

    private async void ItemSelect(object obj)
    {
        if(CurrentItem is not null)
        {
            var id = (Guid)CurrentItem?.Id;
            
            if(Header.Equals("Artists"))
            {
                var artist = await supporter.FetchArtist(id);
                MainVM.ResolveArtistInstance(this, artist).Start(); 
            }
            else if(Header.Equals("Playlists"))
            {
                var playlist = await supporter.FetchPlaylist(id);
                PassPlaylistEntity(playlist);

            }
            else if(Header.Equals("Tracks"))
            {
                var track = await supporter.FetchTrack(id);
                PassTrackEntity(track);
            }
        }
    }
    #endregion

    #region Entity pass methods
    private void PassPlaylistEntity(Playlist playlist)
    {
        var mainPlaylistId = MainVM.CurrentPlaylist?.Id ?? Guid.Empty;
        if(mainPlaylistId.Equals(playlist.Id))
            MainVM.ResolvePlaylistInstance(this, playlist).Start();
        else
            Task.Run(() => MainVM.DropPlaylistInstance(this, playlist));
    }

    private void PassTrackEntity(Track track)
    {
       var mainPlaylistId = MainVM.CurrentTrack?.Id ?? Guid.Empty;
        if(mainPlaylistId.Equals(track.Id))
            MainVM.ResolveTrackInstance(this, track).Start();
        else
            Task.Run(() => MainVM.DropTrackInstance(this, track));
    }
    #endregion
}
