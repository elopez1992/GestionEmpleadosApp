using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GestionEmpleadosApp.Models;

namespace GestionEmpleadosApp.Services
{
    public class DatabaseService
    {
        private static readonly Lazy<SQLiteAsyncConnection> lazyConnection =
            new(() =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "empleados.db3");
                var conn = new SQLiteAsyncConnection(dbPath);
                return conn;
            });

        private static SQLiteAsyncConnection Connection => lazyConnection.Value;

        private static bool _initialized = false;

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            await Connection.CreateTableAsync<Empleado>();
            _initialized = true;
        }

        public Task<List<Empleado>> GetEmpleadosAsync() =>
            Connection.Table<Empleado>().OrderBy(e => e.Nombre).ToListAsync();

        public Task<Empleado> GetEmpleadoAsync(int id) =>
            Connection.Table<Empleado>().Where(e => e.Id == id).FirstOrDefaultAsync();

        public Task<int> SaveEmpleadoAsync(Empleado empleado)
        {
            if (empleado.Id != 0)
                return Connection.UpdateAsync(empleado);
            else
                return Connection.InsertAsync(empleado);
        }

        public Task<int> DeleteEmpleadoAsync(Empleado empleado) =>
            Connection.DeleteAsync(empleado);

        // Importar lista completa (sobrescribe o agrega)
        public async Task<int> ImportarEmpleadosAsync(IEnumerable<Empleado> empleados)
        {
            int count = 0;
            foreach (var emp in empleados)
            {
                // Si viene con Id 0, se insertará nuevo; si no, intenta actualizar
                if (emp.Id != 0)
                {
                    var existente = await GetEmpleadoAsync(emp.Id);
                    if (existente is null) await Connection.InsertAsync(emp);
                    else await Connection.UpdateAsync(emp);
                }
                else
                {
                    await Connection.InsertAsync(emp);
                }
                count++;
            }
            return count;
        }
    }
}

