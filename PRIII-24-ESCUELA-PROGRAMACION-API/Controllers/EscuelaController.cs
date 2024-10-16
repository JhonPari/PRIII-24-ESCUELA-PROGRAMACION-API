using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EscuelaController : ControllerBase
    {
        private readonly DbEscuelasContext _context;

        public EscuelaController(DbEscuelasContext context)
        {
            _context = context;
        }

        //// GET: api/<EscuelaController>
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Escuela>>> GetEscuelas()
        //{
        //    var escuelas = await _context.escuela.ToListAsync();
        //    return Ok(escuelas);
        //}
        // GET: api/<EscuelaController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Escuela>>> GetEscuelas()
        {
            var escuelas = await _context.escuela
                                         .Where(e => e.Estado == 'A')
                                         .ToListAsync();
            return Ok(escuelas);
        }

        // GET api/<EscuelaController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Escuela>> GetEscuela(int id)
        {
            var escuela = await _context.escuela.FindAsync(id);

            if (escuela == null)
            {
                return NotFound();
            }

            return escuela;
        }

        // POST api/<EscuelaController>
        [HttpPost]
        public async Task<ActionResult<Escuela>> PostEscuela(Escuela escuela)
        {// Aquí puedes omitir la verificación del ID ya que estamos creando una nueva entidad
         // y no debería haber un ID preexistente.

            // Asignar la fecha de registro
            escuela.fecha_Registro = DateTime.Now;

            // Agregar la nueva escuela al contexto
            _context.escuela.Add(escuela);
            await _context.SaveChangesAsync();

            // Retornar la acción creada con la nueva escuela
            return CreatedAtAction(nameof(GetEscuela), new { id = escuela.Id }, escuela);
        }

        // PUT api/<EscuelaController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEscuela(int id, Escuela escuela)
        {
            if (id != escuela.Id)
            {
                return BadRequest(new { message = "ID mismatch." });
            }

            escuela.fecha_Actualizacion = DateTime.Now;
            if (escuela.fecha_Registro == DateTime.MinValue)
            {
                escuela.fecha_Registro = DateTime.Now; // Asegurar que no se establezca una fecha inválida
            }

            _context.Entry(escuela).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EscuelaExists(id))
                {
                    return NotFound(new { message = "Escuela not found." });
                }
                else
                {
                    return StatusCode(500, new { message = "A concurrency error occurred." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }

            return Ok();
        }



        //// DELETE api/<EscuelaController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEscuelas(int id)
        //{
        //    var usuario = await _context.escuela.FindAsync(id);
        //    if (usuario == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.escuela.Remove(usuario);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        [HttpPut("{id}/logic-delete")]
        public async Task<IActionResult> DeleteEscuelas(int id)
        {
            var escuela = await _context.escuela.FindAsync(id);
            if (escuela == null)
            {
                return NotFound();
            }

            escuela.Estado = 'E'; // Cambiar el estado a 'E'
            _context.Entry(escuela).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EscuelaExists(int id)
        {
            return _context.escuela.Any(e => e.Id == id);
        }
    }
}
