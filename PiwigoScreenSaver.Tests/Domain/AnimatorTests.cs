using PiwigoScreenSaver.Domain;
using System.Drawing;
using System.Windows.Forms;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain
{
    public class AnimatorTests
    {
        [Theory]
        [InlineData(640, 480, 320, 240, 640, 480)]
        [InlineData(3840, 2160, 931, 1242, 3839, 5122)]
        public void ScaleToFitWindow(int windowWidth, int windowHeight,
            int imageWidth, int imageHeight,
            int expectedWidth, int expectedHeight)
        {
            var expectedSize = new Size(expectedWidth, expectedHeight);
            var windowSize = new Size(windowWidth, windowHeight);
            var imageSize = new Size(imageWidth, imageHeight);

            var animator = new AnimationHandler();
            animator.SetControl(windowSize, imageSize, new PictureBox());

            Assert.Equal(expectedSize, animator.IdealImageSize);
        }
    }
}
