using Ild_Music_CORE.Models.Core;
using Ild_Music_CORE.Models.CORE.Tracklist_Structure;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ild_Music.Controllers.ControllerServices
{
    public class EntitiesFactory
    {
        private Artist _artist;
        private Track _track;
        private Tracklist _playlist;



        public void GenerateArtist(string name, string description) 
        {
            _artist = new Artist(name: name, description: description);
        }

        public void GeneratePlaylist(string name, string description)
        {
            _playlist = new Tracklist(name: name, description: description);
        }

        public void GenerateTrack(string pathway, string name, string description, Artist? artist = null)
        {
            _track = new Track(pathway: pathway, name: name, description: description);
            if (artist != null)
            {
                _track.SetArtist(artist);
            }
            else
            {
                _track.SetArtist(new Artist(name: "Unknown", description:"Unknown artist"));
            }
        }



        public void GetArtist(out Artist? artist) 
        {
            artist = _artist ?? null;
        }

        public void GetTrack(out Track? track) 
        {
            track = _track ?? null;
        }

        public void GetPlaylist(out Tracklist? playlist) 
        {
            playlist = _playlist ?? null;
        }
    }
}
