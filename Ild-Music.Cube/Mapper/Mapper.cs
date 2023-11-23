using Ild_Music.Core.Instances;
using Ild_Music.Core.Instances.DTO;
using Cube.Mapper.Entities;

using System.Collections.Concurrent;
using AutoMapper;

namespace Cube.Mapper;

//this class perform mapping operations and has been created to be used within using scopes
public sealed class Mapper : IDisposable
{
    //Map to database models
    private IMapper _mapper = null;

    private IMappable entity = null;
    private IList<Store> stores = null;
    
    private ConcurrentDictionary<IMappable, IList<Store>> multimapStore = null;

    //Map from database models
    private ConcurrentQueue<Artist> artistsQueue = null;
    private ConcurrentQueue<Playlist> playlistsQueue = null;
    private ConcurrentQueue<Track> trackQueue = null;
    private ConcurrentQueue<Tag> tagQueue = null; 

    public IEnumerable<Artist> Artists => artistsQueue.ToList();
    public IEnumerable<Playlist> Playlists => playlistsQueue.ToList();
    public IEnumerable<Track> Tracks => trackQueue.ToList();
    public IEnumerable<Tag> Tags => tagQueue.ToList();


    public Mapper()
    {  
        MapperConfiguration configure = new MapperConfiguration(cfg => 
        {
            cfg.AddProfile(new MapProfile());
        });

        _mapper = new AutoMapper.Mapper(configure);
    }


    #region Public Methods
    public (IMappable, IList<Store>) MakeSnapshot<T>(T entity)
    {
        this.stores = new List<Store>();
        if (entity is Artist artist)
        {
            this.entity = _mapper.Map<ArtistMap>(artist);
            
            var artistPlaylistStore = _mapper.Map<Store>(artist.Playlists);
            artistPlaylistStore.SetHolder(artist.Id);
            artistPlaylistStore.Tag = 1;
            this.stores.Add(artistPlaylistStore);

            var artistTrackStore = _mapper.Map<Store>(artist.Tracks);
            artistTrackStore.SetHolder(artist.Id);
            artistTrackStore.Tag = 2;                        
            this.stores.Add(artistTrackStore);

            var artistTragger = _mapper.Map<Store>(artist.Tags);
            artistTragger.SetHolder(artist.Id);            
            this.stores.Add(artistTragger);

            return (this.entity, this.stores);
        }
        else if(entity is Playlist playlist)
        {                        
            this.entity = _mapper.Map<PlaylistMap>(playlist);

            var playlistArtistStore = _mapper.Map<Store>(playlist.Artists);
            playlistArtistStore.SetHolder(playlist.Id);
            playlistArtistStore.Tag = 3;            
            this.stores.Add(playlistArtistStore);

            var playlistTrackStore = _mapper.Map<Store>(playlist.Tracky);
            playlistTrackStore.SetHolder(playlist.Id);
            playlistTrackStore.Tag = 4;            
            this.stores.Add(playlistTrackStore);

            var playlistTragger = _mapper.Map<Store>(playlist.Tags);
            playlistTragger.SetHolder(playlist.Id);            
            this.stores.Add(playlistTragger);

            return (this.entity, this.stores);
        }
        else if(entity is Track track)
        {
            this.entity = _mapper.Map<TrackMap>(track);

            var trackArtistStore = _mapper.Map<Store>(track.Artists);
            trackArtistStore.SetHolder(track.Id);
            trackArtistStore.Tag = 5;
            this.stores.Add(trackArtistStore);

            var trackPlaylistStore = _mapper.Map<Store>(track.Playlists);
            trackPlaylistStore.SetHolder(track.Id);
            trackPlaylistStore.Tag = 6;                        
            this.stores.Add(trackPlaylistStore);

            var trackTragger = _mapper.Map<Store>(track.Tags);
            trackTragger.SetHolder(track.Id);            
            this.stores.Add(trackTragger);

            return (this.entity, this.stores);
        }
        else return default;
    }

