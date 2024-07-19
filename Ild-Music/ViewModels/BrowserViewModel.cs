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

        SaveTracksCommand = new(SaveTracks, null);
        CreatePlaylistCommand = new(CreatePlaylist, null);
        BackCommand = new(Back, null);
    }

    public ObservableCollection<Track> Source { get; private set; } = new();
    public ObservableCollection<Track> Output { get; set; } = new();

    public CommandDelegator SaveTracksCommand { get; }
    public CommandDelegator CreatePlaylistCommand { get; }
    public CommandDelegator BackCommand { get; }
    
    public async Task Browse(IList<string> paths)
    {
        await _filer.BrowseFiles(paths);        
        Source.Clear();
        _filer.GetTracks()
             .ToList()
             .ForEach(mf => Source.Add(mf));
        _filer.CleanFiler();
    }

    private void SaveTracks(object obj)
    {
        if (Output.Count == 0)
            return;

        foreach (var track in Output)
            supporter.AddTrackInstance(track);
    }

    public void CreatePlaylist(object obj)
    {
        if (Output.Count == 0)
            return;

        foreach (var track in Output)
            PlaylistEditor.SelectedPlaylistTracks.Add(track.ToCommonDTO());
        
        PlaylistEditor.Name = $"Playlist {DateTime.Now.ToString("MMMM dddd")}";
        PlaylistEditor.Description = "";
        PlaylistEditor.Year = DateTime.Now.Year;
        
        MainVM.PushVM(this, PlaylistEditor);
        MainVM.ResolveWindowStack();
    }
  
    public void Back(object obj)
    {
       Source.Clear();
       Output.Clear();
       MainVM.ResolveWindowStack();
    }
}
