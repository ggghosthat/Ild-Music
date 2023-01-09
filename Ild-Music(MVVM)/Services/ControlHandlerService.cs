using Ild_Music_MVVM_.Services.Parents;
using System.Collections.Generic;
using System.Windows.Controls;
using Ild_Music_MVVM_.View.UIControls;

namespace Ild_Music_MVVM_.Services
{
    public class ControlHandlerService : Service
    {
        public override string ServiceType { get; init; } = "ControlHandler";

        ICollection<UserControl> handledControls = new List<UserControl>();

        public ControlHandlerService()
        {
            HandleUIControls();
        }

        //initialize UIControls 
        private void HandleUIControls()
        {
            handledControls.Add(new StartControl());
            handledControls.Add(new ListControl() );
            handledControls.Add(new TrackControl() );
        }
    }
}
