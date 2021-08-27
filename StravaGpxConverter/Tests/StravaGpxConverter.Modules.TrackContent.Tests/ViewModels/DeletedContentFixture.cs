using Microsoft.Extensions.Logging;
using Moq;
using Prism.Regions;
using Prism.Services.Dialogs;
using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.TrackPoint;
using StravaGpxConverter.Core.Models.TrackSegment;
using StravaGpxConverter.Infrastructure;
using StravaGpxConverter.Modules.TrackContent.ViewModels;
using StravaGpxConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StravaGpxConverter.Modules.TrackContent.Tests.ViewModels
{
    public class DeletedContentFixture
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IDialogService> _dsMock;
        private static Mock<IRegion> _mockTaskButtonRegion;
        private static Mock<IRegion> _mockContentRegion;
        private static IRegionManager _rm;
        private static ITrackPointRepository _trackPointRepository;

        public DeletedContentFixture()
        {
            _loggerMock = new Mock<ILogger>();
            _dsMock = new Mock<IDialogService>();
            _mockTaskButtonRegion = new Mock<IRegion>();
            _mockContentRegion = new Mock<IRegion>();
            _mockTaskButtonRegion.SetupGet((r) => r.Name).Returns(RegionNames.TaskButtonRegion);
            _mockContentRegion.SetupGet((r) => r.Name).Returns(RegionNames.ContentRegion);
            _rm = new RegionManager();
            _rm.Regions.Add(_mockTaskButtonRegion.Object);
            _rm.Regions.Add(_mockContentRegion.Object);
            _trackPointRepository = Factories.CreateTrackPoint();
        }

        [Fact]
        public void OnNavigatedToCalled()
        {
            var vm = new DeletedContentViewModel(_loggerMock.Object, _rm, _dsMock.Object);
            Mock<IRegionNavigationService> rnsMock = new Mock<IRegionNavigationService>();
            var np = new NavigationParameters();
            NavigationContext nc = new NavigationContext(rnsMock.Object, new Uri(ViewNames.DeletedContent, UriKind.Relative), np);
            nc.Parameters.Add("AllTrackPointList", _trackPointRepository.GetAll());

            vm.OnNavigatedTo(nc);
            vm.AllTrackPointList.Count.Is(195);
            vm.WaitingTrackPointList.Count.Is(20);
            vm.WaitingTrackPointList[0].Index.Is<uint>(3);
            vm.WaitingTrackPointList[1].Index.Is<uint>(4);
            vm.WaitingTrackPointList[2].Index.Is<uint>(28);
            vm.WaitingTrackPointList[19].Index.Is<uint>(194);
            vm.TrackSegmentItemList.Count.Is(6);
        }

        [Fact]
        public void DataGridMouseDoubleClickCalled()
        {
            //var _dsMock = new Mock<IDialogService>();
            var vm = new DeletedContentViewModel(_loggerMock.Object, _rm, _dsMock.Object);
            vm.SelectedTrackSegment.Value = new TrackSegmentEntity(new TrackPointEntity(0, "34.64591", "134.995037", "39.5", "11:18:04 2021-06-06", null),
                                                                                                    new TrackPointEntity(10, "34.64691", "134.997037", "34.5", "12:18:04 2021-06-06", null));
            vm.DataGridMouseDoubleClick();

            _dsMock.Verify(d => d.ShowDialog(nameof(ViewNames.TrackPointMap), It.IsAny<DialogParameters>(), null));
        }
    }
}
