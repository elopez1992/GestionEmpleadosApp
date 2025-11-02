using GestionEmpleadosApp.Services;

namespace GestionEmpleadosApp
{
    public partial class App : Application
    {
        public static DatabaseService DbService { get; private set; }

        public App()
        {
            InitializeComponent();
            DbService = new DatabaseService();
            MainPage = new AppShell();

            // ✅ Inicializar la base de datos aquí
            Task.Run(async () => await DbService.InitializeAsync()).Wait();

        }

        
    }
}
