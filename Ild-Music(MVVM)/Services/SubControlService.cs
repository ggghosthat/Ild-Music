using Ild_Music_MVVM_.Services.Parents;
using Ild_Music_MVVM_.View.UISubControls.FactorySubControl;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Ild_Music_MVVM_.Services
{
    public class SubControlService : Service
    {
        public override string ServiceType { get; init; } = "SubControlObserver";

        public IList<UserControl> UserSubControls { get; private set; } = new List<UserControl>();

        public SubControlService() 
            => InitSubControls();

        private void InitSubControls()
        {
            UserSubControls.Add(new FacArtistSubControl());
            UserSubControls.Add(new FacPlaylistSubControl());
            UserSubControls.Add(new FacTrackSubControl());
        }
    }
}
