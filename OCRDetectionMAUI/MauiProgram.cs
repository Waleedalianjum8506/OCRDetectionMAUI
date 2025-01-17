﻿using Microsoft.Maui.Controls.Compatibility.Hosting;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Plugin.Maui.OCR;
namespace BarcodeQrScanner;

// https://docs.microsoft.com/en-us/dotnet/api/skiasharp.views.maui.controls.apphostbuilderextensions?view=skiasharp-views-maui-2.88
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.UseSkiaSharp()
			.UseMauiApp<App>()
            .UseOcr()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).UseMauiCompatibility()
            .ConfigureMauiHandlers((handlers) => {
                
#if ANDROID
                handlers.AddCompatibilityRenderer(typeof(CameraPreview), typeof(BarcodeQrScanner.Platforms.Android.CameraPreviewRenderer));
#endif

#if IOS
//handlers.AddHandler(typeof(CameraPreview), typeof(BarcodeQrScanner.Platforms.iOS.CameraPreviewRenderer));
				                        handlers.AddHandler(typeof(CameraPreview), typeof(BarcodeQrScanner.Platforms.iOS.CameraPreviewRenderer));
#endif
			});

		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif

		return builder.Build();
	}
}
