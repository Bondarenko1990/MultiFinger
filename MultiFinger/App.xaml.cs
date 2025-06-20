using System.Windows;
using MultiFinger.ViewModels;
using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using MultiFinger.Modules;
using MultiFinger.Views;
using MultiFinger.Models.Interfaces;
using MultiFinger.Services;

namespace MultiFinger
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    { 
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<DrawFiguresViewModel>();

            containerRegistry.Register<ITextFileService, TextFileService>();
        }

        protected override Window CreateShell()
        {
            return this.Container.Resolve<MainWindow>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
        }
    }
}
