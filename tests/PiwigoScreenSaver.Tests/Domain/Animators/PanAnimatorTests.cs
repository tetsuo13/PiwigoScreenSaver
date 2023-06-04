using PiwigoScreenSaver.Domain.Animators;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain.Animators;

public class PanAnimatorTests
{
    [Fact]
    public void ThrowsException_Without_SetControl()
    {
        var animator = new PanAnimator();
        Assert.Throws<InvalidOperationException>(animator.Animate);
    }
}
