using PiwigoScreenSaver.Views;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Tests.Mocks
{
    public class MockMainFormView : IMainFormView
    {
        public List<Panel> DisplayPanels { get; set; } = new List<Panel>();
    }
}
