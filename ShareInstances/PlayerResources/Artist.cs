using System;
using System.Collections.Generic;
using ShareInstances.PlayerResources.Base;
using ShareInstances.PlayerResources.Interfaces;

namespace ShareInstances.PlayerResources
{
    public class Artist : ResourceRoot, IDescriptional, IDisposable
    {
        #region Fields
        private Guid id = Guid.NewGuid();
        private string name;
        private string description;

        private IList<Guid> tracks = new List<Guid>();
        private IList<Guid> playlists = new List<Guid>();
        private IList<string> tracksId_collection;
        #endregion

        #region Properties
        public override Guid Id
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

        public IList<Guid> Playlists 
        {
            get { return playlists; } 
            set { playlists = value; } 
        }
        public IList<Guid> Tracks 
        {
            get
            {

             return tracks;
            }
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
        public void AddTrack(Track track)
        {
            if (!tracks.Contains(track.Id))      
                tracks.Add(track.Id);
        }
        public void AddPlaylist(Playlist playlistItem)
        {
            if (!playlists.Contains(playlistItem.Id))  
                playlists.Add(playlistItem.Id);
        }
        #endregion

        #region DeleteResourcesMethods
        public void DeleteTrack(Track track)
        {
            if (tracks.Contains(track.Id))
                tracks.Remove(track.Id);
        }

        public void DeletePlaylist(Playlist playlistItem)
        {     
            if (playlists.Contains(playlistItem.Id))  
                playlists.Remove(playlistItem.Id);
        }
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
