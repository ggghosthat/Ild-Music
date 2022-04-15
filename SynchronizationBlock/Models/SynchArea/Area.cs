using System.IO;
using System.Collections.Generic;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;

using SynchronizationBlock.Models.PlaylistSymch;
using SynchronizationBlock.Models.TrackSynch;
using SynchronizationBlock.Models.ArtistSynch;

namespace SynchronizationBlock.Models.SynchArea
{
    public class Area
    {
        private TrackSynch<Track> trackSynch = new TrackSynch<Track>();
        private ArtistSynch<Artist> artistSynch = new ArtistSynch<Artist>();
        private PlaylistSynch<Tracklist> playlistSynch = new PlaylistSynch<Tracklist>();


        public IList<Artist> existedArtists => artistSynch.Instances;
        public IList<Track> existedTracks => trackSynch.Instances;
        public IList<Tracklist> existedPlaylists => playlistSynch.Instances;

        

        public Area()
        {
        }



        public void Init()
        {
            artistSynch.Deserialize();
            trackSynch.Deserialize();
            playlistSynch.Deserialize();
        }

        public void SetPrefix(string pref)
        {
            if (File.Exists(pref)) 
            {
                trackSynch.Prefix = pref;
                artistSynch.Prefix = pref;
                playlistSynch.Prefix = pref;
            }
        }


        public void AddArtistObj(Artist artist)
        {
            artistSynch.AddInstance(artist);
        }

        public void AddTrackObj(Track track)
        {
            trackSynch.AddInstance(track);
        }

        public void AddPlaylistObj(Tracklist playlist)
        {
            playlistSynch.AddInstance(playlist);
        }


        public void EditArtistObj(Artist artist)
        {
            artistSynch.EditInstance(artist);
        }

        public void EditTrackObj(Track track) 
        {
            trackSynch.EditInstance(track);
        }

        public void EditPlaylistObj(Tracklist playlist) 
        {
            playlistSynch.EditInstance(playlist);
        }


        public void RemoveArtistObj(Artist artist)
        {
            artistSynch.RemoveInstance(artist);
        }

        public void RemoveTrackObj(Track track)
        {
            trackSynch.RemoveInstance(track);
        }

        public void RemovePlaylistObj(Tracklist playlist)
        {
            playlistSynch.RemoveInstance(playlist);
        }


        public void Save() 
        {
            artistSynch.Serialize();
            trackSynch.Serialize();
            playlistSynch.Serialize();
        }

        public void SaveArtists()
        {
            artistSynch.Serialize();
        }
        
        public void SavePlaylists()
        {
            playlistSynch.Serialize();
        }

        public void SaveTracks() 
        {
            trackSynch.Serialize();
        }


        
    }
}
