using System.Drawing;
using System.Threading.Tasks;

namespace PiwigoScreenSaver.Domain;

public interface IGalleryService
{
    Task<Image> GetRandomImage(Size boundingSize);
}
