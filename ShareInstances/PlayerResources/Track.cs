using System;
using System.Collections.Generic;
using System.IO;

namespace ShareInstances.PlayerResources
{
    public class Track : ResourceRoot, IDisposable, IDescriptional
    {
        #region Fields
        private string id = Guid.NewGuid().ToString();
        private string pathway; 
        private string name;
        private string description;
        private TimeSpan duration;
        private Track nextTrack;
        private Track previousTrack;
        #endregion

        #region Properties
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Pathway
        {
            get { return pathway; }
            set { pathway = value; }
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

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public Track NextTrack 
        { 
            get { return nextTrack; }
            set { nextTrack = value; } 
        }
        public Track PreviousTrack 
        { 
            get { return previousTrack; }
            set { previousTrack = value; }
        }        
        #endregion

        #region Ctors
        public Track()
        {
        }

        public Track(string pathway, string? name = null ,string? description = null)
        {
            this.pathway = pathway;

            DefineName();

            this.Description = description ?? string.Empty;
            
            ExtractDuration();
        }
        #endregion

        #region Id
        public string GetId() => id;
        #endregion

        #region Name
        private void DefineName()
        {
            if (File.Exists(pathway))
            {
                var file = new FileInfo(pathway);
                name = file.Name;
            }            
        }
        #endregion

        #region Duration
        private void ExtractDuration()
        {
            if (File.Exists(pathway))
            {
                if (new FileInfo(pathway).Extension == ".mp3")
                {
                    var taglib = TagLib.File.Create(pathway);
                    duration = taglib.Properties.Duration;
                }
            }
        }
        #endregion

        #region ArtistsSettings
        public void SetArtist(Artist artist) =>
            artist.AddTrack(this);
        
        public void SetArtists(IEnumerable<Artist> artists)
        {
            foreach (Artist artist in artists)
            {
                artist.AddTrack(this);
            }
        }
        #endregion

        #region Specials
        public override string ToString() => $"{name}";

        public void Dispose()
        {
            this.id = null;
            this.pathway = null;
            this.name = null;
            nextTrack.PreviousTrack = PreviousTrack;
            previousTrack.nextTrack = NextTrack;
        }
        #endregion
    }
}
