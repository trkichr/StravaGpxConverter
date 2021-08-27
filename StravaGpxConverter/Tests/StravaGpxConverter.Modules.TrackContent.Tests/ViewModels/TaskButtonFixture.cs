using Microsoft.Extensions.Logging;
using Moq;
using Prism.Regions;
using StravaGpxConverter.Core;
using StravaGpxConverter.Core.Models.TrackPoint;
using StravaGpxConverter.Infrastructure;
using StravaGpxConverter.Modules.TrackContent.ViewModels;
using StravaGpxConverter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace StravaGpxConverter.Modules.TrackContent.Tests.ViewModels
{
    public class TaskButtonFixture
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IMessageService> _msMock;
        private static Mock<IRegion> _mockTaskButtonRegion;
        private static Mock<IRegion> _mockContentRegion;
        private static IRegionManager _rm;

        public TaskButtonFixture()
        {
            _mockTaskButtonRegion = new Mock<IRegion>();
            _mockContentRegion = new Mock<IRegion>();
            _mockTaskButtonRegion.SetupGet((r) => r.Name).Returns(RegionNames.TaskButtonRegion);
            _mockContentRegion.SetupGet((r) => r.Name).Returns(RegionNames.ContentRegion);
            _rm = new RegionManager();
            _rm.Regions.Add(_mockTaskButtonRegion.Object);
            _rm.Regions.Add(_mockContentRegion.Object);
        }

        [Fact]
        public void SelectGpxFileCommand()
        {
            _loggerMock = new Mock<ILogger>();
            _msMock = new Mock<IMessageService>();
            _msMock.Setup(x => x.ShowFileDialog()).Returns("test.gpx");

            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm);
            vm.SelectGpxFile();
            vm.GpxFileName.Value.Is("test.gpx");

        }

        [Fact]
        public void CreateModifiedGpxFile()
        {
            _loggerMock = new Mock<ILogger>();
            _msMock = new Mock<IMessageService>();
            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm);
            vm.GpxFileName.Value = "Fake.gpx";
            vm.ReadGpxFile();

            vm.AllTrackPointList.Count.Is(195);
            vm.WaitingTrackPointList.Count.Is(20);
            vm.WaitingTrackPointList[0].Index.Is<uint>(3);
            vm.WaitingTrackPointList[1].Index.Is<uint>(4);
            vm.WaitingTrackPointList[2].Index.Is<uint>(28);
            vm.WaitingTrackPointList[19].Index.Is<uint>(194);

            var np = new NavigationParameters();
            np.Add(nameof(vm.WaitingTrackPointList), vm.WaitingTrackPointList);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.DeletedContent, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }
    }
}
