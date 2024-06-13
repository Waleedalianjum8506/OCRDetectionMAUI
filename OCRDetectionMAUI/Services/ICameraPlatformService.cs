using System.IO;
using System.Threading.Tasks;

namespace BarcodeQrScanner.Services
{
    public interface ICameraPlatformService
    {
        Task<Stream> CapturePhotoAsync();
    }
}
