using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Wpf;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using StravaGpxConverter.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StravaGpxConverter.Modules.PopupContents.ViewModels
{
    public class TrackPointMapViewModel : ViewModelBase, IDialogAware
    {
        public TrackPointMapViewModel(ILogger logger)
            :base(logger)
        {
            Url = new ReactivePropertySlim<string>();
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            StartLat = parameters.GetValue<string>(nameof(StartLat));
            StartLon = parameters.GetValue<string>(nameof(StartLon));
            Url.Value = urlBase + StartLat + "," + StartLon + ",15z";
        }


        private const string urlBase = "https://www.google.com/maps/@";
        public string Title => string.Empty;
        public WebView2 _webView;
        public event Action<IDialogResult> RequestClose;
        public string StartLat { get; set; }
        public string StartLon { get; set; }
        public string UrlId { get; set; }
        public ReactivePropertySlim<string> Url { get; set; }
    }
}
