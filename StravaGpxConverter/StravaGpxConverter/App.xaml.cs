using Microsoft.Extensions.Logging;
using Prism.Ioc;
using Prism.Modularity;
using StravaGpxConverter.Modules.PopupContents;
using StravaGpxConverter.Modules.TrackContent;
using StravaGpxConverter.Services;
using StravaGpxConverter.Services.Interfaces;
using StravaGpxConverter.Views;
using System.Windows;

namespace StravaGpxConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();

            var config = new NLog.Config.LoggingConfiguration();
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log/file.txt" };
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, logfile);
            NLog.LogManager.Configuration = config;
            var factory = new NLog.Extensions.Logging.NLogLoggerFactory();
            ILogger logger = factory.CreateLogger("");
            containerRegistry.RegisterInstance(logger);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<TrackContentModule>();
            moduleCatalog.AddModule<PopupContentsModule>();
        }
    }
}
