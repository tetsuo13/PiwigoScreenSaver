using System.Drawing;

namespace PiwigoScreenSaver.Models;

public class ImageFetchResultModel
{
    public bool HadError { get; set; }
    public string ErrorMessage { get; set; } = default!;
    public Image Image { get; set; } = default!;
}
