using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Calificacion
{
    public class Calificacion
    {
        public int Id { get; set; }
        public uint IdCompetencia { get; set; } // MEDIUMINT UNSIGNED
        public uint IdEstudiante { get; set; } // MEDIUMINT UNSIGNED
        public string? Prueba_Estudiante { get; set; } // BLOB
        public int Aprobado { get; set; } // TINYINT
        public uint IdCalificador { get; set; } // MEDIUMINT UNSIGNED


        public Usuario Estudiante { get; set; }
        public Competencia Competencia { get; set; }

    }
}
