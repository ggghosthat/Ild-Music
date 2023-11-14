using Ild_Music.Core.Instances;
using Cube.Storage.Guido.Engine;
using Cube.Mapper.Entities;
using Cube.Storage.Guido;
namespace Cube.Storage;
public class GuidoForklift //Cars from pixar (lol)
{
    private int capacity;

    //private static ConcurrentQueue<ReadOnlyMemory<char>> queries = new();
    private static Engine _engine;
    private static Mapper.Mapper _mapper;
    private Voyager voyager;

    public GuidoForklift(in string dbPath,
                        int capacity = 300)
    {
        _engine = new (dbPath, capacity);
        _mapper = new();
        voyager = new (dbPath);


        this.capacity = capacity;
    }

    //check database and table existance
    //in negative case it creates from scratch
    public void ForkliftUp()
    {
        _engine.StartEngine();
    }

    #region CRUD
    //insert new entity
    public async Task AddEntity<T>(T entity)
    {
        if (entity is Artist artist)
        {
           var mappedArtist = _mapper.MakeSnapshot<Artist>(artist);
           await _engine.Add<ArtistMap>((ArtistMap)mappedArtist.Item1);           
           await _engine.AddStores(mappedArtist.Item2);
        }
        else if(entity is Playlist playlist)
        {
           playlist.DumpTracks(); 
           var mappedPlaylist = _mapper.MakeSnapshot<Playlist>(playlist);
           await _engine.Add<PlaylistMap>((PlaylistMap)mappedPlaylist.Item1);           
           await _engine.AddStores(mappedPlaylist.Item2);

        }
        else if(entity is Track track)
        {
           var mappedTrack = _mapper.MakeSnapshot<Track>(track);
           await _engine.Add<TrackMap>((TrackMap)mappedTrack.Item1);           
           await _engine.AddStores(mappedTrack.Item2);

        }
    }

    //update(edit) existed entity
    public async Task EditEntity<T>(T entity)
    {
        if (entity is Artist artist)
        {
           var mappedArtist = _mapper.MakeSnapshot<Artist>(artist);
           await _engine.Edit<ArtistMap>((ArtistMap)mappedArtist.Item1);           
           await _engine.EditStores(mappedArtist.Item2);
        }
        else if(entity is Playlist playlist)
        {
           playlist.DumpTracks();
           var mappedPlaylist = _mapper.MakeSnapshot<Playlist>(playlist);
           await _engine.Edit<PlaylistMap>((PlaylistMap)mappedPlaylist.Item1);           
           await _engine.EditStores(mappedPlaylist.Item2);
        }
        else if(entity is Track track)
        {
           var mappedTrack = _mapper.MakeSnapshot<Track>(track);
           await _engine.Edit<TrackMap>((TrackMap)mappedTrack.Item1);           
           await _engine.EditStores(mappedTrack.Item2);
        }
    }

    //delete specific entity by it own id
    public async Task DeleteEntity<T>(T entity)
    {
        if (entity is Artist artist)
        {
          await _engine.Delete<Artist>(artist); 
        }
        else if(entity is Playlist playlist)
        {
          await _engine.Delete<Playlist>(playlist); 
        }
        else if(entity is Track track)
        {
          await _engine.Delete<Track>(track); 
        }

    }

    public async Task<(IEnumerable<Artist>, IEnumerable<Playlist>, IEnumerable<Track>)> StartLoad(int offset=0)
    {
        var load = await _engine.BringAll(offset:offset, inputCapacity:capacity);

        var artists = await _mapper.GetEntityProjections<ArtistMap, Artist>(load.Item1);
        var playlists = await _mapper.GetEntityProjections<PlaylistMap, Playlist>(load.Item2);
        var tracks = await _mapper.GetEntityProjections<TrackMap, Track>(load.Item3);
        
        return (artists, playlists, tracks);
    }

    public async Task<IEnumerable<T>> LoadEntities<T>(int offset)
    {

        if(typeof(T) == typeof(Artist))
        {
            var maps = await _engine.Bring<ArtistMap>(offset, capacity);
            return await _mapper.GetEntityProjections<ArtistMap, T>(maps);
        }
        else if(typeof(T) == typeof(Playlist))
        {
            var maps = await _engine.Bring<PlaylistMap>(offset, capacity);
            return await _mapper.GetEntityProjections<PlaylistMap, T>(maps);
        }
        else if(typeof(T) == typeof(Track))
        {
            var maps = await _engine.Bring<TrackMap>(offset, capacity);
            return await _mapper.GetEntityProjections<TrackMap, T>(maps);
        }
        else if(typeof(T) == typeof(Tag))
        {
            var maps = await _engine.Bring<TagMap>(offset, capacity);
            return await _mapper.GetEntityProjections<TagMap, T>(maps);
        }
        else throw new Exception("Could not load entities of your type.");
    }
    
    public async Task<IEnumerable<T>> LoadEntitiesById<T>(ICollection<Guid> idCollection)
    {
       var result = await _engine.BringItemsById<T>(idCollection); 
       return result;
    }



