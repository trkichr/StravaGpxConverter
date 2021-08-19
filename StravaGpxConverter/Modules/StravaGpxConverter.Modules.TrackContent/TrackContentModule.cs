using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using StravaGpxConverter.Core;
using StravaGpxConverter.Modules.TrackContent.Views;

namespace StravaGpxConverter.Modules.TrackContent
{
    public class TrackContentModule : IModule
    {
        private readonly IRegionManager _regionManager;
        public TrackContentModule(IRegionManager regionManger)
        {
            _regionManager = regionManger;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.TaskButtonRegion, nameof(TaskButton));
            _regionManager.RequestNavigate(RegionNames.ContentRegion, nameof(DeletedContent));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<TaskButton>();
            containerRegistry.RegisterForNavigation<DeletedContent>();
        }
    }
}