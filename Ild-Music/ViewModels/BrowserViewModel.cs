using Ild_Music.Command;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Services.Entities;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Contracts.Services.Interfaces;

using System;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;

public class BrowserViewModel : BaseViewModel
{
    public static readonly string nameVM = "BrowserVM";
    public override string NameVM => nameVM;

    public BrowserViewModel()
    {
       SaveTracksCommand = new(SaveTracks, null);
       CreatePlaylistCommand = new(CreatePlaylist, null);
       BackCommand = new(Back, null);
    }

    private static SupportGhost supporter => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static FactoryGhost factory => (FactoryGhost)App.Stage.GetGhost(Ghosts.FACTORY);
    private MainWindowViewModel MainVM => (MainWindowViewModel)App.ViewModelTable[MainWindowViewModel.nameVM];
    private PlaylistEditorViewModel PlaylistEditor => (PlaylistEditorViewModel)App.ViewModelTable[PlaylistEditorViewModel.nameVM];

    public ObservableCollection<Track> Source = new();
    public ObservableCollection<Track> Output = new();

    public CommandDelegator SaveTracksCommand { get; }
    public CommandDelegator CreatePlaylistCommand { get; set; }
    public CommandDelegator BackCommand { get; }
   
    private void ExitFactory()
    {
        FieldsClear();
        MainVM.ResolveWindowStack();
    }

    private void FieldsClear()
    {
       Source.Clear();
       Output.Clear();
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
        ExitFactory();
    }
}
