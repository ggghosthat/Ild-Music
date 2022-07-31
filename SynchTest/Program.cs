using System;
using ShareInstances;
using ShareInstances.PlayerResources;
using SynchronizationBlock.Models.SynchArea;
using System.Collections.ObjectModel;

namespace SynchTest
{
    class Program
    {
        private static Area SynchArea = new();
        private static Artist artist1 = new Artist("Eminem","");
        private static Artist artist2 = new Artist("Snoop Dog","");
        private static Artist artist3 = new Artist("Dr. Dre","");

        static void Main(string[] args)
        {
            // System.Console.WriteLine(SynchArea.existedArtists.Count);
            SynchArea.AddArtistObj(artist1);
            // System.Console.WriteLine(SynchArea.existedArtists.Count);
            // SynchArea.AddArtistObj(artist2);
            SynchArea.AddArtistObj(artist3);
            // System.Console.WriteLine(SynchArea.existedArtists.Count);
            SynchArea.RemoveArtistObj(artist2);
            // System.Console.WriteLine(SynchArea.existedArtists.Count);

            ObservableCollection<Artist> artists = new(SynchArea.existedArtists);
            System.Console.WriteLine(artists.Count);
            SynchArea.AddArtistObj(artist2);
            System.Console.WriteLine(artists.Count);            
            SynchArea.AddArtistObj(artist2);
            System.Console.WriteLine(artists.Count);
        }
    }
}
