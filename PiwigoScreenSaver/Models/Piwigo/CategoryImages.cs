using System.Collections.Generic;

namespace PiwigoScreenSaver.Models.Piwigo
{
    public class CategoryImages
    {
        public Paging Paging { get; set; }
        public IEnumerable<GalleryImage> Images { get; set; }
    }
}
