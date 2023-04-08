using ShareInstances;
using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Entities;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.ViewModels;

using System;
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
    
    private static IPlayer _player;

    #region Services
    private SupporterService supporter => (SupporterService)App.Stage.GetServiceInstance("SupporterService");
    private FactoryService factory => (FactoryService)base.GetService("FactoryService");
    private ViewModelHolder<BaseViewModel> holder => (ViewModelHolder<BaseViewModel>)base.GetService("HolderService");
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
    public static ObservableCollection<ICoreEntity> CurrentList { get; set; } = new();
    public ICoreEntity CurrentItem { get; set; }

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
        DisplayProviders();
    }

    public void UpdateProviders()
    {
        DisplayProviders();
    }

    private void DisplayProviders()
    {
        CurrentList.Clear();
        switch (Header)
        {
            case "Artists":
                supporter.ArtistsCollection.ToList().ForEach(a => CurrentList.Add(a));
                break;
            case "Playlists":
                supporter.PlaylistsCollection.ToList().ForEach(p => CurrentList.Add(p));
                break;
            case "Tracks":
                supporter.TracksCollection.ToList().ForEach(t => CurrentList.Add(t));
                break;
        }      
    }
    #endregion

    #region CommandMethods
    private void Add(object obj)
    {
        var factory = (FactoryViewModel)App.ViewModelTable[FactoryViewModel.nameVM];

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

    private void Delete(object obj) 
    {
        supporter.DeleteInstance(CurrentItem);
        UpdateProviders();
    }
    
    private void Edit(object obj)
    {
        if(CurrentItem is not null)
        {
            var factory = (FactoryViewModel)App.ViewModelTable[FactoryViewModel.nameVM];
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