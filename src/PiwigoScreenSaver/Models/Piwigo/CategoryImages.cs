using System.Collections.Generic;

namespace PiwigoScreenSaver.Models.Piwigo;

public record CategoryImages
{
    public Paging Paging { get; init; }
    public IEnumerable<GalleryImage> Images { get; init; }
}
