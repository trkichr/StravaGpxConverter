using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using StravaGpxConverter.Core.Mvvm;
using StravaGpxConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace StravaGpxConverter.Modules.TrackContent.ViewModels
{
    public class DeletedContentViewModel : RegionViewModelBase
    {
        public DeletedContentViewModel(ILogger logger, IMessageService ms, IRegionManager rm)
            :base(logger, rm)
        {
            _ms = ms;

            GpxFileName = new ReactivePropertySlim<string>()
                .AddTo(Disposables);
            //SelectGpxFileCommand = new ReactiveCommand()
            //    .WithSubscribe(SelecteGpxFile);
            //CreateModifiedGpxFileCommand = GpxFileName
            //    .Select(x => !string.IsNullOrEmpty(x))
            //    .ToReactiveCommand()
            //    .WithSubscribe(CreateModifiedGpxFile)
            //    .AddTo(Disposables);
        }

        //private ITrackPointRepository _trackPoint;
        private IMessageService _ms;
        public ReactivePropertySlim<string> GpxFileName { get; set; }
        public ReactiveCommand SelectGpxFileCommand { get; }
        public ReactiveCommand CreateModifiedGpxFileCommand { get; }
    }
}
