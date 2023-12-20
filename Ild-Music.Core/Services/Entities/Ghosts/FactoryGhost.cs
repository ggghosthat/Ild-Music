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

    #region Instance Creation Methods
    public void CreateArtist(string name,
                             string description = null,
                             int year = 0,    
                             byte[] avatar = null)
    {


        Console.WriteLine($"Factory.Cube is null: {cube is null}");
        try
        {
            Memory<byte> artistAvatarSource = avatar;
            Memory<char> artistDescription;

            if(description is null)
                artistDescription = string.Empty.ToCharArray();
            else 
                artistDescription = description.ToCharArray();

            producer = new InstanceProducer.InstanceProducer(name.ToCharArray(),
                                                             description?.ToCharArray(),
                                                             artistAvatarSource, 
                                                             year);

            cube.AddArtistObj(producer.ArtistInstance); 
            producer.Dispose();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }
        

    public void CreatePlaylist(string name,
                               string description = null,
                               int year = 0,
                               byte[] avatar = null,
                               IList<Track> tracks = null,
                               IList<Artist> artists = null) 
    {   
        try 
        { 
            Memory<byte> playlistAvatarSource = avatar;
            Memory<char> playlistDescription;
            
            if(description is null)
                playlistDescription = string.Empty.ToCharArray();
            else 
                playlistDescription = description.ToCharArray();
            producer = new InstanceProducer.InstanceProducer(name.ToCharArray(), 
                                                             description?.ToCharArray(),
                                                             playlistAvatarSource, 
                                                             year,
                                                             tracks,
                                                             artists); 

            cube.AddPlaylistObj(producer.PlaylistInstance);
            producer.Dispose(); 
        } 
        catch (InvalidPlaylistException ex) 
        { 
            throw ex;
        } 
    }

    public void CreateTrack(string pathway, 
                            string name=null, 
                            string description=null,
                            int year = 0,
                            byte[] avatar = null,
                            IList<Artist> artists = null) 
    {      
        try 
        {               
            using(var taglib = TagLib.File.Create(pathway)) 
            { 
                Memory<char> trackName; 
                Memory<char> trackDescription; 
                Memory<byte> trackAvatarSource; 

                int trackYear = (year != 0)? year: DateTime.Now.Year;

                if(name is null) 
                    trackName = taglib.Tag.Title.ToCharArray() ?? Path.GetFileName(pathway).ToCharArray(); 
                else 
                    trackName = name.ToCharArray(); 
               
                if(description is null)
                    trackDescription = string.Empty.ToCharArray();
                else 
                    trackDescription = description.ToCharArray();
                
                if (avatar is null && taglib.Tag.Pictures.Length > 0)  
                    trackAvatarSource = taglib.Tag.Pictures[0].Data.Data; 
                else 
                    trackAvatarSource = avatar ?? new byte[0]; 
                
                producer = new InstanceProducer.InstanceProducer(pathway.ToCharArray(), 
                                                                 trackName, 
                                                                 trackDescription, 
                                                                 trackAvatarSource,
                                                                 taglib.Properties.Duration,
                                                                 trackYear,
                                                                 artists); 

                cube.AddTrackObj(producer.TrackInstance);
                producer.Dispose(); 
            } 
        } 
        catch (InvalidTrackException ex) 
        { 
            throw ex;
        } 
    }
    #endregion 
    
    #region Filer Methods 
    public Track CreateTrackBrowsed(string pathway) 
    {      
        try 
        {        
            Track trackResult = default!;
            using(var taglib = TagLib.File.Create(pathway.ToString())) 
            { 
                Memory<char> trackName; 
                Memory<char> trackDescription = default!; 
                Memory<byte> trackAvatarSource = default!; 
                int year = DateTime.Now.Year; 
                trackName = taglib.Tag.Title.ToCharArray() ?? "Unknown track".ToCharArray(); 
                
                if(taglib.Tag.Pictures.Length > 0) 
                {
                    trackAvatarSource = taglib.Tag.Pictures[0].Data.Data; 
                } 

                producer = new InstanceProducer.InstanceProducer(pathway.ToCharArray(),
                                                                 trackName,
                                                                 trackDescription,
                                                                 trackAvatarSource,
                                                                 taglib.Properties.Duration,
                                                                 year,
                                                                 null); 
                
                cube.AddTrackObj(producer.TrackInstance);
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
    #endregion 
    #region Accessory Methods 
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
    #endregion
}
