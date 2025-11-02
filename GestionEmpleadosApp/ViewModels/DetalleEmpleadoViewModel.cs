using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionEmpleadosApp.Helpers;
using GestionEmpleadosApp.Models;

namespace GestionEmpleadosApp.ViewModels
{
    [QueryProperty(nameof(Empleado), "Empleado")]
    public partial class DetalleEmpleadoViewModel : ObservableObject
    {
        [ObservableProperty]
        private Empleado empleado = new()
        {
            FechaIngreso = DateTime.Today
        };

        [ObservableProperty]
        private ImageSource fotoPreview;

        partial void OnEmpleadoChanged(Empleado value)
        {
            // Cuando se recibe un empleado para edición, cargar la foto si existe
            if (value != null && !string.IsNullOrWhiteSpace(value.FotoBase64))
            {
                FotoPreview = ImageHelper.Base64ToImageSource(value.FotoBase64);
            }
        }

        [RelayCommand]
        public async Task TomarFotoAsync()
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    var photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using var stream = await photo.OpenReadAsync();
                        Empleado.FotoBase64 = await ImageHelper.StreamToBase64Async(stream);
                        FotoPreview = ImageHelper.Base64ToImageSource(Empleado.FotoBase64);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "La cámara no está disponible en este dispositivo.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo tomar la foto: {ex.Message}", "OK");
            }
        }

        [RelayCommand]
        public async Task GuardarAsync()
        {
            if (string.IsNullOrWhiteSpace(Empleado.Nombre))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre es obligatorio.", "OK");
                return;
            }

            await App.DbService.SaveEmpleadoAsync(Empleado);
            await Shell.Current.GoToAsync("..");
        }
    }
}

