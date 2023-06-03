using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiwigoScreenSaver.Presenters;

public class MainFormPresenter : IMainFormPresenter
{
    public Rectangle BoundingRectangle { get; set; }
    public int Interval { get { return 30000; } }
    public List<Panel> DisplayPanels { get; set; } = new();

    private readonly ILogger<MainFormPresenter> _logger;
    private readonly AnimationHandler _animator;
    private readonly IGalleryService _galleryService;
    private readonly Random _rand;

    public MainFormPresenter(ILogger<MainFormPresenter> logger, IGalleryService galleryService,
        IEnumerable<Rectangle> allScreensBoundaries)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rand = new Random();

        BoundingRectangle = FindBoundingBox(allScreensBoundaries);

        _galleryService = galleryService ?? throw new ArgumentNullException(nameof(galleryService));
        _animator = new AnimationHandler();
    }

    /// <summary>
    /// Finds the minimum bounding rectangle of all screen boundaries.
    /// This will handle a single display, two displays side by side or on
    /// top of each other, three displays stacked in a pyramid, eight
    /// displays arranged to look like baby Yoda, etc.
    /// </summary>
    /// <param name="allScreensBoundaries"></param>
    /// <returns></returns>
    private static Rectangle FindBoundingBox(IEnumerable<Rectangle> allScreensBoundaries)
    {
        var minX = allScreensBoundaries.Min(s => s.X);
        var minY = allScreensBoundaries.Min(s => s.Y);
        var maxX = allScreensBoundaries.Max(s => s.Width);
        var maxY = allScreensBoundaries.Max(s => s.Height);

        return new Rectangle(minX, minY, maxX - minX, maxY - minY);
    }

    public bool SignificantMouseMovement(Point initialPosition, Point currentPosition)
    {
        return Math.Abs(currentPosition.X - initialPosition.X) > 10 ||
            Math.Abs(currentPosition.Y - initialPosition.Y) > 10;
    }

    public void Tick(object? sender, EventArgs e)
    {
        // Choose a random display to paint the photo onto.
        var targetIndex = _rand.Next(DisplayPanels.Count);

        var targetPanel = DisplayPanels[targetIndex];

        // Blocking, but OK here.
        var imageFetchResult = Task.Run(async () =>
        {
            return await GetImageFromGallery(targetPanel.Size);
        }).Result;

        ShowImageOnDisplay(targetIndex, imageFetchResult);

        // Technically we can fetch another random image here so that it's
        // ready on the next tick interval. We run into the situation
        // where if the interval time isn't set long enough and fetching
        // an image takes significantly long (slow Internet connection,
        // slow gallery instance, etc.), it'll be shown on screen for a
        // shorter amount of time than the interval is set for, giving the
        // impression that the interval is off.
        //
        // On the other hand, not prefetching runs into the same scenario
        // if we encounter slowness on the next interval. So prefetching
        // actually buys us time.
    }

    private void ShowImageOnDisplay(int targetPanelIndex, ImageFetchResultModel imageFetchResult)
    {
        for (var i = 0; i < DisplayPanels.Count; i++)
        {
            var panel = DisplayPanels[i];
            var pictureBox = panel.Controls[MainFormComponents.PictureBox.ToString()]!;
            var errorLabel = panel.Controls[MainFormComponents.ErrorLabel.ToString()]!;

            // Hide everything on this display if it's not the chosen one.
            if (i != targetPanelIndex)
            {
                pictureBox.Visible = false;
                errorLabel.Visible = false;
            }
            else if (imageFetchResult.HadError)
            {
                // This is the chosen display but there was an error
                // fetching the image.

                pictureBox.Visible = false;

                errorLabel.Text = imageFetchResult.ErrorMessage;

                // Random location on screen.
                errorLabel.Location = new Point(_rand.Next(panel.Width / 2),
                    _rand.Next(panel.Height / 2));

                // Expand height to fill screen to leave room for word
                // wrapping in case there's a lot said in the error.
                errorLabel.MaximumSize = new Size(panel.Width - errorLabel.Location.X,
                    panel.Height - errorLabel.Location.Y);

                errorLabel.Visible = true;
            }
            else
            {
                errorLabel.Visible = false;
                pictureBox.Visible = true;
                pictureBox.BackgroundImage = imageFetchResult.Image;

                _animator.SetControl(panel.Size, imageFetchResult.Image.Size, pictureBox);

                pictureBox.Size = _animator.IdealImageSize;

                _logger.LogDebug("Panel size ({panelWidth},{panelHeight}), original image size ({imageWidth},{imageHeight}), image resized ({resizeWidth},{resizeHeight})",
                    panel.Size.Width, panel.Size.Height,
                    imageFetchResult.Image.Width, imageFetchResult.Image.Height,
                    pictureBox.Size.Width, pictureBox.Size.Height);
            }
        }
    }

    internal async Task<ImageFetchResultModel> GetImageFromGallery(Size boundingSize)
    {
        var result = new ImageFetchResultModel();

        try
        {
            result.Image = await _galleryService.GetRandomImage(boundingSize);
        }
        catch (Exception ex)
        {
            result.HadError = true;
            result.ErrorMessage = $"Error getting image: {ex.Message}";
        }

        return result;
    }
}
