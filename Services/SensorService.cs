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

        public bool StartAccelerometer(Action<(double X, double Y, double Z)> onReading)
        {
            try
            {
                _onReading = onReading;
                Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
                return true;
            }
            catch (FeatureNotSupportedException)
            {
                return false;
            }
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

        public bool Vibrate(int milliseconds = 500)
        {
            try
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(milliseconds));
                return true;
            }
            catch (FeatureNotSupportedException)
            {
                return false;
            }
        }

        public async Task<bool> RequestCameraPermissionAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.Camera>();
            return status == PermissionStatus.Granted;
        }
    }
}
