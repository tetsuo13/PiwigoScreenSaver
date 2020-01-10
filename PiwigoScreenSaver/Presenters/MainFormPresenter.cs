using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using PiwigoScreenSaver.Models;
using PiwigoScreenSaver.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace PiwigoScreenSaver.Presenters
{
    public class MainFormPresenter
    {
        public Rectangle BoundingRectangle { get; private set; }
        public int Interval { get { return 30000; } }

        private readonly ILogger logger;
        private readonly AnimationHandler animator;
        private readonly IGalleryService galleryService;
        private readonly IMainFormView mainFormView;
        private readonly Random Rand;

        public MainFormPresenter(ILogger logger, IMainFormView mainFormView,
            IGalleryService galleryService, IEnumerable<Rectangle> allScreensBoundaries)
        {
            this.logger = logger;
            this.mainFormView = mainFormView;
            Rand = new Random();

            BoundingRectangle = FindBoundingBox(allScreensBoundaries);

            this.galleryService = galleryService;
            animator = new AnimationHandler();
        }

        /// <summary>
        /// Finds the minimum bounding rectangle of all screen boundaries.
        /// This will handle a single display, two displays side by side or on
        /// top of each other, three displays stacked in a pyramid, eight
        /// displays arranged to look like baby Yoda, etc.
        /// </summary>
        /// <param name="allScreensBoundaries"></param>
        /// <returns></returns>
        private Rectangle FindBoundingBox(IEnumerable<Rectangle> allScreensBoundaries)
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

        /// <summary>
        /// Display a random photo from the gallery on a random screen. If a
        /// photo can't be fetched for any reason then display an error
        /// error message instead.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Tick(object sender, EventArgs e)
        {
            // Choose a random display to paint the photo onto.
            var targetIndex = Rand.Next(mainFormView.DisplayPanels.Count);

            var targetPanel = mainFormView.DisplayPanels[targetIndex];

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
            for (var i = 0; i < mainFormView.DisplayPanels.Count; i++)
            {
                var panel = mainFormView.DisplayPanels[i];
                var pictureBox = panel.Controls[IMainFormView.Components.PictureBox.ToString()];
                var errorLabel = panel.Controls[IMainFormView.Components.ErrorLabel.ToString()];

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
                    errorLabel.Location = new Point(Rand.Next(panel.Width / 2),
                        Rand.Next(panel.Height / 2));
                    errorLabel.MaximumSize = new Size(panel.Width - 100, panel.Height - 100);
                    errorLabel.Visible = true;
                }
                else
                {
                    errorLabel.Visible = false;
                    pictureBox.Visible = true;
                    pictureBox.BackgroundImage = imageFetchResult.Image;

                    animator.SetControl(panel.Size, imageFetchResult.Image.Size, pictureBox);

                    pictureBox.Size = animator.IdealImageSize;

                    logger.LogDebug("Panel size ({0},{1}), original image size ({1},{2}), image resized ({3},{4})",
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
                result.Image = await galleryService.GetRandomImage(boundingSize);
            }
            catch (Exception ex)
            {
                result.HadError = true;
                result.ErrorMessage = $"Error getting image: {ex.Message}";
            }

            return result;
        }
    }
}
