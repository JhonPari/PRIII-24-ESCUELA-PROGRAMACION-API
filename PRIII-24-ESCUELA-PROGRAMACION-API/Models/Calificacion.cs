namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{
    public class Calificacion
    {
		public int Id { get; set; }
		public int IdCompetencia { get; set; }
		public UInt32 IdEstudiante { get; set; }
		public string? Prueba_Estudiante { get; set; }
		public UInt16 Aprobado { get; set; }

		public UInt32 IdCalificador { get; set; }
		public Usuario Estudiante { get; set; }
		public Competencia Competencia { get; set; }
	}
}
