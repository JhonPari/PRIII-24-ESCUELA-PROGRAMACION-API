using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : Controller
    {
        private readonly DbEscuelasContext _context;

        public ImagesController(DbEscuelasContext context)
        {
            _context = context;
        }

        [HttpPost("subirImagen")]
        public async Task<IActionResult> SubirPrueba([FromForm] SubirImagen imagenSubida)
        {
            var calificacion = await _context.calificacion.FirstOrDefaultAsync(x => x.IdCompetencia == imagenSubida.idCompetencia && x.IdEstudiante == imagenSubida.idEstudiante);
            if (calificacion == null)
                return NotFound();

            var imagen = await _context.Imagen.FirstOrDefaultAsync(x => x.idCalificacion == calificacion.Id);
            if (imagen != null)
                return BadRequest("Ya se subio una imagen anteriormente");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(imagenSubida.imagen.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Formato de imagen no permitido.");
            }

            Imagen img = new Imagen
            {
                idCalificacion = calificacion.Id,
                nombre = $"{Guid.NewGuid()}{Path.GetExtension(imagenSubida.imagen.FileName)}",
                archivo = await ToBytesAsync(imagenSubida.imagen)
            };

            _context.Imagen.Add(img);

            calificacion.Imagen = 1;
            _context.calificacion.Update(calificacion);

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("obtenerImagen/{idCalificacion}")]
        public async Task<IActionResult> ObtenerImagen(int idCalificacion)
        {
            var calificacion = await _context.calificacion.FindAsync(idCalificacion);
            if (calificacion == null)
                return NotFound("Registro no encontrado");

            // Buscar la imagen en la base de datos por el idCalificacion
            var imagen = await _context.Imagen.FindAsync(idCalificacion);

            if (imagen == null)
                return NotFound("Imagen no encontrada");

            // Determinar el tipo de archivo basado en la extensión
            var extension = Path.GetExtension(imagen.nombre).ToLower();
            string contentType;

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                default:
                    return BadRequest("Formato de imagen no permitido.");
            }

            // Devolver la imagen como archivo descargable
            return File(imagen.archivo, contentType, imagen.nombre);

            //var response = new ImagenResponse
            //{
            //    IdCalificacion = idCalificacion,
            //    Nombre = imagen.nombre,
            //    Archivo = imagen.archivo,
            //    ContentType = contentType
            //};

            //return Ok(response);
        }

        private async Task<byte[]> ToBytesAsync(IFormFile img)
        {
            // Leer el contenido del archivo en un MemoryStream
            using (var memoryStream = new MemoryStream())
            {
                await img.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray(); // Aquí obtienes los bytes del archivo

                return fileBytes;
            }
        }
    }
}
