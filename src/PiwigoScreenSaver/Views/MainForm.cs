using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Presenters;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Views;

public partial class MainForm : Form, IMainFormView
{
    private readonly Timer _panelTimer;
    private readonly Point _initialMousePosition;
    private readonly IMainFormPresenter _presenter;

    public MainForm(IMainFormPresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

        InitializeComponent();

        _initialMousePosition = MousePosition;
        _panelTimer = new Timer
        {
            Interval = _presenter.Interval
        };

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
        var boundingRectangle = _presenter.BoundingRectangle;

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
                Name = MainFormComponents.PictureBox.ToString()
            };
            pictureBox.MouseMove += OnMouseMove;

            var errorLabel = new Label
            {
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.Orange,
                Name = MainFormComponents.ErrorLabel.ToString(),
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

            _presenter.DisplayPanels.Add(panel);
            Controls.Add(panel);
        }

        _panelTimer.Tick += _presenter.Tick;
        _panelTimer.Start();

        // Immediately call the event so we're not staring at a black
        // screen until the first interval ellapses.
        _presenter.Tick(null, EventArgs.Empty);
    }

    private void OnKeyDown(object sender, EventArgs e) => ExitApplication();
    private void OnMouseClick(object sender, EventArgs e) => ExitApplication();
    private void OnMouseDown(object sender, EventArgs e) => ExitApplication();

    private void OnMouseMove(object? sender, EventArgs e)
    {
        if (_presenter.SignificantMouseMovement(_initialMousePosition, MousePosition))
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
