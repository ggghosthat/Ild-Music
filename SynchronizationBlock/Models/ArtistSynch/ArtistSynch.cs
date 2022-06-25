using ShareInstances.PlayerResources;
using Newtonsoft.Json;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace SynchronizationBlock.Models.ArtistSynch
{
    public class ArtistSynch<T> : SynchBase<T> where T : Artist
    {
        private string output_pathway = Environment.CurrentDirectory + "/ild_music_artists.json";

        private IList<Artist> artists = new List<Artist>();
        
        public override IList<T> Instances => (IList<T>)artists;

        public string Prefix
        {
            get
            {
                return Path;
            }
            set
            {
                Path = value;
                output_pathway = Path + "/ild_music_artists.json";
            }
        }



        public override void AddInstance(T artist) =>
            artists.Add(artist);
                
        public override void EditInstance(T need_artist)
        {
            var artist = artists.First(a => a.Id.Equals(need_artist.Id));
            var index = artists.IndexOf(artist);
            artists[index] = need_artist;
        }

        public override void RemoveInstance(T artist)
        {
            if (artists.Contains(artist))
                artists.Remove(artist);
        }


        public override void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(artists);
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
                artists = JsonConvert.DeserializeObject<List<Artist>>(jsonString);
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
