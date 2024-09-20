using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt32 Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Contrasenia { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        [Description("E,D,A")]
        public char Rol { get; set; }

        [Required]
        public UInt32 IdUsuario { get; set; }

        [Required]
        [Description("auto")]
        public DateTime fecha_Registro { get; set; }

        public DateTime? fecha_Actualizacion { get; set; }

        [Required]
        [DefaultValue('A')]
        [Description("A,E")]
        public char Estado { get; set; } = 'A';

        [Required]
        [DefaultValue('P')]  //'A', 'R'
        [Description("P,A,R")]
        public char Solicitud { get; set; } = 'P';
        





    }
}
