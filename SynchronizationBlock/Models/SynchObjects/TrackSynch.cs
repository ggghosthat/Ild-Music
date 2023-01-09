using ShareInstances.PlayerResources;
using ShareInstances.Exceptions.SynchAreaExceptions;

using Newtonsoft.Json;
using SynchronizationBlock.Models.SynchObjects.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynchronizationBlock.Models.SynchObjects
{
    public class TrackSynch
    {
        //<PSTF> -> Plain Single Track File

        protected string output_pathway = Environment.CurrentDirectory + "/ild_music_tracks.json";


        //determines a collection of tracks (music files) abstracly
        private IList<Track> tracks = new List<Track>();

        private string path;
        public string Prefix 
        {
            get => path;
            
            set 
            {
                path = value;
                output_pathway = path + "/ild_music_tracks.json";
            }
        }

        public IList<Track> Instances =>  tracks;



        public void AddInstance(Track track) 
        {
            if(tracks.Where(t => t.Pathway.Equals(track.Pathway)).Count() > 0)
                throw new InvalidTrackException("Unable create new track instance");

            tracks.Add(track);
        }
        
        public void EditInstance(Track need_track)
        {
            try
            {
                tracks.Where(t => t.Id.Equals(need_track.Id))
                       .ToList().ForEach(t => t = need_track);
            }
            catch(Exception ex)
            {
                throw new Exception("This is Edit instances exception happened in synch layer.");
            }

            // var track = tracks.First(a => a.Id.Equals(need_track.Id));
            // var index = tracks.IndexOf(track);
            // tracks[index] = need_track;
        }

        public void RemoveInstance(Track track)
        {
            if(tracks.Contains(track))
                tracks.Remove(track);
        }


        public void Serialize()
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

        public void Deserialize()
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

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
