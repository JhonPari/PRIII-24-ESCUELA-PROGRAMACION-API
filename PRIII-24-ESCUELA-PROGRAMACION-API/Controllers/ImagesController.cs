using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImagesController : Controller
    {
        private readonly GraphServiceClient _graphClient;
        private readonly DbEscuelasContext _context;

        public ImagesController(GraphServiceClient graphClient, DbEscuelasContext context)
        {
            _graphClient = graphClient;
            _context = context;
        }

        [HttpPost("subirImagen")]
        public async Task<IActionResult> SubirPrueba([FromForm] SubirImagen imagenSubida)
        {
            var calificacion = await _context.calificacion.FirstOrDefaultAsync(x => x.IdCompetencia == imagenSubida.idCompetencia && x.IdEstudiante == imagenSubida.idEstudiante);

            if (calificacion == null)
                return NotFound();

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png"};
            var extension = Path.GetExtension(imagenSubida.imagen.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Formato de imagen no permitido.");
            }

            string? ImgUrl = await SubirImagen(imagenSubida.imagen);

            if (ImgUrl == null)
                return BadRequest("Debe proporcionar una imagen.");

            calificacion.Prueba_Estudiante = ImgUrl;
            _context.calificacion.Update(calificacion);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<string?> SubirImagen(IFormFile imagen)
        {
            if (imagen == null || imagen.Length == 0)
                return null;

            var nombreUnico = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";

            using (var stream = imagen.OpenReadStream())
            {
                var uploadedFile = await _graphClient.Drive.Special.AppRoot
                    .ItemWithPath(nombreUnico)
                    .Content
                    .Request()
                    .PutAsync<DriveItem>(stream);

                return uploadedFile.WebUrl;
            }
        }
    }
}
