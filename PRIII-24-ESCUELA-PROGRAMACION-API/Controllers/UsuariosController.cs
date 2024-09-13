using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DbEscuelasContext _context;

        public UsuariosController(DbEscuelasContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios
        [HttpGet("Estudiantes")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetEstudiantes()
        {
            return await _context.Usuarios.Where(x => x.Estado == 'A' && x.Rol == 'E').ToListAsync();
        }
        // GET: api/Docentes
        [HttpGet("Docentes")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetDocentes()
        {
            return await _context.Usuarios.Where(x => x.Estado == 'A' && x.Rol == 'D').ToListAsync();
        }
		// GET: api/listaPendienteDocentes
		[HttpGet("listaPendienteDocentes")]
		public async Task<ActionResult<IEnumerable<Usuario>>> GetListaPendientesDocente()
		{
			return await _context.Usuarios.Where(x => x.Estado == 'A' && x.Rol == 'D' && x.Solicitud == 'P').ToListAsync();
		}
		// GET: api/listaPendienteEstudiante
		[HttpGet("listaPendienteEstudiante")]
		public async Task<ActionResult<IEnumerable<Usuario>>> GetListaPendientesEstudiantes()
		{
			return await _context.Usuarios.Where(x => x.Estado == 'A' && x.Rol == 'E' && x.Solicitud == 'P').ToListAsync();
		}

		// GET: api/Usuarios/5
		[HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(uint id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.fecha_Registro = DateTime.Now;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(uint id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            usuario.fecha_Actualizacion = DateTime.Now;
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

		// PUT: api/Usuarios/archivarSolicitud/5
		[HttpPut("archivarSolicitud/{id}")]
		public async Task<IActionResult> ArchivarSolicitud(uint id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario == null)
			{
				return NotFound();
			}

			usuario.Solicitud = 'A';
			usuario.fecha_Actualizacion = DateTime.Now;
			_context.Entry(usuario).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UsuarioExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok();
		}
		// DELETE: api/Usuario/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(uint id)
        {
            Usuario? usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return BadRequest();
            }

            usuario.fecha_Actualizacion = DateTime.Now;
            usuario.Estado = 'I';
            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok();
        }


		private bool UsuarioExists(uint id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
