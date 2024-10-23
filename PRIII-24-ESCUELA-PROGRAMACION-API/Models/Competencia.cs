namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{
    public class Competencia
    {
        public UInt32 Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; } 
        public UInt16 Puntos { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public char Estado { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }

}
