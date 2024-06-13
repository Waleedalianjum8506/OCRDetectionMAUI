using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using BarcodeQrScanner.Droid.Services;
using BarcodeQrScanner.Services;
using System;
using System.IO;
using System.Threading.Tasks;
//using Xamarin.Essentials;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(CameraPlatformService_Android))]

namespace BarcodeQrScanner.Droid.Services
{
    public class CameraPlatformService_Android : ICameraPlatformService
    {
        private static readonly int REQUEST_CAMERA_PERMISSION_CODE = 101;

        public async Task<Stream> CapturePhotoAsync()
        {
            try
            {
                if (ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.Camera) != Permission.Granted)
                {
                    // Request camera permission
                    ActivityCompat.RequestPermissions((Activity)Platform.CurrentActivity, new string[] { Android.Manifest.Permission.Camera }, REQUEST_CAMERA_PERMISSION_CODE);
                    return null;
                }

                // Capture photo using MediaPicker
                var photoFile = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Take a photo"
                });

                // Return photo stream
                return await photoFile.OpenReadAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync error: {ex.Message}");
                return null;
            }
        }
    }
}