    public async Task<IEnumerable<T>> Search<T>(ReadOnlyMemory<char> searchTerm)
    {
        if(typeof(T) == typeof(Artist))
        {
            var maps = await voyager.SearchArtists(searchTerm);
            return await _mapper.GetEntityProjections<ArtistMap, T>(maps);
        }
        else if(typeof(T) == typeof(Playlist))
        {
            var maps = await voyager.SearchPlaylists(searchTerm);
            return await _mapper.GetEntityProjections<PlaylistMap, T>(maps);
        }
        else if(typeof(T) == typeof(Track))
        {
            var maps = await voyager.SearchTracks(searchTerm);
            return await _mapper.GetEntityProjections<TrackMap, T>(maps);
        }
        else throw new Exception("Could not load entities of your type."); 
    }

    

    //here instead of using generic T have been implemented methods for each type
    //reason is imposibility of implicit casting with T generic type.
    public async Task<Artist> FetchArtist(Guid artistId)
    {
       var artistMap = await _engine.BringSingle<ArtistMap>(artistId);
       var artist = _mapper.ExtractSingle<Artist>(artistMap);

        var apStore = await _engine.BringStore(1, artist.Id);   
        var atStore = await _engine.BringStore(3, artist.Id);

        if((apStore.Tag == 1) && (apStore.Holder == artist.Id))
        {
            apStore.Relates.ToList().ForEach(r => artist.Playlists.Add(r));
        }

        if((atStore.Tag == 3) && (atStore.Holder == artist.Id))
        {
            atStore.Relates.ToList().ForEach(r => artist.Tracks.Add(r));
        }
        return artist;
    }

    public async Task<Playlist> FetchPlaylist(Guid playlistId)
    {
       var playlistMap = await _engine.BringSingle<PlaylistMap>(playlistId);
       var playlist = _mapper.ExtractSingle<Playlist>(playlistMap);

       var paStore = await _engine.BringStore(2, playlist.Id); 
       var ptStore = await _engine.BringStore(5, playlist.Id);

       if((paStore.Tag == 2) && (paStore.Holder == playlist.Id))
       {
           paStore.Relates.ToList().ForEach(r => playlist.Artists.Add(r));
       }

       if((ptStore.Tag == 5) && (ptStore.Holder == playlist.Id))
       {
           ptStore.Relates.ToList().ForEach(r => playlist.Tracky.Add(r));
       }

       return playlist;
    }

    public async Task<Track> FetchTrack(Guid trackId)
    {
       var trackMap = await _engine.BringSingle<TrackMap>(trackId);
       var track = _mapper.ExtractSingle<Track>(trackMap);

       var taStore = await _engine.BringStore(4, track.Id);
       var tpStore = await _engine.BringStore(6, track.Id);

        if((taStore.Tag == 4) && (taStore.Holder == track.Id))
        {
            taStore.Relates.ToList().ForEach(r => track.Artists.Add(r));
        }

        if((tpStore.Tag == 6) && (tpStore.Holder == track.Id))
        {
            tpStore.Relates.ToList().ForEach(r => track.Playlists.Add(r));
        }
        return track;
    }

    //use extend methods when you fetched a bare instance without any dependencies
    //for example playlist without tracks or track without artists. 
    //In this case expend methods will come handy.
    public async Task<Artist> ExtendArtist(Artist artist)
    {
        var apStore = await _engine.BringStore(1, artist.Id);   
        var atStore = await _engine.BringStore(3, artist.Id);

        if((apStore.Tag == 1) && (apStore.Holder == artist.Id))
        {
            apStore.Relates.ToList().ForEach(r => artist.Playlists.Add(r));
        }

        if((atStore.Tag == 3) && (atStore.Holder == artist.Id))
        {
            atStore.Relates.ToList().ForEach(r => artist.Tracks.Add(r));
        }
        return artist;
    }
   
    public async Task<Playlist> ExtendPlaylist(Playlist playlist)
    {
        var paStore = await _engine.BringStore(2, playlist.Id); 
        var ptStore = await _engine.BringStore(5, playlist.Id);

        if((paStore.Tag == 2) && (paStore.Holder == playlist.Id))
        {
            paStore.Relates.ToList().ForEach(r => playlist.Artists.Add(r));
        }

        if((ptStore.Tag == 5) && (ptStore.Holder == playlist.Id))
        {
            ptStore.Relates.ToList().ForEach(r => playlist.Tracky.Add(r));
        }

        return playlist;
    }

    public async Task<Track> ExtendTrack(Track track)
    {
        var taStore = await _engine.BringStore(4, track.Id);
        var tpStore = await _engine.BringStore(6, track.Id);

        if((taStore.Tag == 4) && (taStore.Holder == track.Id))
        {
            taStore.Relates.ToList().ForEach(r => track.Artists.Add(r));
        }

        if((tpStore.Tag == 6) && (tpStore.Holder == track.Id))
        {
            tpStore.Relates.ToList().ForEach(r => track.Playlists.Add(r));
        }
        return track;
    }

    public async Task<IEnumerable<Guid>> FilterRelates(int tag, ICollection<Guid> relates)
    {
        return await _engine.CheckRelates(tag, relates);
    }

    public async Task<IEnumerable<Guid>> FilterTrackRelates(Guid trackId, bool isArtist)
    {
        int tag = (isArtist)?4:6;
        var store = await _engine.BringStore(tag, trackId);
        return store.Relates;
    }
    #endregion
}
