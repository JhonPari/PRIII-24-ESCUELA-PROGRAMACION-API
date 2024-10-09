namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class SubirImagen
    {
        public int idCompetencia {  get; set; }
        public int idEstudiante { get; set; }
        public IFormFile imagen {  get; set; }
    }
}
