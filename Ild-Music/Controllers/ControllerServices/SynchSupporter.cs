using Ild_Music_CORE.Models.Core;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models.CORE.Tracklist_Structure;
using Ild_Music_CORE.Models.Interfaces;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ild_Music.Controllers.ControllerServices
{
    internal class SynchSupporter
    {
        private EntityState entityState;
        private Area _area;

        public IList<Artist> ExistedArtists => _area.existedArtists; 
        public IList<Tracklist> ExistedPlaylist => _area.existedPlaylists;
        public IList<Track> ExistedTracks => _area.existedTracks;

        public SynchSupporter()
        {
        }

        public SynchSupporter(Area area)
        {
            _area = area;
            area.Init();
        }


        public void AddInstanceObject(IDescriptional instanceObject)
        {
            if (instanceObject.GetType() is Track)
            {
                _area.AddTrackObj((Track)instanceObject);
            }
            else if (instanceObject.GetType() is Tracklist)
            {
                _area.AddPlaylistObj((Tracklist)instanceObject);
            }
            else if (instanceObject.GetType() is Artist)
            {
                _area.AddArtistObj((Artist)instanceObject);
            }
        }

        public void EditInstanceObject(IDescriptional instanceObject, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Track:
                    _area.EditTrackObj((Track)instanceObject);
                    _area.SaveTracks();
                    break;
                case EntityState.Playlist:
                    _area.EditPlaylistObj((Tracklist)instanceObject);
                    _area.SavePlaylists();
                    break;
                case EntityState.Artist:
                    _area.EditArtistObj((Artist)instanceObject);
                    _area.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void RemoveInstanceObject(IDescriptional instanceObject, EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Track:
                    _area.RemoveTrackObj((Track)instanceObject);
                    _area.SaveTracks();
                    break;
                case EntityState.Playlist:
                    _area.RemovePlaylistObj((Tracklist)instanceObject);
                    _area.SavePlaylists();
                    break;
                case EntityState.Artist:
                    _area.RemoveArtistObj((Artist)instanceObject);
                    _area.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void RefreshList(EntityState state)
        {
            _area.Init();
            
        }

        public void SaveSingleSatate(EntityState state) 
        {
            switch (state)
            {
                case EntityState.Track:
                    _area.SaveTracks();
                    break;
                case EntityState.Playlist:
                    _area.SavePlaylists();
                    break;
                case EntityState.Artist:
                    _area.SaveArtists();
                    break;
                default:
                    break;
            }
        }

        public void SaveState()
        {
            _area.Save();
        }

        ~SynchSupporter() 
        {
            _area.Save();
        }

    }
}
