using SQLite;
using System;

namespace GestionEmpleadosApp.Models
{
    [Table("Empleados")]
    public class Empleado
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(200), NotNull]
        public string Nombre { get; set; }

        [NotNull]
        public DateTime FechaIngreso { get; set; }

        [MaxLength(150)]
        public string Puesto { get; set; }

        [MaxLength(200)]
        public string Correo { get; set; }

        // Almacenar la imagen en Base64
        public string FotoBase64 { get; set; }
    }
}

