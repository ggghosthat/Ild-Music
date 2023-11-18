using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Stage;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
namespace Ild_Music.ViewModels;
public class BrowseViewModel : BaseViewModel
{
	public static readonly string nameVM = "BrowseVM";        
    public override string NameVM => nameVM;

    #region Services
    private MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    private SupportGhost supporterService => (SupportGhost)base.GetService(Ghosts.SUPPORT);
    private Filer filer => (Filer)GetWaiter("Filer");
    #endregion

    #region Properties
    public ObservableCollection<Track> Items {get; private set;} = new();
    public ObservableCollection<Track> SelectedItems {get; set;} = new();
    #endregion

    #region Commands
    public CommandDelegator BrowseFromManagerCommand { get; }
    public CommandDelegator PlaybackSingleCommand { get; }
    public CommandDelegator CreateTrackCommand { get; }
    public CommandDelegator CreatePlaylistCommand { get; }
    public CommandDelegator PlayBrowsedTracksCommand { get; }
    public CommandDelegator CancelCommand { get; }
    #endregion

    public BrowseViewModel()
    {
        BrowseFromManagerCommand = new(BrowseFromManager, null);
        PlaybackSingleCommand = new(PlaybackSingle, null);
        CreateTrackCommand = new(CreateTrack, null);
        CreatePlaylistCommand = new(CreatePlaylist, null);
        PlayBrowsedTracksCommand = new(PlayBrowsedTracks, null);
        CancelCommand = new(Cancel, null);
    }

    #region Private methods
    private async Task UpdateItems()
    {
        Items.Clear();
        IList<Track> musicFiles = filer.GetTracks();
        musicFiles.ToList()
                  .ForEach(mf => Items.Add(mf));
        filer.CleanFiler();
    }
    #endregion

    #region Public methods
    public async Task Browse(IEnumerable<string> paths)
    {
        await filer.BrowseFiles(ref paths);
        await UpdateItems();
    }
    #endregion

    #region Command Methods
    private void PlaybackSingle(object obj)
    {
        if(SelectedItems is not null && SelectedItems.Count > 0)
        {
            var currentEntity = MainVM.CurrentEntity;
            var producedTrack = SelectedItems[0].MusicFileConvertTrack();
            SelectedItems.Clear();

            if(currentEntity is not null && producedTrack.Id.Equals(currentEntity.Id))
            {
                new Task(() => MainVM.DropInstance(this, producedTrack)).Start();
            }
            else
            {
                Task.Run(() => MainVM.DropInstance(this, producedTrack));                    
            }
        }
    }

    private void CreateTrack(object obj)
    {
        if(SelectedItems is not null && SelectedItems.Count > 0)
        {
            Track producedTrack = SelectedItems[0].MusicFileConvertTrack();
            supporterService.AddInstance(producedTrack);
            supporterService.DumpState();
        }
    }

    private void CreatePlaylist(object obj)
    {
        if(SelectedItems is not null && SelectedItems.Count > 0)
        {
            Playlist producedPlaylist = SelectedItems.MusicFileConvertPlaylist(supporter:supporterService);
            supporterService.AddInstance(producedPlaylist);
            supporterService.DumpState();
        }
    }

    private void PlayBrowsedTracks(object obj)
    {
        if(Items is not null && Items.Count > 0)
        {
            Playlist producedPlaylist = Items.MusicFileConvertPlaylist(supporter:supporterService);
            MainVM.HitTemps(Items);
        } 
    }

    private async void BrowseFromManager(object obj)
    {
        OpenFileDialog dialog = new();
        dialog.AllowMultiple = true;
        dialog.Filters.Add(new FileDialogFilter() { Name = "Mp3", Extensions =  { "mp3" } });

        string[] result = await dialog.ShowAsync(new Window());
        if(result != null && result.Length > 0)
        {
            Browse(result);
        }
    }

    private void Cancel(object obj)
    {
        if(SelectedItems is not null)
        {
            SelectedItems.ToList()
                         .ForEach(mf => Items.Remove(mf));
        }
    }    
    #endregion
}
