using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.Core.Services.Entities;
public sealed class SupportGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService".AsMemory();

    private static ICube cube;

    public IEnumerable<Artist> ArtistsCollection => cube.Artists ?? null;
    public IEnumerable<Playlist> PlaylistsCollection => cube.Playlists??null;
    public IEnumerable<Track> TracksCollection => cube.Tracks?? null;

    public event Action OnArtistsNotifyRefresh;
    public event Action OnPlaylistsNotifyRefresh;
    public event Action OnTracksNotifyRefresh;


    public SupportGhost()
    {}


    //Initialize and start Synch Area instance 
    //make sure that you initialized your cube instance
    public void Init(ICube inputCube) 
    {
        cube = inputCube;
    } 

    public void AddArtistInstance(Artist artist)
    {
        cube.AddArtistObj(artist);
        OnArtistsNotifyRefresh?.Invoke();
    }

    public void AddPlaylistInstance(Playlist playlist)
    {
        cube.AddPlaylistObj(playlist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void AddTrackInstance(Track track)
    {
        cube.AddTrackObj(track);
        OnTracksNotifyRefresh?.Invoke();
    }


    public void EditArtistInstance(Artist newArtist)
    {
        cube.EditArtistObj(newArtist);
        OnArtistsNotifyRefresh?.Invoke();
    }
    
    public void EditPlaylistInstance(Playlist newPlaylist)
    {
        cube.EditPlaylistObj(newPlaylist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }
    
    public void EditTrackInstance(Track newTrack)
    {
        cube.EditTrackObj(newTrack);
        OnTracksNotifyRefresh?.Invoke();  
    }


    public void DeleteArtistInstance(Guid artistId) 
    {
        cube.RemoveArtistObj(artistId);
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Guid playlistId) 
    {
        cube.RemovePlaylistObj(playlistId);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Guid trackId)
    {
        cube.RemoveTrackObj(trackId);
        OnArtistsNotifyRefresh?.Invoke();
    }
       
    //these methods get dto notations about requred instances
    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag)
    {
        return await cube.RequireInstances(entityTag);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag,
                                                                       IEnumerable<Guid> id)
    {
        return await cube.RequireInstances(entityTag, id);
    }

    public async Task<Artist> FetchArtist(Guid artistId)
    {
        return await cube.FetchArtist(artistId);
    }

    public async Task<Playlist> FetchPlaylist(Guid playlistId)
    {
        return await cube.FetchPlaylist(playlistId);
    }

    public async Task<Track> FetchTrack(Guid trackId)
    {
        return await cube.FetchTrack(trackId);
    }

}
