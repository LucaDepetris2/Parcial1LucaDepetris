namespace Parcial1LucaDepetris.Services
{
    public interface ISensorService
    {
        Task<(double Latitude, double Longitude)?> GetLocationAsync();
        void StartAccelerometer(Action<(double X, double Y, double Z)> onReading);
        void StopAccelerometer();
        void Vibrate(int milliseconds = 500);
        Task<bool> RequestCameraPermissionAsync();
    }
}
