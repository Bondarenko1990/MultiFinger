using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using MultiFinger.Models.Navigation;
using MultiFinger.Views;

namespace MultiFinger.Modules
{
    public class MainModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate(RegionNames.MainRegionName, ViewNames.DrawFiguresViewName);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<DrawFiguresView>();
        }
    }
}
