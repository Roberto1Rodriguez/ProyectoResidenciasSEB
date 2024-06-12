using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoResidenciasApi.Models;

namespace ProyectoResidenciasApi.Data;

public partial class Sistem21ResidenciasSebContext : DbContext
{
    public Sistem21ResidenciasSebContext()
    {
    }

    public Sistem21ResidenciasSebContext(DbContextOptions<Sistem21ResidenciasSebContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Alumno> Alumno { get; set; }

    public virtual DbSet<Asignatura> Asignatura { get; set; }

    public virtual DbSet<Camposformativos> Camposformativos { get; set; }

    public virtual DbSet<Docente> Docente { get; set; }

    public virtual DbSet<Examengenerado> Examengenerado { get; set; }

    public virtual DbSet<Historialexamen> Historialexamen { get; set; }

    public virtual DbSet<Lectura> Lectura { get; set; }

    public virtual DbSet<Pregunta> Pregunta { get; set; }

    public virtual DbSet<Respuesta> Respuesta { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Alumno>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("alumno");

            entity.HasIndex(e => e.UsuarioId, "UsuarioId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Grado).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Seccion).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasColumnType("int(11)");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Alumno)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("alumno_ibfk_1");
        });

        modelBuilder.Entity<Asignatura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("asignatura");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.NivelEducativo).HasColumnType("enum('Primaria','Secundaria')");
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<Camposformativos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("camposformativos");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre).HasMaxLength(255);
        });

        modelBuilder.Entity<Docente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("docente");

            entity.HasIndex(e => e.UsuarioId, "UsuarioId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.UsuarioId).HasColumnType("int(11)");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Docente)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("docente_ibfk_1");

            entity.HasMany(d => d.Alumno).WithMany(p => p.Docente)
                .UsingEntity<Dictionary<string, object>>(
                    "Docentealumno",
                    r => r.HasOne<Alumno>().WithMany()
                        .HasForeignKey("AlumnoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_alumno"),
                    l => l.HasOne<Docente>().WithMany()
                        .HasForeignKey("DocenteId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_docente"),
                    j =>
                    {
                        j.HasKey("DocenteId", "AlumnoId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("docentealumno");
                        j.HasIndex(new[] { "AlumnoId" }, "fk_alumno");
                        j.IndexerProperty<int>("DocenteId").HasColumnType("int(11)");
                        j.IndexerProperty<int>("AlumnoId").HasColumnType("int(11)");
                    });
        });

        modelBuilder.Entity<Examengenerado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("examengenerado");

            entity.HasIndex(e => e.DocenteId, "DocenteId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Asignatura).HasMaxLength(255);
            entity.Property(e => e.CampoFormativo).HasMaxLength(255);
            entity.Property(e => e.DocenteId).HasColumnType("int(11)");
            entity.Property(e => e.NivelEducativo).HasMaxLength(255);
            entity.Property(e => e.UbicacionPdf)
                .HasMaxLength(255)
                .HasColumnName("UbicacionPDF");

            entity.HasOne(d => d.Docente).WithMany(p => p.Examengenerado)
                .HasForeignKey(d => d.DocenteId)
                .HasConstraintName("examengenerado_ibfk_1");
        });

        modelBuilder.Entity<Historialexamen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("historialexamen");

            entity.HasIndex(e => e.AlumnoId, "AlumnoId");

            entity.HasIndex(e => e.ExamenGeneradoId, "ExamenGeneradoId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AlumnoId).HasColumnType("int(11)");
            entity.Property(e => e.ExamenGeneradoId).HasColumnType("int(11)");

            entity.HasOne(d => d.Alumno).WithMany(p => p.Historialexamen)
                .HasForeignKey(d => d.AlumnoId)
                .HasConstraintName("historialexamen_ibfk_1");

            entity.HasOne(d => d.ExamenGenerado).WithMany(p => p.Historialexamen)
                .HasForeignKey(d => d.ExamenGeneradoId)
                .HasConstraintName("historialexamen_ibfk_2");
        });

        modelBuilder.Entity<Lectura>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lectura");

            entity.HasIndex(e => e.AsignaturaId, "AsignaturaId");

            entity.HasIndex(e => e.CamposFormativosId, "CamposFormativosId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AsignaturaId).HasColumnType("int(11)");
            entity.Property(e => e.CamposFormativosId).HasColumnType("int(11)");
            entity.Property(e => e.Contenido).HasColumnType("text");
            entity.Property(e => e.NivelEducativo).HasColumnType("enum('Primaria','Secundaria')");
            entity.Property(e => e.Titulo).HasMaxLength(255);

            entity.HasOne(d => d.Asignatura).WithMany(p => p.Lectura)
                .HasForeignKey(d => d.AsignaturaId)
                .HasConstraintName("lectura_ibfk_1");

            entity.HasOne(d => d.CamposFormativos).WithMany(p => p.Lectura)
                .HasForeignKey(d => d.CamposFormativosId)
                .HasConstraintName("lectura_ibfk_2");
        });

        modelBuilder.Entity<Pregunta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pregunta");

            entity.HasIndex(e => e.AsignaturaId, "AsignaturaId");

            entity.HasIndex(e => e.CamposFormativosId, "CamposFormativosId");

            entity.HasIndex(e => e.LecturaId, "LecturaId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AsignaturaId).HasColumnType("int(11)");
            entity.Property(e => e.CamposFormativosId).HasColumnType("int(11)");
            entity.Property(e => e.LecturaId).HasColumnType("int(11)");
            entity.Property(e => e.NivelEducativo).HasColumnType("enum('Primaria','Secundaria')");
            entity.Property(e => e.Texto).HasColumnType("text");
            entity.Property(e => e.TipoPregunta).HasColumnType("enum('Abierta','Opción Múltiple','Falso-Verdadera','Multireactiva')");

            entity.HasOne(d => d.Asignatura).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.AsignaturaId)
                .HasConstraintName("pregunta_ibfk_1");

            entity.HasOne(d => d.CamposFormativos).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.CamposFormativosId)
                .HasConstraintName("pregunta_ibfk_2");

            entity.HasOne(d => d.Lectura).WithMany(p => p.Pregunta)
                .HasForeignKey(d => d.LecturaId)
                .HasConstraintName("pregunta_ibfk_3");
        });

        modelBuilder.Entity<Respuesta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("respuesta");

            entity.HasIndex(e => e.PreguntaId, "PreguntaId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.PreguntaId).HasColumnType("int(11)");
            entity.Property(e => e.Texto).HasColumnType("text");

            entity.HasOne(d => d.Pregunta).WithMany(p => p.Respuesta)
                .HasForeignKey(d => d.PreguntaId)
                .HasConstraintName("respuesta_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("usuario");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.NombreUsuario).HasMaxLength(255);
            entity.Property(e => e.Tipo).HasColumnType("enum('A','D')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
