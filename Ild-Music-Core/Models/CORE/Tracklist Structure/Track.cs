using Ild_Music_CORE.Models.Interfaces;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ild_Music_CORE.Models.Core.Tracklist_Structure
{
    public class Track : ResourceRoot, IDisposable, ITrackable, IDescriptional
    {
        #region Fields
        private string id = Guid.NewGuid().ToString();
        private string pathway; //1
        private string name;    //2
        private TimeSpan duration;//3
        private Track nextTrack;
        private Track previousTrack;
        #endregion

        #region Properties
        public override string Id
        {
            get { return id; }
            set { id = value; }
        }
        string ITrackable.id => this.id;
        public string Pathway
        {
            get { return this.pathway; }
            set { pathway = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public Track NextTrack 
        { 
            get { return this.nextTrack; }
            set { this.nextTrack = value; } 
        }
        public Track PreviousTrack 
        { 
            get { return this.previousTrack; }
            set { this.previousTrack = value; }
        }

        public string Description { get; set; }
        #endregion


        #region Ctors
        public Track()
        {
        }
        public Track(string pathway, string? name = null ,string? description = null)
        {
            this.pathway = pathway.Replace("[",string.Empty);

            this.name = name ??  DefineName();

            this.Description = description ?? string.Empty;
            
            CutDuration();
            CutName();
        }
        #endregion

        #region Methods
        private void CutName() 
        {
            if (File.Exists(pathway)) 
            {
                var file = new FileInfo(pathway);
                name = file.Name;
            }
        }

        private string DefineName()
        {
            if (File.Exists(pathway))
            {
                var file = new FileInfo(pathway);
                return file.Name;
            }

            return string.Empty;
        }

        public string GetId()
        {
            return this.id;
        }

        public void Dispose()
        {
            this.id = null;
            this.pathway = null;
            this.name = null;
            nextTrack.PreviousTrack = PreviousTrack;
            previousTrack.nextTrack = NextTrack;
        }

        public void SetArtist(Artist newArtist)
        {
            newArtist.AddTrack(this);
        }

        public void SetArtists(IEnumerable<Artist> newArtists)
        {
            foreach (Artist newArtist in newArtists)
            {
                newArtist.AddTrack(this);
            }
        }

        private void CutDuration() 
        {
            if (File.Exists(pathway))
            {
                if (new FileInfo(pathway).Extension == ".mp3")
                {
                    var mp3Scrapper = new Mp3FileReader(pathway);
                    duration = mp3Scrapper.TotalTime;
                }
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
