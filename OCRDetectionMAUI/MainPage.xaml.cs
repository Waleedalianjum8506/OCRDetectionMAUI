﻿using BarcodeQrScanner.Services;
using Dynamsoft;

namespace BarcodeQrScanner;

public partial class MainPage : ContentPage
{
    string PhotoPath = "";

    public MainPage()
	{
		InitializeComponent();
        InitService();
    }

    async void OnTakePhotoButtonClicked(object sender, EventArgs e)
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            await Navigation.PushAsync(new WebContentPage());
            return;
        }

        try
        {

            FileResult photo = null;
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI || DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                photo = await FilePicker.PickAsync();
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.Android || DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                photo = await MediaPicker.CapturePhotoAsync();
            }

            await LoadPhotoAsync(photo);
            Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Feature is not supported on the device
        }
        catch (PermissionException pEx)
        {
            // Permissions not granted
        }
        catch (Exception ex)
        {
            Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
        }
    }

    async Task LoadPhotoAsync(FileResult photo)
    {
        // canceled
        if (photo == null)
        {
            PhotoPath = null;
            return;
        }
        // save the file into local storage
        var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
        using (var stream = await photo.OpenReadAsync())
        using (var newStream = File.OpenWrite(newFile))
            await stream.CopyToAsync(newStream);

        PhotoPath = newFile;

        await Navigation.PushAsync(new PicturePage(PhotoPath));
    }

    private async void InitService()
    {
        await Task.Run(() =>
        {
            BarcodeQRCodeReader.InitLicense("DLS2eyJoYW5kc2hha2VDb2RlIjoiMjAwMDAxLTE2NDk4Mjk3OTI2MzUiLCJvcmdhbml6YXRpb25JRCI6IjIwMDAwMSIsInNlc3Npb25QYXNzd29yZCI6IndTcGR6Vm05WDJrcEQ5YUoifQ==");

            return Task.CompletedTask;
        });
    }

    async void OnTakeVideoButtonClicked(object sender, EventArgs e)
    {
        var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
        if (status == PermissionStatus.Granted)
        {
            ToCameraPage();
        }
        else
        {
            status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status == PermissionStatus.Granted)
            {
                ToCameraPage();
            }
            else
            {
                await DisplayAlert("Permission needed", "I will need Camera permission for this action", "Ok");
            }
        }
    }

    async void ToCameraPage()
    {
        if (DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
        {
            await Navigation.PushAsync(new WebContentPage());
            return;
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            await Navigation.PushAsync(new DesktopCameraPage());
            //await Navigation.PushAsync(new WebContentPage());
            return;
        }
        else
        {
            await Navigation.PushAsync(new CameraPage());
        }
    }
}

