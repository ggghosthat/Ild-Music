using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Exceptions.CubeExceptions;

namespace Ild_Music.Core.Services.Entities;

public sealed class FactoryGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "FactoryGhost".AsMemory(); 

    private static ICube cube;
    private static InstanceProducer.InstanceProducer producer = default;
    
    public FactoryGhost()
    {}

    public void Init(ICube inputCube)
    {
       cube = inputCube;
    }

    public void CreateArtist(
        string name,
        string description = default!,
        string avatarPath = default!,
        int year = 0)
    {
        try
        {
            producer = new InstanceProducer.InstanceProducer(
                name.ToCharArray(),
                description?.ToCharArray(),
                avatarPath?.ToCharArray(), 
                year);
            
            cube.AddArtistObj(producer.ArtistInstance); 
            producer.Dispose();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }
        
    public void CreateArtist(
        string name,
        string description,
        string avatarPath,
        int year,    
        out Artist artist)
    {
        try
        {
            producer = new InstanceProducer.InstanceProducer(
                name.ToCharArray(),
                description?.ToCharArray(),
                avatarPath?.ToCharArray(), 
                year);

            artist = producer.ArtistInstance; 
            cube.AddArtistObj(producer.ArtistInstance); 
            producer.Dispose();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }

    public void CreatePlaylist(
        string name,
        string description = default!,
        string avatarPath = default!,
        int year = 0,
        IList<Track> tracks = default!,
        IList<Artist> artists = default!) 
    {   
        try 
        { 
            producer = new InstanceProducer.InstanceProducer(
                name.ToCharArray(), 
                description?.ToCharArray(),
                avatarPath?.ToCharArray(), 
                tracks,
                artists,
                year);

            cube.AddPlaylistObj(producer.PlaylistInstance);
            producer.Dispose(); 
        } 
        catch (InvalidPlaylistException ex) 
        { 
            throw ex;
        } 
    }

    public void CreatePlaylist(
        string name,
        string description,
        string avatarPath,
        int year,
        IList<Track> tracks,
        IList<Artist> artists,
        out Playlist playlist) 
    {   
        try 
        { 
            producer = new InstanceProducer.InstanceProducer(
                name.ToCharArray(), 
                description?.ToCharArray(),
                avatarPath?.ToCharArray(), 
                tracks,
                artists,
                year);

            playlist = producer.PlaylistInstance;
            cube.AddPlaylistObj(producer.PlaylistInstance);
            producer.Dispose(); 
        } 
        catch (InvalidPlaylistException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTrack(
        string pathway,
        string name = default!, 
        string description = default!,
        string avatarPath = default!,
        int year = 0,
        IList<Artist> artists = default!) 
    {      
        try 
        {
            using(var taglib = TagLib.File.Create(pathway)) 
            {
                producer = new InstanceProducer.InstanceProducer(
                    pathway.ToCharArray(), 
                    name.ToCharArray(),
                    description?.ToCharArray(), 
                    avatarPath?.ToCharArray(),
                    artists,
                    taglib.Properties.Duration, 
                    year);

                cube.AddTrackObj(producer.TrackInstance);
                producer.Dispose(); 
            } 
        } 
        catch (InvalidTrackException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTrack(
        string pathway, 
        string name, 
        string description,
        string avatarPath,
        int year,
        IList<Artist> artists,
        out Track track) 
    {      
        try 
        {               
            using(var taglib = TagLib.File.Create(pathway)) 
            { 
                producer = new InstanceProducer.InstanceProducer(
                    pathway.ToCharArray(), 
                    name?.ToCharArray(), 
                    description?.ToCharArray(),
                    avatarPath?.ToCharArray(),
                    artists,
                    taglib.Properties.Duration, 
                    year);

                track = producer.TrackInstance;
                cube.AddTrackObj(producer.TrackInstance);
                producer.Dispose(); 
            } 
        } 
        catch (InvalidTrackException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTag(
        string name,
        string color)
    {
        Memory<char> tagName = name.ToArray();
        Memory<char> tagColor = name.ToArray();

        producer = new InstanceProducer.InstanceProducer(tagName, tagColor, null, null, null); 
        cube.AddTagObj(producer.TagInstance);
        producer.Dispose();
    }

    public void CreateTag(
        string name,
        string color, 
        out Tag tag)
    {
        Memory<char> tagName = name.ToArray();
        Memory<char> tagColor = name.ToArray();

        producer = new InstanceProducer.InstanceProducer(tagName, tagColor, null, null, null); 
        tag = producer.TagInstance;
        cube.AddTagObj(producer.TagInstance);
        producer.Dispose();
    }
    
    public Track CreateTrackBrowsed(string pathway) 
    {      
        try 
        {        
            Track trackResult = default!;
            using(var taglib = TagLib.File.Create(pathway.ToString())) 
            {
                int year = DateTime.Now.Year; 
                var name = taglib.Tag.Title ?? "Untitled track";
                
                producer = new InstanceProducer.InstanceProducer(
                    pathway.ToCharArray(),
                    name.ToCharArray(),
                    String.Empty.ToCharArray(),
                    String.Empty.ToCharArray(),
                    null,
                    taglib.Properties.Duration,
                    year); 
               
                if(taglib.Tag.Pictures.Length > 0)
                {
                    var avatarSource = taglib.Tag.Pictures[0].Data.Data;
                    cube.PlaceAvatar(producer.TrackInstance.Id, avatarSource);
                }

                trackResult = producer.TrackInstance; 
                producer.Dispose(); 
            }
            return trackResult; 
        } 
        catch (InvalidTrackException ex)
        {
            throw ex; 
        } 
    } 

    private async ValueTask<Memory<byte>> ExtractTrackAvatar(string pathway) 
    { 
        Memory<byte> buffer; 
        if(File.Exists(pathway)) 
        { 
            using(FileStream fs = File.OpenRead(pathway))
            {
                buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer);
            }
            return buffer;
        }

        throw new FileNotFoundException($"Could not find file: {pathway}");
    }
}
