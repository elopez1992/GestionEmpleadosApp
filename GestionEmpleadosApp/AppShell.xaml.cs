using GestionEmpleadosApp.Views;

namespace GestionEmpleadosApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // ✅ Aquí registras las rutas
            Routing.RegisterRoute("DetalleEmpleadoPage", typeof(DetalleEmpleadoPage));
            Routing.RegisterRoute("ImportarPage", typeof(ImportarPage));
        }
    }
}

