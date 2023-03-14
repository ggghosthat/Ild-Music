using ShareInstances.PlayerResources;
using Ild_Music_MVVM_.Services;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class StartViewModel : Base.BaseViewModel
    {
        public static readonly string nameVM = "StartVM";
        private SupporterService supporter => (SupporterService)base.GetService("Supporter");

        #region Item Collection
        public ObservableCollection<Track> TracksItem { get; set; } = new();
        public ObservableCollection<Playlist> PlaylistsItem { get; set; } = new();
        public ObservableCollection<Artist> ArtistsItem { get; set; } = new();
        #endregion

        #region Props
        public Artist SelectedArtist { get; set; }
        public Playlist SelectedPlaylist { get; set; }
        public Track SelectedTrack { get; set; }
        #endregion

        public StartViewModel()
        {
            Task.Run(PopullateLists);
            supporter.Area.OnArtistSynchRefresh += Area_OnArtistSynchRefresh;
            supporter.Area.OnPlaylistSynchRefresh += Area_OnPlaylistSynchRefresh;
            supporter.Area.OnTrackSynchRefresh += Area_OnTrackSynchRefresh;
        }


        #region Popullation funcs
        private void Area_OnArtistSynchRefresh()
        {
            ArtistsItem.Clear();
            supporter.ArtistSup.ToList().ForEach(a => ArtistsItem.Add(a));
        }

        private void Area_OnPlaylistSynchRefresh()
        {
            PlaylistsItem.Clear();
            supporter.PlaylistSup.ToList().ForEach(p => PlaylistsItem.Add(p));
        }

        private void Area_OnTrackSynchRefresh()
        {
            TracksItem.Clear();
            supporter.TrackSup.ToList().ForEach(t => TracksItem.Add(t));
        }


        private void PopullateLists()
        {
            supporter.ArtistSup.ToList().ForEach(a => ArtistsItem.Add(a));
            supporter.PlaylistSup.ToList().ForEach(p => PlaylistsItem.Add(p));
            supporter.TrackSup.ToList().ForEach(t => TracksItem.Add(t));         
        }
        #endregion
    }
}
