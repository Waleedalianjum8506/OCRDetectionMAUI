using System;
using System.Threading.Tasks;

namespace BarcodeQrScanner.Services
{
    public interface ICameraService
    {
        Task<byte[]> CapturePhotoAsync();
    }
}
