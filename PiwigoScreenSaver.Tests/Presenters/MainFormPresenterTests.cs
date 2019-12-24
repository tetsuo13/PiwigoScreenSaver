using Moq;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Presenters;
using PiwigoScreenSaver.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Xunit;

namespace PiwigoScreenSaver.Tests.Presenters
{
    public class MainFormPresenterTests
    {
        private readonly List<Rectangle> MockBoundaries = new List<Rectangle>
        {
            new Rectangle(0, 0, 640, 480)
        };

        public static IEnumerable<object[]> SignificantMouseMovementData()
        {
            yield return new object[] { new Point(5, 5), new Point(6, 6), false };
            yield return new object[] { new Point(5, 5), new Point(20, 5), true };
            yield return new object[] { new Point(5, 5), new Point(5, 42), true };
        }

        [Theory]
        [MemberData(nameof(SignificantMouseMovementData))]
        public void SignificantMouseMovement(Point initialPosition, Point currentPosition, bool expected)
        {
            var view = new MockMainFormView();
            var presenter = new MainFormPresenter(view, null, MockBoundaries);
            Assert.Equal(expected, presenter.SignificantMouseMovement(initialPosition, currentPosition));
        }

        [Fact]
        public void BoundingRectangle_SingleScreen()
        {
            var view = new MockMainFormView();
            var boundaries = new List<Rectangle>
            {
                new Rectangle(0, 0, 3840, 2160)
            };
            var presenter = new MainFormPresenter(view, null, boundaries);

            Assert.Equal(0, presenter.BoundingRectangle.X);
            Assert.Equal(0, presenter.BoundingRectangle.Y);
            Assert.Equal(boundaries[0].Width, presenter.BoundingRectangle.Width);
            Assert.Equal(boundaries[0].Height, presenter.BoundingRectangle.Height);
        }

        [Fact]
        public async Task GetImageFromGallery_GalleryThrowsException_IndicatesError()
        {
            var view = new MockMainFormView();
            var galleryService = new Mock<IGalleryService>();
            galleryService.Setup(x => x.GetRandomImage(It.IsAny<Size>())).Throws<Exception>();
            var presenter = new MainFormPresenter(view, galleryService.Object, MockBoundaries);

            var actual = await presenter.GetImageFromGallery(new Size(320, 240));

            Assert.NotNull(actual);
            Assert.True(actual.HadError);
            Assert.False(string.IsNullOrEmpty(actual.ErrorMessage));
        }

        [Fact]
        public async Task GetImageFromGallery_GalleryDoesntThrowException_IndicatesSuccess()
        {
            var view = new MockMainFormView();
            var galleryService = new Mock<IGalleryService>();
            var presenter = new MainFormPresenter(view, galleryService.Object, MockBoundaries);

            var actual = await presenter.GetImageFromGallery(new Size(320, 240));

            Assert.NotNull(actual);
            Assert.False(actual.HadError);
            Assert.True(string.IsNullOrEmpty(actual.ErrorMessage));
        }
    }
}
