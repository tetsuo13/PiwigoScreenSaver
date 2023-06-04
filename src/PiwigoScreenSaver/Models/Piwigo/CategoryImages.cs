using System.Collections.Generic;
using System.Linq;

namespace PiwigoScreenSaver.Models.Piwigo;

public record CategoryImages
{
    public Paging Paging { get; init; } = default!;
    public IEnumerable<GalleryImage> Images { get; init; } = Enumerable.Empty<GalleryImage>();
}
