using System;
using System.Collections.Generic;
using ShareInstances.PlayerResources.Base;
using ShareInstances.PlayerResources.Interfaces;


namespace ShareInstances.PlayerResources
{
    public class Artist : ResourceRoot, IDescriptional, IDisposable
    {
        #region Fields
        private string id = Guid.NewGuid().ToString();
        private string name;
        private string description;

        private IList<Track> tracks = new List<Track>();
        private IList<Playlist> playlists = new List<Playlist>();
        private IList<string> tracksId_collection;
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

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public IList<Playlist> Tracks 
        {
            get { return playlists; } 
            set { playlists = value; } 
        }
        public IList<Track> TracksCollection {
            get{ return tracks;}
            set{ tracks = value;}
        }
        public IList<string> TracksIdCollection 
        {
            get{return tracksId_collection;}
            set{tracksId_collection = value;}
        }

        
        #endregion

        #region contr

        public Artist()
        {
        }

        public Artist(string name, string? description = null)
        {
            this.name = name;
            this.description = description ?? string.Empty;
        }          
        #endregion

        #region AddResourcesMethods
        public void AddTrack(Track track) =>        
            tracks.Add(track);
        
        public void AddTracklist(Playlist tracklist) =>        
            playlists.Add(tracklist);
        #endregion

        #region SpecialMethods
        public override string ToString() => $"{name}";
        
        public void Dispose()
        {
            id = default;
            name = default;
            tracksId_collection = default;
            tracks = default;
            playlists = default;
        }
        #endregion
    }
}
