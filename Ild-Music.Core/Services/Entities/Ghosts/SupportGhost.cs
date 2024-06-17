using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Ild_Music.Core.Instances.Pagging;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;

namespace Ild_Music.Core.Services.Entities;

public sealed class SupportGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "SupporterService".AsMemory();

    private static ICube _cube;
    
    private static MetaData _metaData = new();

    public SupportGhost()
    {}

    public IEnumerable<CommonInstanceDTO>? ArtistsCollection => _cube.Artists;
    public IEnumerable<CommonInstanceDTO>? PlaylistsCollection => _cube.Playlists;
    public IEnumerable<CommonInstanceDTO>? TracksCollection => _cube.Tracks;
    public IEnumerable<CommonInstanceDTO> TagsCollection => _cube.Tags;

    public event Action OnArtistsNotifyRefresh;
    public event Action OnPlaylistsNotifyRefresh;
    public event Action OnTracksNotifyRefresh;
    public event Action OnTagsNotifyRefresh;
   
    public void Init(ICube inputCube) 
    {
        _cube = inputCube;
        _cube.LoadStartEntities();
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

    public void AddTagInstance(Tag tag)
    {
        _cube.AddTagObj(tag);
        OnTagsNotifyRefresh?.Invoke();
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
    
    public void EditTagInstance(Tag tag)
    {
        _cube.EditTagObj(tag);
        OnTagsNotifyRefresh?.Invoke();
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
       
    public void DeleteTagInstance(Guid tagId)
    {
        _cube.RemoveTagObj(tagId);
        OnTagsNotifyRefresh?.Invoke();
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

    public MetaData GetPageMetaData()
    {
        return _metaData;
    }

    public void ResolveMetaData(
        int startPage,
        int pageSize,
        EntityTag entityTag)
    {
        const int emptyCollection = 0;

        _metaData.CurrentPage = startPage;
        _metaData.PageSize = pageSize;
        _metaData.EntityTag = entityTag;
        _metaData.TotalCount = entityTag switch
        {
            EntityTag.ARTIST => ArtistsCollection?.Count() ?? emptyCollection,
            EntityTag.PLAYLIST => PlaylistsCollection?.Count() ?? emptyCollection,
            EntityTag.TRACK => TracksCollection?.Count() ?? emptyCollection,
            EntityTag.TAG => TagsCollection?.Count() ?? emptyCollection,
            _ => 0
        };
    }
    
    public void PageBack()
    {
        if (_metaData.TotalCount == 0)
            return;

        if (_metaData.HasPrevious)
            _metaData.CurrentPage--;
    }

    public void PageForward()
    {
        if (_metaData.TotalCount == 0) 
            return;

        if (_metaData.HasNext)
            _metaData.CurrentPage++;
    }
    
    public async Task<IEnumerable<CommonInstanceDTO>> GetCurrentPage()
    {
        int offset = (_metaData.CurrentPage * _metaData.PageSize);
        return await _cube.LoadFramedEntities(_metaData.EntityTag, offset, _metaData.PageSize);
    }
    
    public async Task<IEnumerable<CommonInstanceDTO>> GetPage(int inputPage)
    {
        if (inputPage < 0 && inputPage > _metaData.TotalPages)
            return null;

        int offset = (inputPage * _metaData.PageSize); 
        return await _cube.LoadFramedEntities(_metaData.EntityTag, offset, _metaData.PageSize);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> Search(string searchQuery)
    {
        return await _cube.Search(searchQuery);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> SearchInstance(string searchQuery, EntityTag entityTag)
    {
        return await _cube.SearchInstance(searchQuery, entityTag);
    }

    public async Task<IEnumerable<CommonInstanceDTO>> SearchTag(string searchQuery)
    {
        return await _cube.SearchTag(searchQuery);
    }
}
