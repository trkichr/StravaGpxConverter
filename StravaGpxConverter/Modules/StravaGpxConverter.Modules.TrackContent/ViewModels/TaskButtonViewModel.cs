using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.TrackPoint;
using StravaGpxConverter.Core.Models.TrackSegment;
using StravaGpxConverter.Core.Mvvm;
using StravaGpxConverter.Infrastructure;
using StravaGpxConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace StravaGpxConverter.Modules.TrackContent.ViewModels
{
    public class TaskButtonViewModel : RegionViewModelBase
    {
        public TaskButtonViewModel(ILogger logger, IMessageService ms, IRegionManager rm)
            :this(logger, ms, rm, Factories.CreateTrackPoint())
        {
        }

        public TaskButtonViewModel(ILogger logger, IMessageService ms, IRegionManager rm, ITrackPointRepository trackPointRepository)
            : base(logger, rm)
        {
            _ms = ms;
            _rm = rm;
            _trackPointRepository = trackPointRepository;

            GpxFileName = new ReactivePropertySlim<string>()
                .AddTo(Disposables);
            SelectGpxFileCommand = new ReactiveCommand()
                .WithSubscribe(SelectGpxFile);
            ReadGpxFileCommand = GpxFileName
                .Select(x => !string.IsNullOrEmpty(x))
                .ToReactiveCommand()
                .WithSubscribe(ReadGpxFile)
                .AddTo(Disposables);
        }

        public void SelectGpxFile()
        {
            GpxFileName.Value = _ms.ShowFileDialog();
        }

        public void ReadGpxFile()
        {
            var np = new NavigationParameters();
            _trackPointRepository.Load(GpxFileName.Value);
            AllTrackPointList = _trackPointRepository.GetAll();
            WaitingTrackPointList = TrackPointService.GetWaitingTrackPointList(AllTrackPointList);

            np.Add(nameof(WaitingTrackPointList), WaitingTrackPointList);
            _rm.RequestNavigate(RegionNames.ContentRegion, nameof(ViewNames.DeletedContent), np);
        }

        private ITrackPointRepository _trackPointRepository;
        private IMessageService _ms;
        private IRegionManager _rm;
        public List<TrackPointEntity> AllTrackPointList { get; set; }
        public List<TrackPointEntity> WaitingTrackPointList { get; set; }
        public ReactivePropertySlim<string> GpxFileName { get; set; }
        public ReactiveCommand SelectGpxFileCommand { get; }
        public ReactiveCommand ReadGpxFileCommand { get; }
    }
}
