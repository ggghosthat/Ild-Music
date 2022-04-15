using System;
using System.Collections.Generic;
using System.IO;
using SynchronizationBlock.Models.SynchArea;
using Ild_Music_CORE.Models.Core.Tracklist_Structure;
using Newtonsoft.Json;
using System.Linq;

namespace SynchronizationBlock.Models.PlaylistSymch
{
    public class PlaylistSynch<T> : SynchBase<T> where T : Tracklist
    {
        private string output_pathway = Environment.CurrentDirectory + "/service_list.json";
        private string log;

        //determines a collection of playlists abstracly
        IList<Tracklist> playlists;


        public override IList<T> Instances => (IList<T>)playlists;
        public string Prefix
        {
            get
            {
                return Path;
            }
            set
            {
                Path = value;
                output_pathway = Path + "/service_list.json";
            }
        }


        public PlaylistSynch()
        {
            playlists = new List<Tracklist>();
        }




        public override void AddInstance(T tracklist)
        {
            if(playlists == null)
                playlists = new List<Tracklist>();
            playlists.Add(tracklist);
        }

        public override void EditInstance(T need_playlist)
        {
            var playlist = this.playlists.First(a => a.Id.Equals(need_playlist.Id));
            var index = this.playlists.IndexOf(playlist);
            this.playlists[index] = playlist;
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
                File.WriteAllText(output_pathway,string.Empty);
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
                    playlists = JsonConvert.DeserializeObject<List<Tracklist>>(jsonString);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
