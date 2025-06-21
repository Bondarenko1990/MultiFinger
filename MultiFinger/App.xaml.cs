using System.Windows;
using MultiFinger.ViewModels;
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
            containerRegistry.Register<FingersViewModel>();

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
