using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Calificacion;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetenciaController : ControllerBase
    {
        private readonly DbEscuelasContext _context;

        public CompetenciaController(DbEscuelasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Competencia>>> GetCompetencias()
        {
            try
            {
                var competencias = await _context.Competencia
                    .Select(c => new Competencia
                    {
                        Id = c.Id,
                        Titulo = c.Titulo,
                        Descripcion = c.Descripcion,
                        Fecha_Inicio = c.Fecha_Inicio,
                        Fecha_Fin = c.Fecha_Fin
                    })
                    .ToListAsync();

                if (competencias == null || !competencias.Any())
                {
                    return NotFound("No hay competencias disponibles.");
                }

                return Ok(competencias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
