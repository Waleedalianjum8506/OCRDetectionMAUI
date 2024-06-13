using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using BarcodeQrScanner.Services;

[assembly: Dependency(typeof(CameraService))]

namespace BarcodeQrScanner.Services
{
    public class CameraService : ICameraService
    {
        public async Task<byte[]> CapturePhotoAsync()
        {
            try
            {
                var cameraPlatformService = DependencyService.Get<ICameraPlatformService>();
                if (cameraPlatformService != null)
                {
                    var photoStream = await cameraPlatformService.CapturePhotoAsync();
                    return ReadFully(photoStream);
                }
                else
                {
                    Console.WriteLine("Failed to get ICameraPlatformService from DependencyService.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync error: {ex.Message}");
                return null;
            }
        }

        // Helper method to convert stream to byte array
        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
