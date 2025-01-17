﻿using Microsoft.EntityFrameworkCore;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models;
using PRIII_24_ESCUELA_PROGRAMACION_API.Models.Others;

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
        public DbSet<Competencia> competencia { get; set; }
        public DbSet<Calificacion> calificacion { get; set; }
        public DbSet<Imagen> Imagen { get; set; }

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
            modelBuilder.Entity<Imagen>(entity =>
            {
                // Configurar clave primaria
                entity.HasKey(e => e.idCalificacion);
            });
            modelBuilder.Entity<Calificacion>()
			   .HasOne(c => c.Competencia)
			   .WithMany(c => c.Calificaciones)
			   .HasForeignKey(c => c.IdCompetencia);

			modelBuilder.Entity<Calificacion>()
				.HasOne(c => c.Estudiante)
				.WithMany() // Assuming Estudiante does not have a collection of Calificaciones
				.HasForeignKey(c => c.IdEstudiante);
        }
    }
}
