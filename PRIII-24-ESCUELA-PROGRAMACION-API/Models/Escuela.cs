using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models. Calificacion;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Models
{

    public class Escuela
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [Required]
        [Description("auto")]
        public DateTime fecha_Registro { get; set; }

        public DateTime? fecha_Actualizacion { get; set; }

        [Required]
        [DefaultValue('A')]
        [Description("A,E")]
        public char Estado { get; set; } = 'A';

        
    }
}
