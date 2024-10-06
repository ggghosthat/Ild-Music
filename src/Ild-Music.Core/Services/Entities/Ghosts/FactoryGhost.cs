using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Exceptions.CubeExceptions;
using Ild_Music.Core.Helpers;

namespace Ild_Music.Core.Services.Entities;

public sealed class FactoryGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "FactoryGhost".AsMemory(); 

    private static IRepository _repository;
    private static InstanceProducer producer = default;
    
    public event Action OnArtistUpdate;
    public event Action OnPlaylistUpdate;
    public event Action OnTrackUpdate;
    public event Action OnTagUpdate;
    
    public FactoryGhost()
    {}

    public void Init(IRepository inputCube)
    {
       _repository = inputCube;
    }

    public void CreateArtist(string name, string description = default!, string avatarPath = default!, int year = 0)
    {
        try
        {
            var artist = FactoryHelper.CreateArtist(name, description, avatarPath, year);
            _repository.AddArtistObj(artist);
            OnArtistUpdate?.Invoke();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }
        
    public void CreateArtist(string name, string description, string avatarPath, int year, out Artist artist)
    {
        try
        {
            
            artist = FactoryHelper.CreateArtist(name, description, avatarPath, year);
            _repository.AddArtistObj(artist);
            OnArtistUpdate?.Invoke();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }

    public void CreatePlaylist(string name, string description = default!, string avatarPath = default!, int year = 0, IList<Track> tracks = default!, IList<Artist> artists = default!) 
    {   
        try 
        { 
            var playlist = FactoryHelper.CreatePlaylist(name, description, avatarPath, year, tracks, artists);
            _repository.AddPlaylistObj(playlist);
            OnPlaylistUpdate?.Invoke();
        } 
        catch (InvalidPlaylistException ex) 
        { 
            throw ex;
        } 
    }

    public void CreatePlaylist(string name, string description, string avatarPath, int year, IList<Track> tracks, IList<Artist> artists, out Playlist playlist) 
    {   
        try 
        { 
            playlist = FactoryHelper.CreatePlaylist(name, description, avatarPath, year, tracks, artists);
            _repository.AddPlaylistObj(playlist);
            OnPlaylistUpdate?.Invoke();
        } 
        catch (InvalidPlaylistException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTrack(string pathway, string name = default!, string description = default!, string avatarPath = default!, int year = 0, IList<Artist> artists = default!) 
    {      
        try 
        {
            var track = FactoryHelper.CreateTrack(pathway, name, description, avatarPath, year, artists);
            _repository.AddTrackObj(producer.TrackInstance);
            OnTrackUpdate?.Invoke();
        } 
        catch (InvalidTrackException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTrack(string pathway,string name,string description, string avatarPath, int year, IList<Artist> artists, out Track track)
    {      
        try 
        {               
            track = FactoryHelper.CreateTrack(pathway, name, description, avatarPath, year, artists);
            _repository.AddTrackObj(producer.TrackInstance);
            OnTrackUpdate?.Invoke();
        } 
        catch (InvalidTrackException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTag(string name,string color)
    {
        var tag = FactoryHelper.CreateTag(name, color);
        _repository.AddTagObj(tag);
    }

    public void CreateTag(string name, string color, out Tag tag)
    {
        tag = FactoryHelper.CreateTag(name, color);
        _repository.AddTagObj(tag);
    }
    
    public Track CreateTrackBrowsed(string pathway, bool allocateInstance = false)
    {      
        try 
        {        
            var track = FactoryHelper.CreateTrack(pathway);
            
            if (allocateInstance)
                _repository.AddTrackObj(track).Wait();

            return track; 
        } 
        catch (InvalidTrackException ex)
        {
            throw ex; 
        } 
    } 

    public Playlist CreateTemporaryPlaylist(IList<Track> tracks) 
    {      
        try 
        {  
            _repository.RegisterBrowsedTracks(tracks);
            string playlistName = $"Temporary playlist {DateTime.Now}";
            return FactoryHelper.CreatePlaylist(playlistName, String.Empty, String.Empty, DateTime.Now.Year, tracks, null);
        } 
        catch (InvalidTrackException ex)
        {
            throw ex; 
        } 
    }

    public void ClearBrowsedTracks()
    {
        if (_repository.BrowsedTracks.Count() > 0)
            _repository.EraseBrowsedTracks();
    }
}
