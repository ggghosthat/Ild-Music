using System;
using System.Threading.Tasks;

namespace ShareInstances
{
    //Represent Synchronization block instance
    public interface ISynchArea
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }


        //public void AddArtistObj(Artist artist);
    }

    //Represent Player instance
    public interface IPlayer
    {
        public Guid PlayerId { get; }
        public string PlayerName { get; }

        public Task Play();

        public Task StopPlayer();

        public Task Pause_ResumePlayer();

        public Task ShuffleTrackCollection();

        public Task ChangeVolume(float volume);
    }
}
