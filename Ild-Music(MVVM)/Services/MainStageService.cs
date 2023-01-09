using Ild_Music_MVVM_.Services.Parents;
using MainStage.Stage;

namespace Ild_Music_MVVM_.Services
{
    internal class MainStageService : Service
    {
        public override string ServiceType { get; init; }  = "MainStageProvider";

        public Platform Platform => App.mainPlatform;
    }
}
