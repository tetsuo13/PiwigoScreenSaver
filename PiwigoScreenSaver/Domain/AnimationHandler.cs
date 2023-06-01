using PiwigoScreenSaver.Domain.Animators;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Domain
{
    public class AnimationHandler
    {
        public Size IdealImageSize { get; private set; }

        private IAnimator? animator;

        public AnimationHandler()
        {
            var timer = new Timer
            {
                Interval = 200
            };
            timer.Tick += new EventHandler(Tick);
            timer.Start();
        }

        private void Tick(object? sender, EventArgs e)
        {
            if (animator == null)
            {
                return;
            }

            animator.Animate();
        }

        public void SetControl(Size windowSize, Size imageSize, Control control)
        {
            IdealImageSize = SizeToFitWindow(windowSize, imageSize);

            // We can randomly choose an animation here.
            animator = new PanAnimator();
            animator.SetControl(windowSize, IdealImageSize, control);
        }

        internal Size SizeToFitWindow(Size windowSize, Size imageSize)
        {
            float percentWidth = windowSize.Width / (float)imageSize.Width;
            float percentHeight = windowSize.Height / (float)imageSize.Height;
            float percent = percentHeight < percentWidth ? percentWidth : percentHeight;

            var destinationWidth = (int)(imageSize.Width * percent);
            var destionalHeight = (int)(imageSize.Height * percent);

            return new Size(destinationWidth, destionalHeight);
        }
    }
}
