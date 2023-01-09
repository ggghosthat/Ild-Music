using ShareInstances.PlayerResources;
using ShareInstances.Services.Entities;
using ShareInstances.Services.Interfaces;
using ShareInstances.Services.InstanceProducer;
using ShareInstances.Exceptions.SynchAreaExceptions;

using System;
using System.Collections.Generic;

namespace ShareInstances.Services.Entities
{
    public class FactoryService : IService
    {
        public string ServiceName {get; init;} = "FactoryService";        

        public SupporterService SupporterService {get; set;}
        private InstanceProducer.InstanceProducer producer = default;
        

        #region Public Methods
        public void CreateArtist(string name, string description)
        {
            try
            {
                producer = new InstanceProducer.InstanceProducer(name, description);
                SupporterService.AddInstance(producer.ArtistInstance);
                producer.Dispose();
            }
            catch (InvalidArtistException ex)
            {
                throw ex;
            }
        }
            

        public void CreatePlaylist(string name, string description, IList<Track> tracks = null, IList<Artist> artists = null)
        {   
            try
            {
                producer = new InstanceProducer.InstanceProducer(name, description, tracks, artists);
                SupporterService.AddInstance(producer.PlaylistInstance);
                producer.Dispose();
            }
            catch (InvalidPlaylistException ex)
            {
                throw ex;
            }
        }

        public void CreateTrack(string pathway, string name, string description, IList<Artist> artists = null)
        {      
            try
            {      
                producer = new InstanceProducer.InstanceProducer(pathway, name, description, artists);
                SupporterService.AddInstance(producer.TrackInstance);
                producer.Dispose();
            }
            catch (InvalidTrackException ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}