using AndroidX.Startup;
using BarcodeQrScanner.Services;

namespace BarcodeQrScanner;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        ConfigureServices();
    }
    private void ConfigureServices()
    {
        // Get the service collection
        var services = new ServiceCollection();

        // Register your services
        services.AddSingleton<ICameraService, CameraService>();

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve any required services
        // For example:
        // var cameraService = serviceProvider.GetRequiredService<ICameraService>();

        // Continue with app initialization
    }
}
