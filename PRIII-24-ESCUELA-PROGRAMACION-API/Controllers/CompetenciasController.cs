using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
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

        [HttpGet("Competencias")]
        public async Task<ActionResult<IEnumerable<CalificacionCompetencia>>> GetCompetenciasEst(int idEstudiante)
        {
            var hoy = DateTime.Now;
            var competencias = await (from c in _context.competencia
                                      join ca in _context.calificacion on c.Id equals ca.IdCompetencia
                                      where ca.IdEstudiante == idEstudiante
                                      select new CompetenciaEst
                                      {
                                          Id = c.Id,
                                          Titulo = c.Titulo,
                                          Fecha_Inicio = c.Fecha_Inicio,
                                          Fecha_Fin = c.Fecha_Fin,
                                          Estado = c.Estado,
                                          Imagen = ca.Imagen,
                                          Revisado = ca.Aprobado
                                      })
                                      .OrderByDescending(c => hoy >= c.Fecha_Inicio && hoy <= c.Fecha_Fin)
                                      .ThenBy(c => c.Fecha_Inicio >= hoy ? c.Fecha_Inicio : DateTime.MaxValue)
                                      .ThenByDescending(c => c.Fecha_Fin < hoy ? c.Fecha_Fin : DateTime.MinValue)
                                      .ToListAsync();

            if (competencias == null)
            {
                return NotFound();
            }

            return Ok(competencias);
        }

        [HttpGet("Logros")]
        public async Task<ActionResult<IEnumerable<CalificacionCompetencia>>> GetLogrosEst(int idEstudiante)
        {
            var competencias = await (from c in _context.competencia
                                      join ca in _context.calificacion on c.Id equals ca.IdCompetencia
                                      where ca.IdEstudiante == idEstudiante
                                      select new CalificacionCompetencia
                                      {
                                          Id = ca.Id,
                                          Titulo = c.Titulo,
                                          //prueba_Estudiante = ca.Prueba_Estudiante,
                                          FechaInicio = c.Fecha_Inicio,
                                          Aprobado = ca.Aprobado,
                                          Imagen = ca.Imagen
                                      }).ToListAsync();

            if (competencias == null)
            {
                return NotFound();
            }

            return Ok(competencias);
        }
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Competencia>>> GetCompetencias()
		{
			try
			{
				var competencias = await _context.competencia
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
