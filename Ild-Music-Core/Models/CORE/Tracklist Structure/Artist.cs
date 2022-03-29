using Ild_Music_CORE.Models.Core;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ild_Music_CORE.Models.CORE.Tracklist_Structure
{
    public class Artist : ResourceRoot, IDescriptional
    {
        #region Fields
        private string id;
        private string name;
        private IList<Tracklist> _tracks;
        private IList<Track> tracks_collection;
        private IList<string> tracksId_collection;
        #endregion

        #region Properties
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name 
        {
            get { return name; } 
            set { name = value; } 
        }
        
        public IList<Tracklist> Tracks 
        {
            get { return _tracks; } 
            set { _tracks = value; } 
        }
        public IList<Track> TracksCollection {
            get{ return tracks_collection;}
            set{ tracks_collection = value;}
        }
        public IList<string> TracksIdCollection 
        {
            get{return tracksId_collection;}
            set{tracksId_collection = value;}
        }

        public string Description { get; set; }
        #endregion


        #region contr

        public Artist()
        {

        }

        public Artist(string name)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
        }

        public Artist(string name, string? description = null)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.Description = description ?? string.Empty;
        }

        public Artist(string id_str, string name, string? description = null)
        {
            this.id = id_str;
            this.name = name;
            this.Description = description ?? string.Empty;
        }

        public Artist(string name, IList<Tracklist> tracks)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this._tracks = tracks;
        }

        public Artist(string name, IList<Track> tracks_collection)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.tracks_collection = tracks_collection;
        }

        public Artist(string name, Track? track = null, string? trackId = null)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            if (track != null)
            {
                tracks_collection = new List<Track>();
                tracks_collection.Add(track);
            }
            if (trackId != null)
            {
                tracksId_collection = new List<string>();
                tracksId_collection.Add(trackId);
            }
        }

        public Artist(string name, Tracklist track)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            tracks_collection = new List<Track>();
            _tracks.Add(track);
        }

        #endregion

        #region Methods
        public void AddTrack(Track track)
        {
            if (_tracks != null)
            {
                tracks_collection.Add(track);
            }
            else
            {
                tracks_collection = new List<Track>();
                tracks_collection.Add(track);
            }
        }

        public void AddTracklist(Tracklist tracklist)
        {
            if (_tracks != null)
            {
                _tracks.Add(tracklist);
            }
            else
            {
                _tracks = new List<Tracklist>();
                _tracks.Add(tracklist);
            }
        }


        public override string ToString()
        {
            return $"{name}";
        }

        public object ToList()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
