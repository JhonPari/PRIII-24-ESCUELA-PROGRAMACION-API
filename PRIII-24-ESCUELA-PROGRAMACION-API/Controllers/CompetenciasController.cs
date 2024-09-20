using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompetenciasController : ControllerBase
    {
        private readonly DbEscuelasContext _context;

        public CompetenciasController(DbEscuelasContext context)
        {
            _context = context;
        }

        [HttpGet("Est")]
        public async Task<ActionResult<IEnumerable<CalificacionCompetencia>>> GetCompetenciasEst(int idEstudiante)
        {
            var competencias = await _context.CompetenciasEst(idEstudiante);

            if (competencias == null)
            {
                return NotFound();
            }

            return Ok(competencias);
        }
    }
}
