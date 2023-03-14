using Ild_Music_MVVM_.Holder;
using Ild_Music_MVVM_.Services.Parents;

namespace Ild_Music_MVVM_.Services
{
    internal class TabHolderService : Service
    {
        public override string ServiceType { get; init; } = "TabHolder";
        public TabHolder TabHolder { get; set; }

        public TabHolderService (Service service)
        {
            if (service is SubControlService subControlService)
                TabHolder = new(subControlService);
        }
    }
}
