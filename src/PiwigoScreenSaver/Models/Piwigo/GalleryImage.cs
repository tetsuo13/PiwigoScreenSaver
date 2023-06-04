using System.Collections.Generic;

namespace PiwigoScreenSaver.Models.Piwigo;

public record GalleryImage
{
    public string Name { get; init; } = default!;
    public IDictionary<string, Derivative> Derivatives { get; init; } = new Dictionary<string, Derivative>();
}
