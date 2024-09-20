using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Calificacion;

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
        public DbSet<Calificacion> Calificacion { get; set; }
        public DbSet<Competencia> Competencia { get; set; }

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

            

            modelBuilder.Entity<Calificacion>()
             .HasOne(c => c.Estudiante)
             .WithMany()
             .HasForeignKey(c => c.IdEstudiante);

            modelBuilder.Entity<Calificacion>()
           .HasOne(c => c.Competencia)
           .WithMany() // Asumiendo que no hay una colección inversa
           .HasForeignKey(c => c.IdCompetencia);

            modelBuilder.Entity<Calificacion>()
                .HasOne(c => c.Estudiante)
                .WithMany() // Asumiendo que no hay una colección inversa
                .HasForeignKey(c => c.IdEstudiante);





            base.OnModelCreating(modelBuilder);




          
        }



    }
    
}
