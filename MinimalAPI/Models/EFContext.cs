using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MinimalAPI.Models
{
    public partial class EFContext : DbContext
    {
        public EFContext()
        {
        }

        public EFContext(DbContextOptions<EFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Alumno> Alumnos { get; set; } = null!;
        public virtual DbSet<Cerveza> Cervezas { get; set; } = null!;
        public virtual DbSet<Paise> Paises { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.ToTable("alumnos");

                entity.HasIndex(e => e.Dni, "IX_Dni")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "IX_alumnos")
                    .IsUnique();

                entity.Property(e => e.Apellidos).HasMaxLength(50);

                entity.Property(e => e.Dni).HasMaxLength(9);

                entity.Property(e => e.EstadoCivil).HasMaxLength(50);

                entity.Property(e => e.FechaNacimiento)
                    .HasMaxLength(8)
                    .IsFixedLength();

                entity.Property(e => e.Nombre).HasMaxLength(50);

                entity.HasOne(d => d.PaisNavigation)
                    .WithMany(p => p.Alumnos)
                    .HasForeignKey(d => d.Pais)
                    .HasConstraintName("FK_alumnos_paises");
            });

            modelBuilder.Entity<Cerveza>(entity =>
            {
                entity.Property(e => e.Marca).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<Paise>(entity =>
            {
                entity.ToTable("paises");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
