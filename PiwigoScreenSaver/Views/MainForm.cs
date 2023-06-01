using PiwigoScreenSaver.Presenters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Views;

public partial class MainForm : Form, IMainFormView
{
    public List<Panel> DisplayPanels { get; set; } = new List<Panel>();

    private Timer panelTimer;

    private readonly Point _initialMousePosition;

    public MainForm()
    {
        InitializeComponent();

        _initialMousePosition = MousePosition;

        Load += OnLoad;
    }

    /// <summary>
    /// Create a panel to encompass every screen available. These panels
    /// will serve to display the gallery image, both image and panel
    /// chosen at random.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnLoad(object? sender, EventArgs e)
    {
        var boundingRectangle = ((MainFormPresenter)Tag).BoundingRectangle;

        // Change size of form to encompass all possible screens.
        ClientSize = new Size(boundingRectangle.Width, boundingRectangle.Height);

        Location = new Point(boundingRectangle.X, boundingRectangle.Y);
        Cursor.Hide();
        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

        // Create a panel for each screen that matches its boundary.
        foreach (var screen in Screen.AllScreens)
        {
            // No need to set location and size as it'll change.
            var pictureBox = new PictureBox
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Name = IMainFormView.Components.PictureBox.ToString()
            };
            pictureBox.MouseMove += OnMouseMove;

            var errorLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Orange,
                Name = IMainFormView.Components.ErrorLabel.ToString(),
                Visible = false
            };

            var panel = new Panel
            {
                Location = new Point(screen.Bounds.X, screen.Bounds.Y),
                Size = new Size(screen.Bounds.Width, screen.Bounds.Height)
            };

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(errorLabel);
            panel.MouseMove += OnMouseMove;

            DisplayPanels.Add(panel);
            Controls.Add(panel);
        }

        panelTimer = new Timer
        {
            Interval = ((MainFormPresenter)Tag).Interval
        };
        panelTimer.Tick += ((MainFormPresenter)Tag).Tick;
        panelTimer.Start();

        // Immediately call the event so we're not staring at a black
        // screen until the first interval ellapses.
        ((MainFormPresenter)Tag).Tick(null, null);
    }

    private void OnKeyDown(object sender, EventArgs e)
    {
        ExitApplication();
    }

    private void OnMouseClick(object sender, EventArgs e)
    {
        ExitApplication();
    }

    private void OnMouseDown(object sender, EventArgs e)
    {
        ExitApplication();
    }

    private void OnMouseMove(object? sender, EventArgs e)
    {
        if (((MainFormPresenter)Tag).SignificantMouseMovement(_initialMousePosition, MousePosition))
        {
            ExitApplication();
        }
    }

    private void ExitApplication()
    {
        galleryPictureBox?.Dispose();
        Application.Exit();
    }
}
