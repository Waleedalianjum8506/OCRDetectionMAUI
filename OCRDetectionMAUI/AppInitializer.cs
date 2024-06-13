using Microsoft.Extensions.DependencyInjection;
using BarcodeQrScanner.Services;

namespace BarcodeQrScanner
{
    public static class AppInitializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register your services
            services.AddSingleton<ICameraService, CameraService>();

            // Add other services or dependencies here
        }
    }
}
