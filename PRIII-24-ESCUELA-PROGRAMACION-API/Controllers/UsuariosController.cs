﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;
using System.Net.Mail;
using System.Net;
using System.Text;

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
		// GET: api/ReporteEstudiantes
		[HttpGet("EstudiantesReporte")]
		public async Task<ActionResult<IEnumerable<object>>> GetEstudiantesReporte()
		{
			var reporteEstudiantes = await (from u in _context.Usuarios
											join cal in _context.calificacion on u.Id equals cal.IdEstudiante
											join cmp in _context.competencia on cal.IdCompetencia equals cmp.Id
											where u.Estado == 'A' && u.Rol == 'E'
											group cmp by new { u.Nombre, u.Correo } into estudianteGrupo
											select new
											{
												Nombre_Estudiante = estudianteGrupo.Key.Nombre,
												Correo_Estudiante = estudianteGrupo.Key.Correo,
												Total_Puntos = estudianteGrupo.Sum(cmp => cmp.Puntos)
											}).ToListAsync();

			return Ok(reporteEstudiantes);
		}
		// GET: api/ReporteEstudiantesFecha
		[HttpGet("EstudiantesReporteFecha")]
		public async Task<ActionResult<IEnumerable<object>>> ReporteEstudiantesFecha()
		{
			var fechaLimite = DateTime.Now.AddMonths(-3);

			var reporteEstudiantesFecha = await (from u in _context.Usuarios
												 join cal in _context.calificacion on u.Id equals cal.IdEstudiante
												 join cmp in _context.competencia on cal.IdCompetencia equals cmp.Id
												 where u.Estado == 'A' && u.Rol == 'E'
												 && cmp.Fecha_Inicio >= fechaLimite
												 group new { cal, cmp } by new { u.Nombre, u.Correo, cmp.Fecha_Inicio } into estudianteGrupo
												 select new
												 {
													 NombreUsuario = estudianteGrupo.Key.Nombre,
													 CorreoUsuario = estudianteGrupo.Key.Correo,
													 TotalPuntos = estudianteGrupo.Sum(e => e.cmp.Puntos),
													 FechaInicioCompetencia = estudianteGrupo.Key.Fecha_Inicio // Sin formato
												 }).ToListAsync();

			return Ok(reporteEstudiantesFecha);
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

		[HttpPut("archivarSolicitud/{id}")]
		public async Task<IActionResult> ArchivarSolicitud(uint id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario == null)
			{
				return NotFound();
			}

			var nuevaContrasenia = GenerarContraseniaSegura();
			usuario.Contrasenia = HashPassword(nuevaContrasenia);
			usuario.Solicitud = 'A';
			usuario.fecha_Actualizacion = DateTime.Now;
			_context.Entry(usuario).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
				// Enviar correo con la nueva contraseña
				EnviarCorreo(usuario.Correo, nuevaContrasenia);
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

        // PUT: api/Usuarios/5
		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginRequest login)
		{
			// Buscar el usuario por correo
			Usuario? usuario = _context.Usuarios.FirstOrDefault(x => x.Correo == login.Correo && x.Estado == 'A' && x.Solicitud == 'A');


			if (usuario == null || !BCrypt.Net.BCrypt.Verify(login.Contrasenia, usuario.Contrasenia))
			{
				return NotFound();
			}

			return Ok(new
			{
				id = usuario.Id,
				name = usuario.Nombre,
				role = usuario.Rol
			});
		}

		private bool UsuarioExists(uint id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

		[HttpPost("RecuperarContrasenia")]
		public async Task<IActionResult> RecuperarContrasenia([FromBody] RecuperaContrasenia request)
		{
			var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Correo == request.Correo && x.Estado == 'A' && x.Solicitud == 'A');
			if (usuario == null)
			{
				return NotFound("Correo no registrado.");
			}
			var nuevaContrasenia = GenerarContraseniaSegura();
			usuario.Contrasenia = HashPassword(nuevaContrasenia);
			usuario.fecha_Actualizacion = DateTime.Now;
			_context.Usuarios.Update(usuario);
			await _context.SaveChangesAsync();
			EnviarCorreo(request.Correo, nuevaContrasenia);
			return Ok("Correo Enviado");
		}
		private string GenerarContraseniaSegura(int length = 8)
		{
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
			StringBuilder res = new StringBuilder();
			Random rnd = new Random();
			while (0 < length--)
			{
				res.Append(valid[rnd.Next(valid.Length)]);
			}
			return res.ToString();
		}
		private void EnviarCorreo(string toEmail, string nuevaContrasenia)
		{
			var fromEmail = "foxbolivia.fbol@gmail.com";
			var fromPassword = "qjuqxebmjhvcvtmr";
			var smtpClient = new SmtpClient("smtp.gmail.com")
			{
				Port = 587,
				Credentials = new NetworkCredential(fromEmail, fromPassword),
				EnableSsl = true,
			};
			var mailMessage = new MailMessage
			{
				From = new MailAddress(fromEmail),
				Subject = "Nueva Contraseña",
				Body = $"Su nueva contraseña es: {nuevaContrasenia}",
				IsBodyHtml = true,
			};
			mailMessage.To.Add(toEmail);
			smtpClient.Send(mailMessage);
		}
		private string HashPassword(string password)
		{
			return BCrypt.Net.BCrypt.HashPassword(password);
		}
	}
}
