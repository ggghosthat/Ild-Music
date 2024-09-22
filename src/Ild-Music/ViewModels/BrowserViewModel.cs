using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ild_Music.ViewModels;

public class BrowserViewModel : BaseViewModel
{
    public static readonly Guid viewModelId = Guid.NewGuid();
    public override Guid ViewModelId => viewModelId;

    private static string FILER_TAG = "Filer";
    public static Filer _filer;

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.viewModelId];
    private PlaylistEditorViewModel PlaylistEditor => (PlaylistEditorViewModel)App.ViewModelTable[PlaylistEditorViewModel.viewModelId];

    public BrowserViewModel()
    {
        _filer = (Filer)App.Stage.GetWaiter(FILER_TAG);

        PlayTrackCommand = new(PlaySingleTrack, null);
        PlaySourceCommand = new(PlaySelectedSource, null);
        SaveTracksCommand = new(SaveTracks, null);
        CreatePlaylistCommand = new(CreatePlaylist, null);
        EraseCommand = new(Erase, null);
        BackCommand = new(Back, null);
    }

    public ObservableCollection<Track> Source { get; private set; } = new();

    public ObservableCollection<Track> Output { get; set; } = new();
   
    public CommandDelegator PlayTrackCommand { get; }

    public CommandDelegator PlaySourceCommand { get; }

    public CommandDelegator SaveTracksCommand { get; }

    public CommandDelegator CreatePlaylistCommand { get; }

    public CommandDelegator EraseCommand { get; }

    public CommandDelegator BackCommand { get; }
    
    public async Task Browse(IEnumerable<string> paths)
    {
        var browsed = _filer.BrowseFiles(paths);
        
        foreach (var track in browsed)
            Source.Add(track);

        _filer.CleanFiler();
    }
    
    private void PlaySingleTrack(object obj)
    {
        if (Output.Count == 1)
        {
            var selectedTrack = Output[0];
            MainVM.DropTrackInstance(this, selectedTrack);
        }
    }

    private void PlaySelectedSource(object obj)
    {
        Playlist tempPlaylist;

        if (Output.Count > 0)
            tempPlaylist = factory.CreateTemporaryPlaylist(Output);
        else
            tempPlaylist = factory.CreateTemporaryPlaylist(Source);
        
        MainVM.DropPlaylistInstance(this, tempPlaylist);
    }

    private void SaveTracks(object obj)
    {
        if (Output.Count == 0)
            return;

        foreach (var track in Output)
            supporter.AddTrackInstance(track);
    }

    private void CreatePlaylist(object obj)
    {
        if (Output.Count == 0)
            return;

        foreach (var track in Output)
        {
            supporter.AddTrackInstance(track);
            PlaylistEditor.SelectedPlaylistTracks.Add(track.ToCommonDTO());
        }

        PlaylistEditor.Name = $"Playlist {DateTime.Now.ToString("MMMM dddd")}";
        PlaylistEditor.Description = "";
        PlaylistEditor.Year = DateTime.Now.Year;
        
        MainVM.PushVM(this, PlaylistEditor);
        MainVM.ResolveWindowStack();
    }
 
    private void Erase(object obj)
    {
        if (Output.Count > 0)
            Output.Clear();
        if (Source.Count > 0)
            Source.Clear();
        
        factory.ClearBrowsedTracks();
    }

    private void Back(object obj)
    { 
       Source.Clear();
       Output.Clear(); 
       factory.ClearBrowsedTracks();
       MainVM.ResolveWindowStack();
    }
}
