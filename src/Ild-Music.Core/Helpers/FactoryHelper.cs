using Ild_Music.Core.Instances;
using Ild_Music.Core.Services;
using TagLib.Id3v2;

namespace Ild_Music.Core.Helpers;

public class FactoryHelper
{
    public static Artist CreateArtist(string name, string description = default!, string avatarPath = default!, int year = 0)
    {
        Artist artist;
        using var producer = new InstanceProducer(name.ToCharArray(), description?.ToCharArray(),avatarPath?.ToCharArray(),year);
        artist = producer.ArtistInstance;
        return artist;
    }

    public static Playlist CreatePlaylist(string name, string description = default!, string avatarPath = default!, int year = 0, IList<Track> tracks = default!, IList<Artist> artists = default!)
    {
        Playlist playlist;
        using var producer = new InstanceProducer(name.ToCharArray(), description?.ToCharArray(), avatarPath?.ToCharArray(),tracks, artists, year);
        playlist = producer.PlaylistInstance;
        return playlist;
    }

    

    public static Ild_Music.Core.Instances.Tag CreateTag (string name, string color)
    {
        Ild_Music.Core.Instances.Tag tag;
        using var producer = new InstanceProducer(name.ToCharArray(), color.ToCharArray(), null, null, null);
        tag = producer.TagInstance;
        return tag;
    }

    public static Track CreateTrack(string pathway, string name, string description = default!, string avatarPath = default!, int year = 0, IList<Artist> artists = default!)
    {
        Track track;
        using var taglib = TagLib.File.Create(pathway);
        using var producer = new InstanceProducer(pathway.ToCharArray(), name.ToCharArray(), description?.ToCharArray(), avatarPath?.ToCharArray(), artists, taglib.Properties.Duration, year);
        track = producer.TrackInstance;
        return track;
    }

    public static Track CreateTrack(string pathway)
    {
        Track track;
        using var taglib = TagLib.File.Create(pathway);
        
        string name = taglib.Tag.Title ?? "Untitled track";
        int year = DateTime.Now.Year; 
        
        using var producer = new InstanceProducer(pathway.ToCharArray(), name.ToCharArray(), String.Empty.ToCharArray(), String.Empty.ToCharArray(), null , taglib.Properties.Duration, year); 
                        
        track = producer.TrackInstance; 
        track.MimeType = taglib.MimeType.AsMemory();
        
        if(taglib.Tag.Pictures.Length > 0)
            track.AvatarSource = taglib.Tag.Pictures[0].Data.Data;

        return track;
    }
}