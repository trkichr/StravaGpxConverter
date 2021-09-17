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
using System.Collections.ObjectModel;
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

            FileNameCollection = new ObservableCollection<string>();
            FileNameList = FileNameCollection
                .ToReadOnlyReactiveCollection()
                .AddTo(Disposables);
            SelectGpxFileCommand = new ReactiveCommand()
                .WithSubscribe(SelectGpxFile);
            IsReadGpxFile = new ReactivePropertySlim<bool>(false)
                .AddTo(Disposables);
            ReadGpxFileCommand = IsReadGpxFile
                .ToReactiveCommand()
                .WithSubscribe(ReadGpxFile)
                .AddTo(Disposables);
        }

        public void SelectGpxFile()
        {
            var fileNameList = _ms.ShowFileDialog();
            if(fileNameList == null && FileNameCollection.Count == 0)
            {
                IsReadGpxFile.Value = false;
            }
            else
            {
                IsReadGpxFile.Value = true;
                FileNameCollection.AddRange(fileNameList);
            }
        }

        public void ReadGpxFile()
        {
            var np = new NavigationParameters();

            try
            {
                _trackPointRepository.Load(FileNameCollection.ToList());
                AllTrackPointList = _trackPointRepository.GetAll();
                WaitingTrackPointList = TrackPointService.GetWaitingTrackPointList(AllTrackPointList);
                _trackPointRepository.Save(WaitingTrackPointList);
            }
            catch(Exception ex)
            {
                ExceptionProc(ex);
                return;
            }

            np.Add(nameof(WaitingTrackPointList), WaitingTrackPointList);
            _rm.RequestNavigate(RegionNames.ContentRegion, nameof(ViewNames.DeletedContent), np);
        }

        private ITrackPointRepository _trackPointRepository;
        private IMessageService _ms;
        private IRegionManager _rm;
        public List<TrackPointEntity> AllTrackPointList { get; set; }
        public List<TrackPointEntity> WaitingTrackPointList { get; set; }
        public ObservableCollection<string> FileNameCollection { get; set; }
        public ReadOnlyReactiveCollection<string> FileNameList { get; private set; }
        public ReactivePropertySlim<bool> IsReadGpxFile{ get; }
        public ReactiveCommand SelectGpxFileCommand { get; }
        public ReactiveCommand ReadGpxFileCommand { get; }
    }
}
