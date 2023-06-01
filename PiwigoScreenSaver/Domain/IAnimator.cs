using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Domain;

public interface IAnimator
{
    void SetControl(Size windowSize, Size imageSize, Control control);
    void Animate();
}
