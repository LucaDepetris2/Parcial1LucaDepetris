namespace Parcial1LucaDepetris.Services
{
    public class SensorService : ISensorService
    {
        private Action<(double X, double Y, double Z)>? _onReading;

        public async Task<(double Latitude, double Longitude)?> GetLocationAsync()
        {
            try
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                    return null;

                var location = await Geolocation.Default.GetLocationAsync(
                    new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10)));

                return location is null ? null : (location.Latitude, location.Longitude);
            }
            catch
            {
                return null;
            }
        }

        public void StartAccelerometer(Action<(double X, double Y, double Z)> onReading)
        {
            _onReading = onReading;
            Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
            Accelerometer.Default.Start(SensorSpeed.UI);
        }

        public void StopAccelerometer()
        {
            Accelerometer.Default.Stop();
            Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
            _onReading = null;
        }

        private void OnAccelerometerReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            var r = e.Reading;
            _onReading?.Invoke((r.Acceleration.X, r.Acceleration.Y, r.Acceleration.Z));
        }

        public void Vibrate(int milliseconds = 500)
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(milliseconds));
        }

        public async Task<bool> RequestCameraPermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            return status == PermissionStatus.Granted;
        }
    }
}
