using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.Core.Services.Entities;
using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts.Services.Interfaces;

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
    private static MainViewModel MainVM => (MainViewModel)App.ViewModelTable[MainViewModel.nameVM];
    private static SupportGhost supporterService => (SupportGhost)App.Stage.GetGhost(Ghosts.SUPPORT);
    private static Filer filer;
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
    public CommandDelegator SaveTrackCommand { get; }
    public CommandDelegator SavePlaylistCommand { get; }
    public CommandDelegator CancelCommand { get; }
    #endregion

    public BrowseViewModel()
    {
        string filer_tag = "Filer";
        filer = (Filer)App.Stage.GetWaiter(ref filer_tag);

        BrowseFromManagerCommand = new(BrowseFromManager, null);
        PlaybackSingleCommand = new(PlaybackSingle, null);
        SaveTrackCommand = new(SaveTrack, null);
        SavePlaylistCommand = new(SavePlaylists, null);
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
        await filer.BrowseFiles(paths);
        await UpdateItems();
    }
    #endregion

    #region Command Methods
    private void PlaybackSingle(object obj)
    {
        if(SelectedItems is not null && SelectedItems.Count > 0)
        {
            var currentTrack = MainVM.CurrentTrack;
            var producedTrack = SelectedItems[0];
            SelectedItems.Clear();

            if(currentTrack is not null && producedTrack.Id.Equals(currentTrack?.Id))
            {
                new Task(() => MainVM.DropTrackInstance(this, producedTrack)).Start();
            }
            else
            {
                Task.Run(() => MainVM.DropTrackInstance(this, producedTrack));                    
            }
        }
    }

    private void SaveTrack(object obj)
    {
        if(SelectedItems.Count == 1)
        {
            Track producedTrack = SelectedItems[0];
            supporterService.AddTrackInstance(producedTrack);
        }
    }

    private void SavePlaylists(object obj)
    {
        if(SelectedItems.Count > 1)
        {
            Playlist producedPlaylist = SelectedItems.ToList().ComposePlaylist();
            supporterService.AddPlaylistInstance(producedPlaylist);
        }
        else if(SelectedItems.Count == 0 && Items.Count > 0)
        {
            Playlist producedPlaylist = Items.ToList().ComposePlaylist();
            supporterService.AddPlaylistInstance(producedPlaylist);
        }
    } 

    private void PlayBrowsedTracks(object obj)
    {
        if(Items is not null && Items.Count > 0)
        {
            Playlist producedPlaylist = Items.ToList().ComposePlaylist();
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
