using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionEmpleadosApp.Models;
using Newtonsoft.Json;

namespace GestionEmpleadosApp.ViewModels
{
    public partial class ImportarViewModel : ObservableObject
    {
        [ObservableProperty]
        private string archivoSeleccionado;

        [RelayCommand]
        public async Task SeleccionarArchivoAsync()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
    {
        { DevicePlatform.Android, new[] { "application/json", "text/json" } },
        { DevicePlatform.iOS, new[] { "public.json" } },
        { DevicePlatform.WinUI, new[] { ".json" } },
        { DevicePlatform.MacCatalyst, new[] { "public.json" } }
    });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona archivo JSON",
                FileTypes = customFileType
            });

            if (result != null)
            {
                ArchivoSeleccionado = result.FullPath;
            }
        }

        [RelayCommand]
        public async Task ImportarAsync()
        {
            if (string.IsNullOrEmpty(ArchivoSeleccionado))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Selecciona un archivo primero.", "OK");
                return;
            }

            try
            {
                var json = File.ReadAllText(ArchivoSeleccionado);
                var lista = JsonConvert.DeserializeObject<List<Empleado>>(json) ?? new();
                int count = await App.DbService.ImportarEmpleadosAsync(lista);
                await Application.Current.MainPage.DisplayAlert("Importación", $"Registros procesados: {count}", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"No se pudo importar: {ex.Message}", "OK");
            }
        }
    }
}

