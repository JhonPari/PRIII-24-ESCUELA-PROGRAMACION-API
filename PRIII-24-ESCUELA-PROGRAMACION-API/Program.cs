using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using PRIII_24_ESCUELA_PROGRAMACION_API.Data;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


var clientId = builder.Configuration.GetSection("ImagenesOneDrive")["clientId"];
var tenantId = builder.Configuration.GetSection("ImagenesOneDrive")["tenantId"];
var clientSecret = builder.Configuration.GetSection("ImagenesOneDrive")["clientSecret"];

builder.Services.AddSingleton<GraphServiceClient>(sp =>
{
    var confidentialClient = ConfidentialClientApplicationBuilder.Create(clientId)
        .WithClientSecret(clientSecret)
        .WithTenantId(tenantId)
        .Build();

    return new GraphServiceClient(new CustomAuthenticationProvider(confidentialClient));
});


// Add services to the container.
builder.Services.AddDbContext<DbEscuelasContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add CORS services
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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
