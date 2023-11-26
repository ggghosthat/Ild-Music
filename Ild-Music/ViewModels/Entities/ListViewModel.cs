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
    private SupportGhost supporter => (SupportGhost)App.Stage.GetServiceInstance(Ghosts.SUPPORT);
    private FactoryGhost factory => (FactoryGhost)base.GetService(Ghosts.FACTORY);
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
    public CommonInstanceDTO CurrentItem { get; set; }

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

    #region Public Methods
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
        switch (Header)
        {
            case "Artists":
                supporter.DeleteArtistInstance(CurrentItem.Id);
                break;
            case "Playlists": 
                supporter.DeletePlaylistInstance(CurrentItem.Id);
                break;
            case "Tracks":
                supporter.DeleteTrackInstance(CurrentItem.Id);
                break;
        };

        Task.Run( async () => await UpdateProviders());
    }
    
    private void Edit(object obj)
    {
        if(CurrentItem != default!)
        {
            var factory = (FactoryContainerViewModel)App.ViewModelTable[FactoryContainerViewModel.nameVM];
            factory.SetEditableItem(CurrentItem);

            switch (Header)
            {
                case "Artists":
                    factory.SetSubItem(index:0);
                    break;
                case "Playlists":
                    factory.SetSubItem(index:1);
                    break;
                case "Tracks":
                    factory.SetSubItem(index:2);
                    break;
            }

            MainVM.PushVM(this, factory);
            MainVM.ResolveWindowStack();
        }
    }

    private void ItemSelect(object obj)
    {
        if(CurrentItem != default)
        {
            var currentEntity = MainVM.CurrentEntity;
            if(currentEntity is not null && CurrentItem.Id.Equals(currentEntity.Id))
            {
                new Task(() => MainVM.ResolveInstance(this, CurrentItem)).Start();
            }
            else
            {
                Task.Run(() => MainVM.DropInstance(this, CurrentItem, true));                    
            }
        }
    }
    #endregion
}
