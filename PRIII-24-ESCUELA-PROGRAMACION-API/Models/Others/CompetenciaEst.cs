namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class CompetenciaEst
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
        public char Estado { get; set; }
        public UInt16 Imagen { get; set; }
        public UInt16 Revisado { get; set; }
    }
}
