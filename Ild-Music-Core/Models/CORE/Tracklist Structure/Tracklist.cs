using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Ild_Music_CORE.Models.CORE.Tracklist_Structure;
using Ild_Music_CORE.Models.Interfaces;

namespace Ild_Music_CORE.Models.Core
{
    public class Tracklist : ResourceRoot, ITrackable, IDescriptional
    {
        #region Fields
        private string id;
        private string name;
        private IList<Track> tracks = new List<Track>();
        private Track head;
        private Track tail;
        #endregion

        #region Properties
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        string ITrackable.id => this.id;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public IList<Track> Tracks
        {
            get { return tracks; }
            set { this.tracks = value; }
        }

        //public Track? Head => tracks[0] ?? null;
        //public Track? Tail => tracks[tracks.Count - 1] ?? null;
        public Track Current
        {
            get;
            set;
        }

        public bool isCurrent 
        {
            get; 
            set; 
        }
        public bool isOrdered 
        {
            get; 
            private set;
        }
        public bool IsSynchronized => default(bool);

        public string Description
        {
            get;
            set;
        }
        public int Count => tracks.Count;
        public object SyncRoot => default(object);
        #endregion


        #region Ctor
        public Tracklist()
        {
        }

        public Tracklist(string name)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
        }

        public Tracklist(string name, string? description = null)
        {
            this.id = Guid.NewGuid().ToString();
            this.name = name;
            this.Description = description ?? string.Empty;
        }

        public Tracklist(string id, string name, string? description = null, Track? trackFirst = null)
        {
            this.id = id;
            this.name = name;
            this.Description = description ?? string.Empty;

            if (trackFirst != null) 
                this.tracks.Add(trackFirst);

        }

        public Tracklist(string id, string name, Artist artist)
        {
            this.id = id;
            this.name = name;
            SetArtist(artist);
        }

        public Tracklist(IList<Track> tracks)
        {
            this.id = Guid.NewGuid().ToString();
            this.tracks = new List<Track>();
            tracks.ToList().ForEach(t => this.tracks.Add(t));
        }

        public Tracklist(IList<string> tracks)
        {
            this.id = Guid.NewGuid().ToString();
            this.tracks = new List<Track>();

            tracks.ToList().ForEach(t =>
            {
                var newTrack = new Track(t);
                this.tracks.Add(newTrack);
            });
        }
        #endregion

        #region Methods

        public void Order()
        {
            if (tracks.Count > 0)
            {
                if (tracks.Count == 1)
                {
                    return;
                } 
                if (!isOrdered)
                {
                    if (tracks != null)
                    {
                        for (int i = 0; i < tracks.Count; i++)
                        {
                            if (i == 0)
                            {
                                head = tracks[0];
                                var next = tracks[i + 1];

                                head.NextTrack = next;
                            }
                            else if (i == tracks.Count - 1)
                            {
                                tail = tracks[i];
                                var previous = tracks[i - 1];

                                tail.PreviousTrack = previous;
                            }
                            else
                            {
                                var current = tracks[i];
                                var previous = tracks[i - 1];
                                var next = tracks[i + 1];

                                current.NextTrack = next;
                                current.PreviousTrack = previous;
                            }
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("Could not order the track list, beacause tracklist is empty");
                    }

                    head.PreviousTrack = tail;
                    tail.NextTrack = head;
                    isOrdered = true;
                }
                
            }
            else if (tracks.Count == 1) 
            {
                return;
            }
        }

        public void CheckIsCurrent()
        {
            if (Current == null)
                isCurrent = false;
            else
                isCurrent = true;
        }

        public void PrepareToSerialize()
        {
            
        }


        public void Add(Track track)
        {
            if (tracks == null)
                tracks = new List<Track>();

            if (tracks.Count > 0 && isOrdered)
            {                
                var tail = tracks.Last();
                tail.NextTrack = track;
                tracks.Add(track);
                return;
            }

            tracks.Add(track);
        }

        public void Remove(Track track)
        {
            if (tracks.Contains(track))
            {
                tracks.First(t => t.Equals(track)).Dispose();
                tracks.Remove(track);
            }
        }

        public void Shuffle() 
        {
            IList<Track> shuffledList = tracks.OrderBy(i => Guid.NewGuid()).ToList();
            tracks = shuffledList;
        }

        public void SetArtist(Artist newArtist)
        {
            newArtist.AddTracklist(this);
        }

        public void SetArtists(IEnumerable<Artist> newArtists)
        {
            foreach (var artist in newArtists)
            {
                artist.AddTracklist(this);
            }
        }

        public Dictionary<string, Track> CraftDictFromTracklist()
        {
            Dictionary<string, Track> result = new Dictionary<string, Track>();
            if (tracks.Count > 0)
            {
                foreach (Track item in tracks)
                {
                    result.Add(item.Name, item);
                }
            }

            return result;
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
