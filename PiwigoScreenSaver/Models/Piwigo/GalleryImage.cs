using System.Collections.Generic;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class GalleryImage
    {
        public string Name { get; set; }
        public IDictionary<string, Derivative> Derivatives { get; set; }
    }
}
