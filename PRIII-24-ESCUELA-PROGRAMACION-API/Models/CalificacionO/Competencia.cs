
namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.CalificacionO


{
   
    public class Competencia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}
