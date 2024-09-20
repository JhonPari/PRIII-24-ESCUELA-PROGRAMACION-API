using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Calificacion;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CslificacionController : ControllerBase
    {
        private readonly DbEscuelasContext _context;

        public CslificacionController(DbEscuelasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificaciones()
        {
            try
            {
                var calificaciones = await _context.Calificacion.ToListAsync();
                // Maneja el caso en que no hay datos si es necesario
                if (calificaciones == null || !calificaciones.Any())
                {
                    return NotFound("No calificaciones found.");
                }
                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                // Manejo de errores mejorado
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("with-student-info")]
        public async Task<ActionResult<IEnumerable<CalificacionEstudianteDto>>> GetCalificacionesConEstudiante()
        {
            try
            {
                var calificaciones = await _context.Calificacion
                    .Include(c => c.Estudiante)
                    .Select(c => new CalificacionEstudianteDto
                    {
                        id = c.Id,
                        Nombre = c.Estudiante.Nombre,
                        Correo = c.Estudiante.Correo,
                        Aprobado = c.Aprobado
                    })
                    .ToListAsync();

                return Ok(calificaciones);
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CalificacionDto>> GetCalificacion(int id)
        {
            var calificacion = await _context.Calificacion
                .Include(c => c.Competencia)
                .Include(c => c.Estudiante)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (calificacion == null)
            {
                return NotFound();
            }

            var calificacionDto = new CalificacionDto
            {
                Id = calificacion.Id,
                CompetenciaTitulo = calificacion.Competencia.Titulo,
                EstudianteNombre = calificacion.Estudiante.Nombre
            };

            return Ok(calificacionDto);
        }

        public class CalificacionDto
        {
            public int Id { get; set; }
            public string CompetenciaTitulo { get; set; }
            public string EstudianteNombre { get; set; }
        }
        [HttpPut("{id}/Calificar")]
        public async Task<IActionResult> UpdateCalificacion(int id, [FromBody] UpdateCalificacionDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest("ID mismatch");
            }

            var calificacion = await _context.Calificacion.FindAsync(id);

            if (calificacion == null)
            {
                return NotFound();
            }

            calificacion.Aprobado = updateDto.Aprobado;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalificacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool CalificacionExists(int id)
        {
            return _context.Calificacion.Any(e => e.Id == id);
        }

        public class UpdateCalificacionDto
        {
            public int Id { get; set; }
            public int Aprobado { get; set; }
        }




    }
}
