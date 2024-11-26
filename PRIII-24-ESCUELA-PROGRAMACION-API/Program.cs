using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

// Obtener la cadena de conexi�n de configuraci�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Agregar DbContext para conectar a la base de datos MySQL
builder.Services.AddDbContext<DbEscuelasContext>(options =>
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Agregar soporte para CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins",
		builder =>
		{
			builder.AllowAnyOrigin()
				   .AllowAnyMethod()
				   .AllowAnyHeader();
		});
});

builder.Services.AddControllers();
// Configuraci�n de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
	// Habilitar Swagger solo en entorno de desarrollo
	app.UseSwagger();
	app.UseSwaggerUI();
}

// Desactivar la redirecci�n HTTPS en desarrollo
// Elimina la llamada a app.UseHttpsRedirection() si no est�s usando HTTPS
if (!app.Environment.IsDevelopment())
{
	// Si no est�s en desarrollo, habilitar HTTPS en producci�n
	app.UseHttpsRedirection();
}

app.UseCors("AllowAllOrigins"); // Habilitar CORS
app.UseAuthorization(); // Activar autorizaci�n

app.MapControllers(); // Mapear los controladores

app.Run(); // Ejecutar la aplicaci�n