    private async Task GetMappedStaff<T>(ICollection<T> raws)
    {
        var opt = new ParallelOptions {MaxDegreeOfParallelism=4};
        await Parallel.ForEachAsync(raws, opt, async (raw, token) => {
            if (raw is Artist artist)
            {
                var mappedEntity = _mapper.Map<ArtistMap>(artist);
                
                var artistPlaylistStore = _mapper.Map<Store>(artist.Playlists);
                artistPlaylistStore.SetHolder(artist.Id);
                artistPlaylistStore.Tag = 1;

                var artistTrackStore = _mapper.Map<Store>(artist.Tracks);
                artistTrackStore.SetHolder(artist.Id);
                artistTrackStore.Tag = 2;      

                var artistTragger = _mapper.Map<Store>(artist.Tags);
                artistTragger.SetHolder(artist.Id);

                multimapStore.TryAdd(mappedEntity,
                                     new List<Store> {artistPlaylistStore, artistTrackStore, artistTragger} );                      
            }
            else if(raw is Playlist playlist)
            {                        
                var mappedEntity = _mapper.Map<PlaylistMap>(playlist);

                var playlistArtistStore = _mapper.Map<Store>(playlist.Artists);
                playlistArtistStore.SetHolder(playlist.Id);
                playlistArtistStore.Tag = 3;

                var playlistTrackStore = _mapper.Map<Store>(playlist.Tracky);
                playlistTrackStore.SetHolder(playlist.Id);
                playlistTrackStore.Tag = 4;

                var playlistTragger = _mapper.Map<Store>(playlist.Tags);
                playlistTragger.SetHolder(playlist.Id);

                multimapStore.TryAdd(mappedEntity,
                                     new List<Store> {playlistArtistStore, playlistTrackStore, playlistTragger});                      
            }
            else if(raw is Track track)
            {
                var mappedEntity = _mapper.Map<TrackMap>(track);

                var trackArtistStore = _mapper.Map<Store>(track.Artists);
                trackArtistStore.SetHolder(track.Id);
                trackArtistStore.Tag = 5;

                var trackPlaylistStore = _mapper.Map<Store>(track.Playlists);
                trackPlaylistStore.SetHolder(track.Id);
                trackPlaylistStore.Tag = 6;

                var trackTragger = _mapper.Map<Store>(track.Tags);
                trackTragger.SetHolder(track.Id);

                multimapStore.TryAdd(mappedEntity,
                                     new List<Store> {trackArtistStore, trackPlaylistStore, trackTragger});                      
            }
            else if (raw is Tag tag)
            {
                var mappedTag = _mapper.Map<TagMap>(tag);
                multimapStore.TryAdd(mappedTag, null);
            }
        });
    }


    public async Task<IDictionary<IMappable, IList<Store>>> MakeSnapshot<T>(ICollection<T> entities)
    {
        multimapStore = new ConcurrentDictionary<IMappable, IList<Store>>();
        await GetMappedStaff<T>(entities);
        return multimapStore.ToDictionary(pair => pair.Key, pair => pair.Value, multimapStore.Comparer);
    }


    public async Task<IEnumerable<TProjection>> GetEntityProjections<T, TProjection>(IEnumerable<T> raws)
    {
        var resultProjections = new ConcurrentQueue<TProjection>(); 
        var opt = new ParallelOptions {MaxDegreeOfParallelism=4};
        await Parallel.ForEachAsync(raws, opt, async (raw, token) => 
        {
            if (raw is ArtistMap artist && typeof(TProjection) == typeof(Artist))
            {
                var artistProjection = _mapper.Map<TProjection>(artist);
                resultProjections.Enqueue(artistProjection);
            }
            else if(raw is PlaylistMap playlist && typeof(TProjection) == typeof(Playlist))
            {                        
                var playlistProjection = _mapper.Map<TProjection>(playlist);
                resultProjections.Enqueue(playlistProjection);
            }
            else if(raw is TrackMap track && typeof(TProjection) == typeof(Track))
            {
                var trackProjection = _mapper.Map<TProjection>(track);
                resultProjections.Enqueue(trackProjection);
            }
            else if (raw is TagMap tag && typeof(TProjection) == typeof(Tag))
            {
                var tagProjection = _mapper.Map<TProjection>(tag);
                resultProjections.Enqueue(tagProjection);
            }
        });
        
        return resultProjections;
    }


    public T ExtractSingle<T>(IMappable mappedEntity) 
    {      
        T result = _mapper.Map<T>(mappedEntity);
        return result;
    }    

    public void Clean()
    {
        if (entity is not null)
        {
            entity = null;
        }
        if(stores is not null)
        {
            stores = null;
        }

        if (multimapStore is not null)
        {
            multimapStore.Clear();
            multimapStore = null;
        }

        if(artistsQueue is not null)
        {
            artistsQueue.Clear();
            artistsQueue = null;
        }
        
        if(playlistsQueue is not null)
        {
            playlistsQueue.Clear();
            playlistsQueue = null;
        }

        if(trackQueue is not null)
        {
            trackQueue.Clear();
            trackQueue = null;
        }
        
        if(tagQueue is not null)
        {
            tagQueue.Clear();
            tagQueue = null;
        }
    }

    public async Task<IEnumerable<CommonInstanceDTO>> MapCommonInstanceDTOs (IEnumerable<CommonInstanceDTOMap> maps)
    {
        var resultProjections = new ConcurrentQueue<CommonInstanceDTO>(); 
        var opt = new ParallelOptions {MaxDegreeOfParallelism=4};
        await Parallel.ForEachAsync(maps, opt, async (map, token) => 
        {
            if (map is CommonInstanceDTOMap artist)
            {
                var artistProjection = _mapper.Map<CommonInstanceDTO>(artist);
                resultProjections.Enqueue(artistProjection);
            }
        });
        return resultProjections;
    }


    public void Dispose() 
    {
        Clean();
    }
    #endregion
}
