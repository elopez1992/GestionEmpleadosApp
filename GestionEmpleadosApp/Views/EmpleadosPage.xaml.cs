namespace GestionEmpleadosApp.Views
{
    public partial class EmpleadosPage : ContentPage
    {
        public EmpleadosPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ViewModels.EmpleadosViewModel vm)
                await vm.CargarAsync();
        }
    }
}
