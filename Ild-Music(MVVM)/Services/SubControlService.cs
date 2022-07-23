using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using ShareInstances.PlayerResources;
using ShareInstances.PlayerResources.Interfaces;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.Services
{
    public class SubControlService : Service
    {
        #region Fields
        private static SubControlViewModel SubControlVM = new();
        #endregion

        #region PROPERTIES 
        public override string ServiceType { get; init; } = "SubControlObserver";

        public IList<UserControl> UserSubControls { get; private set; } = new List<UserControl>();
        #endregion

        #region Const
        public SubControlService() 
            => InitSubControls();
        #endregion

        #region Private Methods
        private void InitSubControls()
        {
            UserSubControls.Add(new FacArtistSubControl() { DataContext = SubControlVM} );
            UserSubControls.Add(new FacPlaylistSubControl() { DataContext = SubControlVM } );
            UserSubControls.Add(new FacTrackSubControl() { DataContext = SubControlVM } );
        }
        #endregion

        #region Public Methods
        public void DropInstance(ICoreEntity instance)
        {
            SubControlVM.DropInstance(instance);

            if (instance is Artist)
            {
                var asrtistSubControl = (FacArtistSubControl)UserSubControls[0];
                asrtistSubControl.InvokeCheckInstance();
            }
            if (instance is Playlist)
            {
                var playlistSubControl = (FacPlaylistSubControl)UserSubControls[1];
                playlistSubControl.InvokeCheckInstance();
            }
            if (instance is Track)
            {
                var trackSubControl = (FacTrackSubControl)UserSubControls[2];
                trackSubControl.InvokeCheckInstance();
            }
        }

        #endregion
    }
}
