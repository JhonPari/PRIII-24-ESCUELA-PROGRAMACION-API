namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int idCompentencia { get; set; }
        public Competencia Competencia { get; set; }
        public int IdEstudiante { get; set; }
        public UInt16 Aprobado { get; set; }
    }
}
