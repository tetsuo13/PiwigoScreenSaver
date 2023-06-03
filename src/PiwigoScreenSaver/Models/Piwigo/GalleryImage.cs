using System.Collections.Generic;

namespace PiwigoScreenSaver.Models.Piwigo;

public record GalleryImage
{
    public string Name { get; init; }
    public IDictionary<string, Derivative> Derivatives { get; init; }
}
