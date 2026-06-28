using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Parcial1LucaDepetris.Services;

namespace Parcial1LucaDepetris.ViewModels
{
    public partial class SensorViewModel : ObservableObject
    {
        private readonly ISensorService _sensorService;
        private bool _isAccelerometerRunning;

        [ObservableProperty]
        private string _locationInfo = "Sin datos de ubicación.";

        [ObservableProperty]
        private string _accelerometerData = "Acelerómetro inactivo.";

        [ObservableProperty]
        private string _cameraPermissionStatus = "Permiso de cámara: no solicitado.";

        [ObservableProperty]
        private string _accelButtonText = "Iniciar acelerómetro";

        public SensorViewModel(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [RelayCommand]
        private async Task GetLocationAsync()
        {
            LocationInfo = "Obteniendo ubicación...";
            var result = await _sensorService.GetLocationAsync();
            LocationInfo = result.HasValue
                ? $"Lat: {result.Value.Latitude:F6}  |  Lon: {result.Value.Longitude:F6}"
                : "No se pudo obtener la ubicación. Verifique los permisos.";
        }

        [RelayCommand]
        private void ToggleAccelerometer()
        {
            if (_isAccelerometerRunning)
            {
                _sensorService.StopAccelerometer();
                AccelerometerData = "Acelerómetro detenido.";
                AccelButtonText = "Iniciar acelerómetro";
                _isAccelerometerRunning = false;
            }
            else
            {
                var started = _sensorService.StartAccelerometer(reading =>
                {
                    AccelerometerData = $"X: {reading.X:F3}  |  Y: {reading.Y:F3}  |  Z: {reading.Z:F3}";
                });

                if (started)
                {
                    AccelButtonText = "Detener acelerómetro";
                    _isAccelerometerRunning = true;
                }
                else
                {
                    AccelerometerData = "Acelerómetro no disponible en este dispositivo.";
                }
            }
        }

        [RelayCommand]
        private void Vibrate()
        {
            if (!_sensorService.Vibrate(500))
                AccelerometerData = string.Empty; // no hace nada visible, falla silenciosamente en desktop
        }

        [RelayCommand]
        private async Task RequestCameraPermissionAsync()
        {
            CameraPermissionStatus = "Solicitando permiso...";
            var granted = await _sensorService.RequestCameraPermissionAsync();
            CameraPermissionStatus = granted
                ? "Permiso de cámara: concedido."
                : "Permiso de cámara: denegado.";
        }
    }
}
