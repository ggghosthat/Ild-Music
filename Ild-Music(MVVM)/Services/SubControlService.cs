using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using Ild_Music_MVVM_.ViewModel.VM.FactoryVM;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.Services
{
    public class SubControlService : Service
    {
        private static SubControlViewModel SubControlVM = new();

        public override string ServiceType { get; init; } = "SubControlObserver";

        public IList<UserControl> UserSubControls { get; private set; } = new List<UserControl>();

        public SubControlService() 
            => InitSubControls();

        private void InitSubControls()
        {
            UserSubControls.Add(new FacArtistSubControl() { DataContext = SubControlVM} );
            UserSubControls.Add(new FacPlaylistSubControl() { DataContext = SubControlVM } );
            UserSubControls.Add(new FacTrackSubControl() { DataContext = SubControlVM } );
        }
    }
}
