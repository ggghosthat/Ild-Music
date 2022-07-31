using ShareInstances.PlayerResources;
using Newtonsoft.Json;
using SynchronizationBlock.Models.SynchObjects.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynchronizationBlock.Models.SynchObjects
{
    public class TrackSynch<T> : SynchBase<T> where T : Track
    {
        //<PSTF> -> Plain Single Track File

        protected string output_pathway = Environment.CurrentDirectory + "/ild_music_tracks.json";


        //determines a collection of tracks (music files) abstracly
        private IList<Track> tracks = new List<Track>();
        public string Prefix 
        {
            get => Path;
            
            set 
            {
                Path = value;
                output_pathway = Path + "/ild_music_tracks.json";
            }
        }

        public override IList<T> Instances =>  (IList<T>)tracks;



        public override void AddInstance(T track) =>
            tracks.Add(track);
        
        public override void EditInstance(T need_track)
        {
            var track = tracks.First(a => a.Id.Equals(need_track.Id));
            var index = tracks.IndexOf(track);
            tracks[index] = need_track;
        }

        public override void RemoveInstance(T track)
        {
            if(tracks.Contains(track))
                tracks.Remove(track);
        }


        public override void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(tracks);
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
            try
            {
                string jsonString = File.ReadAllText(output_pathway);
                tracks = JsonConvert.DeserializeObject<List<Track>>(jsonString);
            }
            catch (FileNotFoundException fileNotFound)
            {

            }
            catch (Exception)
            {
                throw;
            }
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }
    }
}
