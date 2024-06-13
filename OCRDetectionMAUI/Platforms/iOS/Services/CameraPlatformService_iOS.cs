using System;
using System.IO;
using System.Threading.Tasks;
using BarcodeQrScanner.iOS.Services;
using BarcodeQrScanner.Services;
using Foundation;
using UIKit;

[assembly: Microsoft.Maui.Controls.Dependency(typeof(CameraPlatformService_iOS))]

namespace BarcodeQrScanner.iOS.Services
{
    public class CameraPlatformService_iOS : ICameraPlatformService
    {
        private UIImagePickerController imagePicker;

        public async Task<Stream> CapturePhotoAsync()
        {
            try
            {
                var tcs = new TaskCompletionSource<Stream>();

                // Check if the device has a camera available
                if (!UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
                {
                    Console.WriteLine("Camera is not available on this device.");
                    return null;
                }

                // Initialize UIImagePickerController
                imagePicker = new UIImagePickerController
                {
                    SourceType = UIImagePickerControllerSourceType.Camera,
                    CameraCaptureMode = UIImagePickerControllerCameraCaptureMode.Photo
                };

                // Set event handlers
                imagePicker.FinishedPickingMedia += (sender, e) =>
                {
                    var image = e.Info[UIImagePickerController.OriginalImage] as UIImage;
                    if (image != null)
                    {
                        NSData data = image.AsJPEG();
                        tcs.SetResult(data.AsStream());
                    }
                    else
                    {
                        tcs.SetResult(null);
                    }
                    imagePicker.DismissViewController(true, null);
                };

                imagePicker.Canceled += (sender, e) =>
                {
                    tcs.SetResult(null);
                    imagePicker.DismissViewController(true, null);
                };

                // Present UIImagePickerController
                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(imagePicker, true, null);

                return await tcs.Task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync error: {ex.Message}");
                return null;
            }
        }
    }
}
