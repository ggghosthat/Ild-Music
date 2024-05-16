using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.PagedList;

namespace Ild_Music.Core.Services.Entities;

public sealed class SupportGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService".AsMemory();

    private static ICube _cube;
    
    private static MetaData _metaData = new();

    public IEnumerable<CommonInstanceDTO>? ArtistsCollection => _cube.Artists ?? null;
    public IEnumerable<CommonInstanceDTO>? PlaylistsCollection => _cube.Playlists ?? null;
    public IEnumerable<CommonInstanceDTO>? TracksCollection => _cube.Tracks ?? null;
    public IEnumerable<Tag> TagsCollection => _cube.Tags ?? null;

    public event Action OnArtistsNotifyRefresh;
    public event Action OnPlaylistsNotifyRefresh;
    public event Action OnTracksNotifyRefresh;
    public event Action OnTagsNotifyRefresh;

    public SupportGhost()
    {}
   
    public void Init(ICube inputCube) 
    {
        _cube = inputCube;
    }

    public void AddArtistInstance(Artist artist)
    {
        _cube.AddArtistObj(artist);
        OnArtistsNotifyRefresh?.Invoke();
    }

    public void AddPlaylistInstance(Playlist playlist)
    {
        _cube.AddPlaylistObj(playlist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void AddTrackInstance(Track track)
    {
        _cube.AddTrackObj(track);
        OnTracksNotifyRefresh?.Invoke();
    }

    public void EditArtistInstance(Artist newArtist)
    {
        _cube.EditArtistObj(newArtist);
        OnArtistsNotifyRefresh?.Invoke();
    }
    
    public void EditPlaylistInstance(Playlist newPlaylist)
    {
        _cube.EditPlaylistObj(newPlaylist);        
        OnPlaylistsNotifyRefresh?.Invoke();
    }
    
    public void EditTrackInstance(Track newTrack)
    {
        _cube.EditTrackObj(newTrack);
        OnTracksNotifyRefresh?.Invoke();  
    }

    public void DeleteArtistInstance(Guid artistId) 
    {
        _cube.RemoveArtistObj(artistId);
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Guid playlistId) 
    {
        _cube.RemovePlaylistObj(playlistId);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Guid trackId)
    {
        _cube.RemoveTrackObj(trackId);
        OnArtistsNotifyRefresh?.Invoke();
    }
       
    public async Task<Artist> GetArtistAsync(CommonInstanceDTO instanceDTO)
    {
        return await _cube.QueryArtist(instanceDTO);
    }

    public async Task<Playlist> GetPlaylistAsync(CommonInstanceDTO instanceDTO)
    {
        return await _cube.QueryPlaylist(instanceDTO);
    }

    public async Task<Track> GetTrackAsync(CommonInstanceDTO instanceDTO)
    {
        return await _cube.QueryTrack(instanceDTO);
    }

    public async Task<Tag> GetTagAsync(Guid tagId)
    {
        return await _cube.QueryTag(tagId);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> GetInstanceDTOsFromIds(IEnumerable<Guid> ids, EntityTag entityTag)
    {
        return await _cube.QueryInstanceDtosFromIds(ids, entityTag);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> PageBack(EntityTag entityTag)
    {
        _metaData.TotalCount = entityTag switch
        {
            EntityTag.ARTIST => ArtistsCollection?.Count(),
            EntityTag.PLAYLIST => PlaylistsCollection?.Count(),
            EntityTag.TRACK => TracksCollection?.Count(),
            EntityTag.TAG => TagsCollection?.Count()
        } ?? 0;

        if (_metaData.HasPrevious)
            _metaData.CurrentPage--;

        int offset = (_metaData.CurrentPage * _metaData.PageSize);
        return await _cube.LoadFramedEntities(entityTag, offset, _metaData.PageSize);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> PageForward(EntityTag entityTag)
    {
        _metaData.TotalCount = entityTag switch
        {
            EntityTag.ARTIST => ArtistsCollection?.Count(),
            EntityTag.PLAYLIST => PlaylistsCollection?.Count(),
            EntityTag.TRACK => TracksCollection?.Count(),
            EntityTag.TAG => TagsCollection?.Count()
        } ?? 0;

        if (_metaData.HasNext)
            _metaData.CurrentPage++;

        int offset = (_metaData.CurrentPage * _metaData.PageSize);
        return await _cube.LoadFramedEntities(entityTag, offset, _metaData.PageSize);
    }
}
