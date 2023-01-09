using ShareInstances.PlayerResources;
using ShareInstances.Exceptions.SynchAreaExceptions;

using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace SynchronizationBlock.Models.SynchObjects
{
    public class ArtistSynch
    {
        private string output_pathway = Environment.CurrentDirectory + "/ild_music_artists.json";

        private IList<Artist> artists = new List<Artist>();
        
        public IList<Artist> Instances => artists;

        private string path;
        public string Prefix
        {
            get => path;
            
            set
            {
                path = value;
                output_pathway = path + "/ild_music_artists.json";
            }
        }

        public void AddInstance(Artist artist)
        {
            if(artists.Where(a => a.Name.Equals(artist.Name)).Count() > 0)
                throw new InvalidArtistException("Unable create new artist instance");

            artists.Add(artist);
        }
                
        public void EditInstance(Artist need_artist)
        {
            try
            {
                artists.Where(a => a.Id.Equals(need_artist.Id))
                       .ToList().ForEach(a => a = need_artist);                
            }
            catch(Exception ex)
            {
                throw new Exception("This is Edit instances exception happened in synch layer.");
            }

            // var artist = artists.First(a => a.Id.Equals(need_artist.Id));
            // var index = artists.IndexOf(artist);
            // artists[index] = need_artist;
        }

        public void RemoveInstance(Artist artist)
        {
            if (artists.Contains(artist))
                artists.Remove(artist);
        }


        public void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(artists);
                File.WriteAllText(output_pathway, string.Empty);
                File.WriteAllText(output_pathway, jsonString);
            }
            catch (Exception)
            {
                Debug.WriteLine("Hello am exception");
                throw;
            }
        }

        public void Deserialize()
        {
            try
            {
                string jsonString = File.ReadAllText(output_pathway);
                artists = JsonConvert.DeserializeObject<List<Artist>>(jsonString);
            }
            catch(FileNotFoundException fileNotFound)
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
