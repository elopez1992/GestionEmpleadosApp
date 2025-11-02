using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionEmpleadosApp.Models;
using System.Collections.ObjectModel;

namespace GestionEmpleadosApp.ViewModels
{
    public partial class EmpleadosViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Empleado> empleados = new();

        [ObservableProperty]
        private bool isBusy;

        public EmpleadosViewModel() { }

        // Cargar lista de empleados
        [RelayCommand]
        public async Task CargarAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var lista = await App.DbService.GetEmpleadosAsync();
                Empleados.Clear();
                foreach (var e in lista) Empleados.Add(e);
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Navegar a página de nuevo empleado
        [RelayCommand]
        public async Task NuevoAsync()
        {
            await Shell.Current.GoToAsync("DetalleEmpleadoPage");
        }

        // Navegar a página de edición con parámetros
        [RelayCommand]
        public async Task EditarAsync(Empleado empleado)
        {
            if (empleado == null) return;
            var parametros = new Dictionary<string, object>
            {
                { "Empleado", empleado }
            };
            await Shell.Current.GoToAsync("DetalleEmpleadoPage", parametros);
        }

        // Eliminar empleado
        [RelayCommand]
        public async Task EliminarAsync(Empleado empleado)
        {
            if (empleado == null) return;
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Eliminar",
                $"¿Eliminar a {empleado.Nombre}?",
                "Sí",
                "No");

            if (!confirmar) return;

            await App.DbService.DeleteEmpleadoAsync(empleado);
            await CargarAsync();
        }

        // Navegar a página de importación
        [RelayCommand]
        public async Task IrImportarAsync()
        {
            await Shell.Current.GoToAsync("ImportarPage");
        }

        // Guardar todos los cambios y nuevos registros
        [RelayCommand]
        public async Task GuardarTodoAsync()
        {
            foreach (var empleado in Empleados)
            {
                await App.DbService.SaveEmpleadoAsync(empleado);
            }

            await Application.Current.MainPage.DisplayAlert(
                "Éxito",
                "Todos los cambios y nuevos registros se han guardado.",
                "OK");
        }
    }
}
