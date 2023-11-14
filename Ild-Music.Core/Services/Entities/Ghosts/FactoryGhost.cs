using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Exceptions.SynchAreaExceptions;

namespace Ild_Music.Core.Services.Entities;
public sealed class FactoryGhost : IGhost
{
    public ReadOnlyMemory<char> GhostName {get; init;} = "FactoryGhost".AsMemory(); 

    public static SupportGhost SupportGhost {get; private set;}
    private static InstanceProducer.InstanceProducer producer = default;
    
    public FactoryGhost(SupportGhost supportGhost)
    {
        SupportGhost = supportGhost;
    }

    public void Init(ref SupportGhost supportGhost)
    {
        SupportGhost = supportGhost;
    }

    #region Instance Creation Methods
    public void CreateArtist(string name,
                             string description,
                             int year,    
                             string avatar = null)
    {
        try
        {
            Memory<byte> artistAvatarSource = ExtractTrackAvatar(avatar).Result;

            producer = new InstanceProducer.InstanceProducer(name.ToCharArray(),
                                                             description.ToCharArray(),
                                                             artistAvatarSource, 
                                                             year);
            SupportGhost.AddArtistInstance(producer.ArtistInstance);
            producer.Dispose();
        }
        catch (InvalidArtistException ex)
        {
            throw ex;
        }
    }
        

    public void CreatePlaylist(string name,
                               string description,
                               int year,
                               string avatar = null,
                               IList<Track> tracks = null,
                               IList<Artist> artists = null)
    {   
        try
        {
            Memory<byte> playlistAvatarSource = ExtractTrackAvatar(avatar).Result;

            producer = new InstanceProducer.InstanceProducer(name.ToCharArray(),
                                                             description.ToCharArray(),
                                                             playlistAvatarSource,
                                                             year,
                                                             tracks,
                                                             artists);
            SupportGhost.AddPlaylistInstance(producer.PlaylistInstance);
            producer.Dispose();
        }
        catch (InvalidPlaylistException ex)
        {
            throw ex;
        }
    }

    public void CreateTrack(string pathway,
                            int year,
                            string name=null,
                            string description=null,
                            string avatarPath = null,
                            IList<Artist> artists = null)
    {      
        try
        {               
            using(var taglib = TagLib.File.Create(pathway))
            {
                
                Memory<char> trackName;
                Memory<char> trackDescription;
                Memory<byte> trackAvatarSource = default!;

                if(name is null)
                {
                    trackName = taglib.Tag.Title.ToCharArray()
                                    ?? Path.GetFileName(pathway).ToCharArray();
                }
                else trackName = name.ToCharArray();

                trackDescription = description.ToCharArray();
                
                if (string.IsNullOrEmpty(avatarPath))
                {
                    if(taglib.Tag.Pictures.Length > 0)
                    {
                        trackAvatarSource = taglib.Tag.Pictures[0].Data.Data;
                    }
                }
                else trackAvatarSource = ExtractTrackAvatar(avatarPath).Result;

                               
                producer = new InstanceProducer.InstanceProducer(pathway.ToCharArray(),
                                                                 trackName,
                                                                 trackDescription,
                                                                 trackAvatarSource,
                                                                 taglib.Properties.Duration,
                                                                 year,
                                                                 artists);
                SupportGhost.AddTrackInstance(producer.TrackInstance);
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
    public Track CreateTrackBrowsed(Memory<char> pathway)
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

                               
                producer = new InstanceProducer.InstanceProducer(pathway,
                                                                 trackName,
                                                                 trackDescription,
                                                                 trackAvatarSource,
                                                                 taglib.Properties.Duration,
                                                                 year,
                                                                 null);
                SupportGhost.AddTrackInstance(producer.TrackInstance);
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
