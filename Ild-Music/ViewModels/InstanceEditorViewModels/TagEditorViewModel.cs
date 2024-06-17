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

public class TagEditorViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    public TagEditorViewModel()
    {
        CreateTagCommand = new(HandleChanges, null);
        CancelCommand = new(Cancel, null);
        TagArtistExplorerCommand = new(OpenTagArtistExplorer, null);
        TagPlaylistExplorerCommand = new(OpenTagPlaylistExplorer, null);
        TagTrackExplorerCommand = new(OpenTagTrackExplorer, null);

        Explorer.OnSelected += this.OnArtistsItemsSelected;
        Explorer.OnSelected += this.OnPlaylistsItemsSelected;
        Explorer.OnSelected += this.OnTracksItemsSelected;
    }
    
    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private static MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];
    private static InstanceExplorerViewModel Explorer => (InstanceExplorerViewModel)App.ViewModelTable[InstanceExplorerViewModel.viewModelId];
    
    public CommandDelegator CreateTagCommand { get; }
    public CommandDelegator CancelCommand { get; }
    public CommandDelegator TagArtistExplorerCommand { get; }
    public CommandDelegator TagPlaylistExplorerCommand { get; }
    public CommandDelegator TagTrackExplorerCommand { get; }

    public ObservableCollection<CommonInstanceDTO> ArtistProvider { get; set; } = new();
    public ObservableCollection<CommonInstanceDTO> PlaylistProvider { get; set; } = new();
    public ObservableCollection<CommonInstanceDTO> TrackProvider { get; set; } = new();

    public ObservableCollection<CommonInstanceDTO> SelectedTagArtists { get; set; } = new();
    public ObservableCollection<CommonInstanceDTO> SelectedTagPlaylists { get; set; } = new();
    public ObservableCollection<CommonInstanceDTO> SelectedTagTracks { get; set; } = new();

    public static Tag TagInstance { get; private set; } = default!;
    public string Name {get; set; } = default!;
    public string Color { get; set; } = default!;

    public string TagLogLine { get; set; } = default!;
    public bool TagLogError { get; set; } = default!;

    public bool IsEditMode {get; private set;} = false;
    public string ViewHeader {get; private set;} = "Tag";

    private async void ExitFactory()
    {
        await FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private async Task FieldsClear()
    {
        Name = default!;
        Color = default!;
    }

    public void CreateTagInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            factory.CreateTag(Name, Color);
            TagLogLine = "Successfully created!";
            ExitFactory();
        }
        catch (InvalidArtistException ex)
        {
            TagLogLine = ex.Message;
        }
    }

    public void EditTagInstance()
    {
        try
        {
            if (String.IsNullOrEmpty(Name))
                throw new InvalidArtistException();

            var editTag = TagInstance;
            editTag.Name = Name.AsMemory();
            editTag.Color = Color.AsMemory();

            supporter.EditTagInstance(editTag);
            IsEditMode = false;
            ExitFactory();                
        }
        catch(Exception exm)
        {
            TagLogLine = exm.Message;
        }
    }

    public async Task DropInstance(CommonInstanceDTO tagDto) 
    {
        TagInstance = await supporter.GetTagAsync(tagDto.Id);
        IsEditMode = true;
        Name = TagInstance.Name.ToString();
        Color = TagInstance.Color.ToString();
    }

    public async Task DropInstance(Tag tag) 
    {
        TagInstance = tag;
        IsEditMode = true;
        Name = TagInstance.Name.ToString();
        Color = TagInstance.Color.ToString();
    }

    private void Cancel(object obj)
    {
        ExitFactory();
    }

    private void OnArtistsItemsSelected()
    {
        if(Explorer.Output.Count > 0)
        {
            if (Explorer.Output[0].Tag == EntityTag.ARTIST)
            {
                SelectedTagArtists.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                 
                supporter?.ArtistsCollection?
                    .Where(a => outIds.Contains(a.Id))
                    .ToList()
                    .ForEach(i => SelectedTagArtists.Add(i));
            }
        }
    }

    private void OpenTagArtistExplorer(object obj)
    {
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.ARTIST)
            Explorer.Arrange(EntityTag.ARTIST, preSelected); 
        else
            Explorer.Arrange(EntityTag.ARTIST); 

        MainVM.PushVM(null, Explorer);
        MainVM.ResolveWindowStack();        
    }

    private void OnPlaylistsItemsSelected()
    {
        if(Explorer.Output.Count > 0)
        {
            if (Explorer.Output[0].Tag == EntityTag.PLAYLIST)
            {
                SelectedTagPlaylists.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                 
                supporter?.ArtistsCollection?
                    .Where(a => outIds.Contains(a.Id))
                    .ToList()
                    .ForEach(i => SelectedTagPlaylists.Add(i));
            }
        }
    }

    private void OpenTagPlaylistExplorer(object obj)
    {
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.PLAYLIST)
            Explorer.Arrange(EntityTag.PLAYLIST, preSelected); 
        else
            Explorer.Arrange(EntityTag.PLAYLIST); 

        MainVM.PushVM(null, Explorer);
        MainVM.ResolveWindowStack();        
    }

    private void OnTracksItemsSelected()
    {
        if(Explorer.Output.Count > 0)
        {
            if (Explorer.Output[0].Tag == EntityTag.TRACK)
            {
                SelectedTagTracks.Clear();
                var outIds = Explorer.Output.Select(o => o.Id);
                                 
                supporter?.ArtistsCollection?
                    .Where(a => outIds.Contains(a.Id))
                    .ToList()
                    .ForEach(i => SelectedTagTracks.Add(i));
            }
        }
    }

    private void OpenTagTrackExplorer(object obj)
    {
        if (obj is IList<CommonInstanceDTO> preSelected &&
            preSelected[0].Tag == EntityTag.TRACK)
            Explorer.Arrange(EntityTag.TRACK, preSelected); 
        else
            Explorer.Arrange(EntityTag.TRACK); 

        MainVM.PushVM(null, Explorer);
        MainVM.ResolveWindowStack();        
    }

    private void HandleChanges(object obj)
    {
        if (IsEditMode)
            EditTagInstance();
        else
            CreateTagInstance();
    }

    private static IEnumerable<Window> Windows =>
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows ?? Array.Empty<Window>();

    public Window? FindWindowByViewModel(INotifyPropertyChanged viewModel) =>
        Windows.FirstOrDefault(x => ReferenceEquals(viewModel, x.DataContext));
}
