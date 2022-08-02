using ShareInstances.PlayerResources;
using Ild_Music_MVVM_.Services;
using System.Collections.Generic;

namespace Ild_Music_MVVM_.ViewModel.VM
{
    public class StartViewModel : Base.BaseViewModel
    {
        #region Item Collection
        public IList<Track> TracksItem { get; set; } 
        public IList<Playlist> PlaylistsItem { get; set; }
        public IList<Artist> ArtistsItem { get; set; }
        #endregion

        public StartViewModel()
        {
            InitItems();
        }

        //Collections providing
        private void InitItems()
        {
            //if (base.GetService("Supporter") is SupporterService supporter)
            //{
            //    TracksItem = supporter.TrackSup;
            //    PlaylistsItem = supporter.PlaylistSup;
            //    ArtistsItem = supporter.ArtistSup;
            //}
        }
    }
}
