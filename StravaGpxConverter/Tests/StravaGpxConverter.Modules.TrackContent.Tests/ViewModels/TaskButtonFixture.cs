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
        private static ITrackPointRepository _trackPointRepository;

        public TaskButtonFixture()
        {
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
        public void SelectGpxFileCommand()
        {
            _loggerMock = new Mock<ILogger>();
            _msMock = new Mock<IMessageService>();
            _msMock.Setup(x => x.ShowFileDialog()).Returns("test.gpx");

            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm, _trackPointRepository);
            vm.SelectGpxFile();
            vm.GpxFileName.Value.Is("test.gpx");

        }

        [Fact]
        public void CreateModifiedGpxFile()
        {
            _loggerMock = new Mock<ILogger>();
            _msMock = new Mock<IMessageService>();
            var vm = new TaskButtonViewModel(_loggerMock.Object, _msMock.Object, _rm, _trackPointRepository);
            vm.GpxFileName.Value = "Fake.gpx";
            vm.ReadGpxFile();

            var np = new NavigationParameters();
            np.Add("allTrackPointList", vm._allTrackPointList);
            _mockContentRegion.Verify((r) => r.RequestNavigate(new Uri(ViewNames.DeletedContent, UriKind.Relative),
                                                                            It.IsAny<Action<NavigationResult>>(),
                                                                            np));
        }
    }
}
