using System.ComponentModel.DataAnnotations;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others
{
    public class LoginRequest
    {
        [Required]
        public string Correo { get; set; }
        [Required]
        public string Contrasenia { get; set; }
    }
}
