using PiwigoScreenSaver.Domain;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain
{
    public class ModeManagerTests
    {
        [Fact]
        public void NoArgs_ReturnsConfiguration()
        {
            var manager = GetManager();
            var actual = manager.GetMode(null);

            Assert.Equal(ModeManager.Mode.Configuration, actual);
        }

        [Fact]
        public void EmptyArgsArray_ReturnsConfiguration()
        {
            var manager = GetManager();
            var actual = manager.GetMode(new string[] { });

            Assert.Equal(ModeManager.Mode.Configuration, actual);
        }

        [Theory]
        [InlineData("/c", ModeManager.Mode.Configuration)]
        [InlineData("/c:123456", ModeManager.Mode.Configuration)]
        [InlineData("/p", ModeManager.Mode.Preview)]
        [InlineData("/p:123456", ModeManager.Mode.Preview)]
        [InlineData("/s", ModeManager.Mode.FullScreen)]
        [InlineData("/s:123456", ModeManager.Mode.FullScreen)]
        public void SingleArg_ReturnsMode(string arg, ModeManager.Mode expected)
        {
            var manager = GetManager();
            var actual = manager.GetMode(new string[] { arg });

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("/z", "/x")]
        [InlineData("/a")]
        public void InvlidArgs_ThrowsException(params string[] args)
        {
            var manager = GetManager();
            Assert.Throws<ArgumentException>(() => manager.GetMode(args));
        }

        private ModeManager GetManager()
        {
            return new ModeManager();
        }
    }
}
