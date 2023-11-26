using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services;
using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.Core.Services.Entities;
public sealed class SupportGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService".AsMemory();

    //temp solution
    public static ICube CubeArea;

    public IEnumerable<Artist> ArtistsCollection => CubeArea.Artists;
    public IEnumerable<Playlist> PlaylistsCollection => CubeArea.Playlists;
    public IEnumerable<Track> TracksCollection => CubeArea.Tracks;

    public event Action OnArtistsNotifyRefresh = null;
    public event Action OnPlaylistsNotifyRefresh = null;
    public event Action OnTracksNotifyRefresh = null;


    public SupportGhost(IPluginBag pluginBag)
    {
        CubeArea = pluginBag.GetCurrentCube();
        CubeArea.Init();
    }

    //Initialize and start Synch Area instance 
    public void Init(ref ICube syncCube) 
    {
        CubeArea = syncCube;
        CubeArea.Init();
    }
    
    

    public void AddArtistInstance(Artist artist)
    {
        CubeArea.AddArtistObj(artist);
        OnArtistsNotifyRefresh?.Invoke();
    }

    public void AddPlaylistInstance(Playlist playlist)
    {
        CubeArea.AddPlaylistObj(playlist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void AddTrackInstance(Track track)
    {
        CubeArea.AddTrackObj(track);
        OnTracksNotifyRefresh?.Invoke();
    }


    public void EditArtistInstance(Artist artist)
    {
        CubeArea.EditArtistObj(artist);
        OnArtistsNotifyRefresh?.Invoke();
    }
    
    public void EditPlaylistInstance(Playlist playlist)
    {
        CubeArea.EditPlaylistObj(playlist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }
    
    public void EditTrackInstance(Track track)
    {
        CubeArea.EditTrackObj(track);
        OnTracksNotifyRefresh?.Invoke();  
    }


    public void DeleteArtistInstance(Guid artistId) 
    {
        CubeArea.RemoveArtistObj(artistId);
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Guid playlistId) 
    {
        CubeArea.RemovePlaylistObj(playlistId);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Guid trackId)
    {
        CubeArea.RemoveTrackObj(trackId);
        OnArtistsNotifyRefresh?.Invoke();
    }
       
    //these methods get dto notations about requred instances
    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag)
    {
        return await CubeArea.RequireInstances(entityTag);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> RequireInstances(EntityTag entityTag,
                                                                       IEnumerable<Guid> id)
    {
        return await CubeArea.RequireInstances(entityTag, id);
    }

}
