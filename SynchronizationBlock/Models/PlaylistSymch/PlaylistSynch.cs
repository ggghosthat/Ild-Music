using ShareInstances.PlayerResources;
using System;
using System.Collections.Generic;
using System.IO;
using SynchronizationBlock.Models.SynchArea;
using Newtonsoft.Json;
using System.Linq;

namespace SynchronizationBlock.Models.PlaylistSymch
{
    public class PlaylistSynch<T> : SynchBase<T> where T : Playlist
    {
        private string output_pathway = Environment.CurrentDirectory + "/ild_music_playlists.json";

        //determines a collection of playlists abstracly
        IList<Playlist> playlists = new List<Playlist>();


        public override IList<T> Instances => (IList<T>)playlists;
        public string Prefix
        {
            get => Path;
            
            set
            {
                Path = value;
                output_pathway = Path + "/ild_music_playlists.json";
            }
        }



        public override void AddInstance(T playlist) =>        
            playlists.Add(playlist);
        

        public override void EditInstance(T playlist_update)
        {
            var playlist = playlists.First(a => a.Id.Equals(playlist_update.Id));
            var index = playlists.IndexOf(playlist);
            playlists[index] = playlist_update;
        }

        public override void RemoveInstance(T tracklist)
        {
            if (playlists.Contains(tracklist))
                playlists.Remove(tracklist);
        }



        public override void Serialize()
        {
            try
            {
                string jsonString = JsonConvert.SerializeObject(this.playlists);
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
                playlists = JsonConvert.DeserializeObject<List<Playlist>>(jsonString);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
