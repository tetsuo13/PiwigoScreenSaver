using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Presenters;

public interface IMainFormPresenter
{
    Rectangle BoundingRectangle { get; set; }
    int Interval { get; }
    List<Panel> DisplayPanels { get; set; }

    bool SignificantMouseMovement(Point initialPosition, Point currentPosition);

    /// <summary>
    /// Display a random photo from the gallery on a random screen. If a
    /// photo can't be fetched for any reason then display an error
    /// error message instead.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Tick(object? sender, EventArgs e);
}
