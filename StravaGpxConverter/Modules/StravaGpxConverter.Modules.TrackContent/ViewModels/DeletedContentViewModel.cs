using Microsoft.Extensions.Logging;
using Prism.Regions;
using Prism.Services.Dialogs;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.TrackPoint;
using StravaGpxConverter.Core.Models.TrackSegment;
using StravaGpxConverter.Core.Mvvm;
using StravaGpxConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace StravaGpxConverter.Modules.TrackContent.ViewModels
{
    public class DeletedContentViewModel : RegionViewModelBase
    {
        public DeletedContentViewModel(ILogger logger, IRegionManager rm, IDialogService ds)
            :base(logger, rm)
        {
            _ds = ds;

            SelectedTrackSegment = new ReactivePropertySlim<TrackSegmentEntity>();
            TrackSegmentItemList = new ObservableCollection<TrackSegmentEntity>();
            TrackSegmentItems = TrackSegmentItemList
                .ToReadOnlyReactiveCollection()
                .AddTo(Disposables);
            DataGridMouseDoubleClickCommand = new ReactiveCommand()
                .WithSubscribe(DataGridMouseDoubleClick);
        }

        public void DataGridMouseDoubleClick()
        {
            var dp = new DialogParameters();
            dp.Add(nameof(SelectedTrackSegment.Value.StartLat), SelectedTrackSegment.Value.StartLat.StringValue);
            dp.Add(nameof(SelectedTrackSegment.Value.StartLon), SelectedTrackSegment.Value.StartLon.StringValue);
            _ds.ShowDialog(nameof(ViewNames.TrackPointMap), dp, null);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            WaitingTrackPointList = navigationContext.Parameters.GetValue<List<TrackPointEntity>>(nameof(WaitingTrackPointList));
            TrackSegmentItemList.AddRange(TrackSegmentService.CreateSegmentList(WaitingTrackPointList)
                .Where(x => x.WaitingTime.Value.TotalSeconds > 60));
        }

        private IDialogService _ds;
        public ReactiveCommand CreateModifiedGpxFileCommand { get; }
        public List<TrackPointEntity> WaitingTrackPointList { get; set; }
        public ReactivePropertySlim<TrackSegmentEntity> SelectedTrackSegment { get; set; }
        public ObservableCollection<TrackSegmentEntity> TrackSegmentItemList { get; set; }
        public ReadOnlyReactiveCollection<TrackSegmentEntity> TrackSegmentItems { get; private set; }
        public ReactiveCommand DataGridMouseDoubleClickCommand { get; }
    }
}
