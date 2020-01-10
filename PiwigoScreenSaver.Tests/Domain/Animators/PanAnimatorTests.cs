using PiwigoScreenSaver.Domain.Animators;
using System;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain.Animators
{
    public class PanAnimatorTests
    {
        [Fact]
        public void ThrowsException_Without_SetControl()
        {
            var animator = new PanAnimator();
            Assert.Throws<NullReferenceException>(() => animator.Animate());
        }
    }
}
