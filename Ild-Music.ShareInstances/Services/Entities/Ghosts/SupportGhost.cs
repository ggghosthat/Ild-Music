using ShareInstances.Instances;
using ShareInstances.Contracts;
using ShareInstances.Contracts.Services;
using ShareInstances.Contracts.Services.Interfaces;

using System;
using System.Collections.Generic;
namespace ShareInstances.Services.Entities;
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


    public void DeleteArtistInstance(Artist artist) 
    {
        CubeArea.RemoveArtistObj(artist);
        OnTracksNotifyRefresh?.Invoke();
    }
    
    public void DeletePlaylistInstance(Playlist playlist) 
    {
        CubeArea.RemovePlaylistObj(playlist);
        OnPlaylistsNotifyRefresh?.Invoke();
    }

    public void DeleteTrackInstance(Track track)
    {
        CubeArea.RemoveTrackObj(track);
        OnArtistsNotifyRefresh?.Invoke();
    }
       
}
