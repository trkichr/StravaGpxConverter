using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using StravaGpxConverter.Modules.PopupContents.Views;

namespace StravaGpxConverter.Modules.PopupContents
{
    public class PopupContentsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TrackPointMap>();
        }
    }
}