using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AspFirstMVC.Models
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

        public virtual DbSet<Alumno> Alumnos { get; set; }
        public virtual DbSet<Cerveza> Cervezas { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<Paise> Paises { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=gigabyte-sabre\\sqlexpress;Database=pruebas;integrated security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.ToTable("alumnos");

                entity.HasIndex(e => e.Dni, "IX_Dni")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "IX_alumnos")
                    .IsUnique();

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Dni)
                    .IsRequired()
                    .HasMaxLength(9);

                entity.Property(e => e.EstadoCivil).HasMaxLength(50);

                entity.Property(e => e.FechaNacimiento)
                    .IsRequired()
                    .HasMaxLength(8)
                    .IsFixedLength(true);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.PaisNavigation)
                    .WithMany(p => p.Alumnos)
                    .HasForeignKey(d => d.Pais)
                    .HasConstraintName("FK_alumnos_paises");
            });

            modelBuilder.Entity<Cerveza>(entity =>
            {
                entity.Property(e => e.Marca)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.FileDb).HasColumnName("file.db");

                entity.Property(e => e.Path)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("path");
            });

            modelBuilder.Entity<Paise>(entity =>
            {
                entity.ToTable("paises");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
