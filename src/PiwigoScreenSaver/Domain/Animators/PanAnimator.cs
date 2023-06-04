using System;
using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Domain.Animators;

public class PanAnimator : IAnimator
{
    private Control? control;
    private int moveX;
    private int moveY;

    private readonly Random _rand;

    public PanAnimator()
    {
        _rand = new Random();
    }

    public void SetControl(Size windowSize, Size imageSize, Control control)
    {
        this.control = control;

        // Center the image relative to the window. Likely will have
        // top/bottom or left/right edges past the viewable area.
        this.control.Top = (windowSize.Height - imageSize.Height) / 2;
        this.control.Left = (windowSize.Width - imageSize.Width) / 2;

        // REducehorizontal panning for images that are taller than wider
        // otherwise the edge of the image will be shown.
        var maxStepX = imageSize.Height > imageSize.Width ? 1 : 3;
        var maxStepY = imageSize.Height < imageSize.Width ? 1 : 3;

        moveX = _rand.Next(-maxStepX, maxStepX);
        moveY = _rand.Next(-maxStepY, maxStepY);
    }

    public void Animate()
    {
        if (control == null)
        {
            throw new InvalidOperationException("Missing call to SetControl() first");
        }

        control.Top += moveY;
        control.Left += moveX;
    }
}
