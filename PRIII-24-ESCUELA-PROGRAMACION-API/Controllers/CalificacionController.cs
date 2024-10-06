using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
	public class CalificacionController : ControllerBase
	{
		private readonly DbEscuelasContext _context;

		public CalificacionController(DbEscuelasContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Calificacion>>> GetCalificaciones()
		{
			try
			{
				var calificaciones = await _context.calificacion.ToListAsync();

				if (calificaciones == null || !calificaciones.Any())
				{
					return NotFound("No hay caificaciones.");
				}
				return Ok(calificaciones);
			}
			catch (Exception ex)
			{

				return StatusCode(500, $"Error: {ex.Message}");
			}
		}

		[HttpGet("FiltrarEstudiante/{competenciaId}")]
		public async Task<ActionResult<IEnumerable<CalificacionDto>>> GetCalificacionesConEstudiante(int competenciaId)
		{
			try
			{
				var calificaciones = await _context.calificacion
					.Include(c => c.Estudiante)
					.Where(c => c.IdCompetencia == competenciaId)
					.Select(c => new CalificacionDto
					{
						Id = c.Id,
						Nombre = c.Estudiante.Nombre,
						Correo = c.Estudiante.Correo,
						Aprobado = c.Aprobado
					})
					.ToListAsync();

				return Ok(calificaciones);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error: {ex.Message}");
			}
		}

		//[HttpGet("with-student-info")]
		//public async Task<ActionResult<IEnumerable<CalificacionDto>>> GetCalificacionesConEstudiante()
		//{
		//    try
		//    {
		//        var calificaciones = await _context.Calificacion
		//            .Include(c => c.Estudiante)
		//            .Select(c => new CalificacionDto
		//            {
		//                Id = c.Id,
		//                Nombre = c.Estudiante.Nombre,
		//                Correo = c.Estudiante.Correo,
		//                Aprobado = c.Aprobado
		//            })
		//            .ToListAsync();

		//        return Ok(calificaciones);
		//    }
		//    catch (Exception ex)
		//    {
		//        return StatusCode(500, $"Error: {ex.Message}");
		//    }
		//}

		[HttpGet("{id}")]
		public async Task<ActionResult<CalificacionDto>> GetCalificacion(int id)
		{
			var calificacion = await _context.calificacion
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

		[HttpPut("{id}/Calificar")]
		public async Task<IActionResult> UpdateCalificacion(int id, [FromBody] CalificarAprobado calificacionUpdate)
		{
			if (id != calificacionUpdate.Id)
			{
				return BadRequest("ID mismatch");
			}

			var calificacion = await _context.calificacion.FindAsync(id);

			if (calificacion == null)
			{
				return NotFound();
			}

			calificacion.Aprobado = calificacionUpdate.Aprobado;

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
			return _context.calificacion.Any(e => e.Id == id);
		}



	}
}
