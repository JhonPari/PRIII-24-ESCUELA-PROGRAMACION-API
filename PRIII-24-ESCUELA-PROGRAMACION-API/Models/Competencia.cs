namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{
    public class Competencia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Puntos { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}
