using Ild_Music.Core.Instances;
using Ild_Music.Core.Contracts.Services.Interfaces;
using Ild_Music.Core.Services.Entities;
using Ild_Music.ViewModels.Base;
using Ild_Music.Views.FactorySubViews;

using System;
using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels;
public class FactoryContainerViewModel : BaseViewModel
{
    public static readonly string nameVM = "FactoryVM";
    public override string NameVM => nameVM;

    #region Services
    private FactoryGhost factoryService => (FactoryGhost)base.GetService(Ghosts.FACTORY);
    private SupportGhost supporterService => (SupportGhost)base.GetService(Ghosts.SUPPORT);
    #endregion

    #region Properties
    public static ArtistFactoryViewModel SubArtistContent { get; set; } = new();
    public static PlaylistFactoryViewModel SubPlaylistContent { get; set; } = new();
    public static TrackFactoryViewModel SubTrackContent { get; set; } = new();


    public ObservableCollection<SubItem> SubItems { get; set; } = new();
    public static SubItem SubItem { get; set; }
    #endregion
        
    #region Const
    public FactoryContainerViewModel()
    {
        SubItems.Add(new SubItem("Artist", new SubArtist() {DataContext = SubArtistContent} ));
        SubItems.Add(new SubItem("Playlist", new SubPlaylist() {DataContext = SubPlaylistContent} ));
        SubItems.Add(new SubItem("Track", new SubTrack() {DataContext = SubTrackContent} ));
            
        SubItem = SubItems[0];
    }
    #endregion

    public void SetSubItem(EntityTag entityTag)
    {
        SubItem = entityTag switch
        {
            EntityTag.ARTIST => SubItems[0],
            EntityTag.PLAYLIST => SubItems[1],
            EntityTag.TRACK => SubItems[2]
        };
    }

    public void SetEditableItem(EntityTag entityTag,
                                Guid entityId)
    {
        SubItem = entityTag switch
        {
            EntityTag.ARTIST => SubItems[0],
            EntityTag.PLAYLIST => SubItems[1],
            EntityTag.TRACK => SubItems[2]
        };

    }

    public void SetEditableItem(Artist artist)
    {
        SubItem = SubItems[0];
        var context = (ArtistFactoryViewModel)SubItem.Control.DataContext;
        context?.DropInstance(artist);
    }

    public void SetEditableItem(Playlist playlist)
    {
        SubItem = SubItems[1];
        var context = (PlaylistFactoryViewModel)SubItem.Control.DataContext;
        context?.DropInstance(playlist);
    }

    public void SetEditableItem(Track track)
    {
        SubItem = SubItems[2];
        var context = (TrackFactoryViewModel)SubItem.Control.DataContext;
        context?.DropInstance(track);
    }
}

public record class SubItem(string Name, UserControl Control);