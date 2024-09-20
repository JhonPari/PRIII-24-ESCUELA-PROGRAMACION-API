using System.ComponentModel.DataAnnotations;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class RecuperaContrasenia
    {
        [Required]
        public string Correo { get; set; }
    }
}
