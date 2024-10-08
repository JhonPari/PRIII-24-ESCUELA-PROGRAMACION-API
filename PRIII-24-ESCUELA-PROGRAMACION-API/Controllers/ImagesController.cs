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
            await _context.SaveChangesAsync();

            return Ok();
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
