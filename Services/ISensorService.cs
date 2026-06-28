namespace Parcial1LucaDepetris.Services
{
    public interface ISensorService
    {
        Task<(double Latitude, double Longitude)?> GetLocationAsync();
        bool StartAccelerometer(Action<(double X, double Y, double Z)> onReading);
        void StopAccelerometer();
        bool Vibrate(int milliseconds = 500);
        Task<bool> RequestCameraPermissionAsync();
    }
}
