using ShareInstances.PlayerResources;
using Newtonsoft.Json;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynchronizationBlock.Models.TrackSynch
{
    public class TrackSynch<T> : SynchBase<T> where T : Track
    {
        //<PSTF> -> Plain Single Track File

        protected string output_pathway = Environment.CurrentDirectory + "/service_tracks.json";
        private string log;


        //determines a collection of tracks (music files) abstracly
        private IList<Track> tracks;
        public string Prefix 
        {
            get 
            {
                return Path;
            }
            set 
            {
                Path = value;
                output_pathway = Path + "/service_tracks.json";
            }
        }

        public override IList<T> Instances =>  (IList<T>)tracks;




        public TrackSynch()
        {
            tracks = new List<Track>();
        }





        public override void AddInstance(T track)
        {
            if (tracks == null)
                tracks = new List<Track>();
            tracks.Add(track);
        }

        public override void EditInstance(T need_track)
        {
            var track = this.tracks.First(a => a.Id.Equals(need_track.Id));
            var index = this.tracks.IndexOf(track);
            this.tracks[index] = need_track;
        }


        public override void RemoveInstance(T track)
        {
            if(tracks.Contains(track))
                tracks.Remove(track);
        }


        //this method search a track(music file) in directory
        public void InitPSTF(string path)
        {
            if (File.Exists(path)) 
            {
                var track = new Track(path);
                tracks.Add(track);
            }
        }


        public override void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(this.tracks);
                File.WriteAllText(output_pathway, string.Empty);
                File.WriteAllText(output_pathway, jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void Deserialize()
        {
            if (File.Exists(output_pathway))
            {
                try
                {
                    string jsonString = File.ReadAllText(output_pathway);
                    tracks = JsonConvert.DeserializeObject<List<Track>>(jsonString);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
