using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Newtonsoft.Json;
using SynchronizationBlock.Models.SynchArea;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SynchronizationBlock.Models.ArtistSynch
{
    public class ArtistSynch<T> : SynchBase<T> where T : Artist
    {

        private IList<Artist> artists;

        private string output_pathway = Environment.CurrentDirectory + "/service_artists.json";

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
                output_pathway = Path + "/service_artists.json";
            }
        }


        public ArtistSynch()
        {
            artists = new List<Artist>();
        }

        //add artist into collection
        public override void AddInstance(T artist)
        {
            artists.Add(artist);
        }

        //modify artists state
        public override void EditInstance(T need_artist)
        {
            var artist = this.artists.First(a => a.Id.Equals(need_artist.Id));
            var index = artists.IndexOf(artist);
            this.artists[index] = need_artist;
        }

        //remove artist from collection
        public override void RemoveInstance(T artist)
        {
            if (artists.Contains(artist))
                artists.Remove(artist);
        }


        public override void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(this.artists);
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
                    this.artists = JsonConvert.DeserializeObject<List<Artist>>(jsonString);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
