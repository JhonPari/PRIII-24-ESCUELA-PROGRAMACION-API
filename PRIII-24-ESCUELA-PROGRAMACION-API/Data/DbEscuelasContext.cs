using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;

namespace PRIII_24_ESCUELA_PROGRAMACION_API.Data
{
    public class DbEscuelasContext : DbContext
    {
        public DbEscuelasContext()
        {
        }

        public DbEscuelasContext(DbContextOptions<DbEscuelasContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Escuela> escuela { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                // Configurar clave primaria
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Estado)
                    .HasDefaultValue('A'); // Valor por defecto 'A'

                entity.Property(e => e.Solicitud)
                    .HasDefaultValue('P'); // Valor por defecto 'P'
            });
            // Configuración para la entidad Escuela
            modelBuilder.Entity<Escuela>(entity =>
            {
                // Configurar clave primaria
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Estado)
                    .HasDefaultValue('A'); // Valor por defecto 'A'
            });
        }
    }
}
