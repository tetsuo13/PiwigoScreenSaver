using System.Collections.Generic;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Views
{
    public interface IMainFormView
    {
        List<Panel> DisplayPanels { get; set; }

        enum Components
        {
            PictureBox,
            ErrorLabel
        }
    }
}
