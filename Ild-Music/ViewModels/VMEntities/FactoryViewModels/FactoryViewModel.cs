using ShareInstances.Instances;
using ShareInstances.Instances.Interfaces;
using ShareInstances.Services.Entities;
using Ild_Music;
using Ild_Music.Command;
using Ild_Music.ViewModels.Base;
using Ild_Music.Views.FactorySubViews;


using Avalonia.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Ild_Music.ViewModels
{
    public class FactoryViewModel : BaseViewModel
    {
        public static readonly string nameVM = "FactoryVM";
        public override string NameVM => nameVM;

        #region Services
        private FactoryService factoryService => (FactoryService)base.GetService("FactoryService");
        private SupporterService supporterService => (SupporterService)base.GetService("SupporterService");
        #endregion

        #region Properties
        public static ArtistFactoryViewModel SubArtistContent { get; set; } = new();
        public static PlaylistFactoryViewModel SubPlaylistContent { get; set; } = new();
        public static TrackFactoryViewModel SubTrackContent { get; set; } = new();


        public ObservableCollection<SubItem> SubItems { get; set; } = new();
        public static SubItem SubItem { get; set; }
        #endregion
        
        #region Const
        public FactoryViewModel()
        {
            SubItems.Add(new SubItem("Artist", new SubArtist() {DataContext = SubArtistContent} ));
            SubItems.Add(new SubItem("Playlist", new SubPlaylist() {DataContext = SubPlaylistContent} ));
            SubItems.Add(new SubItem("Track", new SubTrack() {DataContext = SubTrackContent} ));
            
            SubItem = SubItems[0];
        }
        #endregion

        public void SetSubItem(int index)
        {
            if(index <= 0)
                SubItem = SubItems[0];
            else if(index == 1)
                SubItem = SubItems[1];
            else if(index >= 2)
                SubItem = SubItems[2];            
        }

        public void SetEditableItem(ICoreEntity entity)
        {
            if (entity is Artist artist)
            {
                SubItem = SubItems[0];
                var context = (ArtistFactoryViewModel)SubItem.Control.DataContext;
                context.DropInstance(entity);
            }
            else if (entity is Playlist playlist)
            {
                SubItem = SubItems[1];
                var context = (PlaylistFactoryViewModel)SubItem.Control.DataContext;
                context.DropInstance(entity);
            }
            else if (entity is Track track)
            {
                SubItem = SubItems[2];
                var context = (TrackFactoryViewModel)SubItem.Control.DataContext;
                context.DropInstance(entity);
            }
        }
    }

    public record class SubItem(string Name, UserControl Control);

}
