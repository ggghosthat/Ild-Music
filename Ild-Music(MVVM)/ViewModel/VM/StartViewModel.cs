using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_MVVM_.Services;
using System.Collections.ObjectModel;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class StartViewModel : Base.BaseViewModel
    {
        #region Item Collection
        public ObservableCollection<Track> TracksItem { get; set; } 
        public ObservableCollection<Tracklist> PlaylistsItem { get; set; }
        public ObservableCollection<Artist> ArtistsItem { get; set; }
        #endregion
        public StartViewModel()
        {
            InitItems();
        }

        //Collections providing
        private void InitItems()
        {
            if (base.GetService("Supporter") is SupporterService supporter)
            {
                TracksItem = supporter.TrackSup;
                PlaylistsItem = supporter.PlaylistSup;
                ArtistsItem = supporter.ArtistsSup;
            }
        }
    }
}
