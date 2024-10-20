namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class ImagenResponse
    {
        public int IdCalificacion { get; set; }
        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }
        public string ContentType { get; set; }
    }
}
