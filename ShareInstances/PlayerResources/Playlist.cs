using System;
using System.Collections.Generic;
using System.Linq;
using ShareInstances.PlayerResources.Base;
using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances.PlayerResources
{
    public class Playlist : ResourceRoot, IDescriptional, IDisposable
    {
        #region Fields
        private string id = Guid.NewGuid().ToString();
        private string name;
        private string description;
                
        private Track head;
        private Track tail;

        private IList<Track> tracks_collection = new List<Track>();
        private IDictionary<string, Track> playlist_table = new Dictionary<string, Track>();
        #endregion

        #region Properties
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public override string Name
        {
            get { return name; }
            set { name = value; }
        }
        public IList<Track> Tracks
        {
            get { return tracks_collection; }
            set { tracks_collection = value; }
        }
        public string Description 
        {
            get { return description; }
            set { description = value; }
        }

        public Track Current { get; set; }
        public bool IsCurrent { get; set; } = false;
        public bool IsOrdered { get; private set; } = false;

        public int Count => tracks_collection.Count;
        public bool IsSynchronized => default(bool);
        public object SyncRoot => default(object);
        #endregion

        #region Ctor
        public Playlist()
        {
        }

        public Playlist(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public Playlist(IList<Track> tracks)
        {
            tracks.ToList().ForEach(t => tracks_collection.Add(t));
        }

        #endregion

        #region Order
        public void Order()
        {
            if (tracks_collection != null && 
                tracks_collection.Count > 1 && 
                !IsOrdered)
            {
                int last = tracks_collection.Count;
                for (int i = 0; i < last; i++)
                {
                    if (i == 0)                            
                        OrderHead(i);
                    else if (i == last - 1)
                        OrderTail(i);
                    else
                        OrderBody(i);
                }                 
                RoundCollection();
            }            
        }

        private void RoundCollection()
        {
            head.PreviousTrack = tail;
            tail.NextTrack = head;
            IsOrdered = true;
        }

        private void OrderBody(int i)
        {
            var current = tracks_collection[i];
            var previous = tracks_collection[i - 1];
            var next = tracks_collection[i + 1];

            current.NextTrack = next;
            current.PreviousTrack = previous;
        }

        private void OrderTail(int i)
        {
            tail = tracks_collection[i];
            var previous = tracks_collection[i - 1];

            tail.PreviousTrack = previous;
        }

        private void OrderHead(int i)
        {
            head = tracks_collection[0];
            var next = tracks_collection[i + 1];

            head.NextTrack = next;
        }
        #endregion

        #region CheckCurrentMethods
        public void CheckIsCurrent()
        {
            if (Current != null)
                IsCurrent = true;
        }
        #endregion

        #region CollectionManipulationMethods
        public void AddTrack(Track track)
        {
            if (tracks_collection.Count == 0 )
            {
                tracks_collection.Add(track);
                return;
            }

            if (tracks_collection.Count > 0 && IsOrdered)
            {                
                var tail = tracks_collection.Last();
                tail.NextTrack = track;
                track.PreviousTrack = tail;
                tracks_collection.Add(track);
                return;
            }
        }

        public void RemoveTrack(Track track)
        {
            if (tracks_collection.Contains(track))
            {
                tracks_collection.First(t => t.Equals(track)).Dispose();
                tracks_collection.Remove(track);
            }
        }
        #endregion

        #region Shuffle
        public void Shuffle() 
        {
            IList<Track> shuffledList = tracks_collection.OrderBy(i => Guid.NewGuid()).ToList();
            tracks_collection = shuffledList;
        }
        #endregion

        #region ArtistsSettings
        public void SetArtist(Artist artist) =>
            artist.AddPlaylist(this);
        
        public void SetArtists(IEnumerable<Artist> newArtists)
        {
            foreach (var artist in newArtists)
            {
                artist.AddPlaylist(this);
            }
        }
        #endregion

        #region SpecialMethods

        public IDictionary<string, Track> CraftTableFromTracklist()
        {
            if (tracks_collection.Count > 0)
            {
                foreach (Track item in tracks_collection)
                {
                    playlist_table.Add(item.Name, item);
                }
            }

            return playlist_table;
        }

        public override string ToString() => $"{name}";

        public void Dispose()
        {
            id = default;
            name= default;
            head = default;
            tail= default;
            tracks_collection = default;
        }
        #endregion
       
    }
}
