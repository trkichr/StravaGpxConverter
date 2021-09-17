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
            _msMock.Setup(x => x.ShowFileDialog())
                .Returns(new string[] { "test1.gpx", "test2.gpx", "test3.gpx" });

            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm);
            vm.SelectGpxFile();
            vm.IsReadGpxFile.Value.IsTrue();
            vm.FileNameCollection.Contains("test1.gpx").IsTrue();
            vm.FileNameCollection.Contains("test2.gpx").IsTrue();
            vm.FileNameCollection.Contains("test3.gpx").IsTrue();
        }

        [Fact]
        public void CreateModifiedGpxFile()
        {
            _loggerMock = new Mock<ILogger>();
            _msMock = new Mock<IMessageService>();
            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm);
            vm.FileNameCollection.Add(Shared.FakePath + "Fake1.gpx");
            vm.FileNameCollection.Add(Shared.FakePath + "Fake2.gpx");
            vm.FileNameCollection.Add(Shared.FakePath + "Fake3.gpx");
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
