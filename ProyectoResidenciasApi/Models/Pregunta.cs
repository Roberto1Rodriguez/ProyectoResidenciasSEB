using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Pregunta
{
    public int Id { get; set; }

    public string Texto { get; set; } = null!;

    public string TipoPregunta { get; set; } = null!;

    public string? NivelEducativo { get; set; }

    public int? AsignaturaId { get; set; }

    public int? CamposFormativosId { get; set; }

    public int? LecturaId { get; set; }

    public virtual Asignatura? Asignatura { get; set; }

    public virtual Camposformativos? CamposFormativos { get; set; }

    public virtual ICollection<Examenpregunta> Examenpregunta { get; set; } = new List<Examenpregunta>();

    public virtual Lectura? Lectura { get; set; }

    public virtual ICollection<Respuesta> Respuesta { get; set; } = new List<Respuesta>();
}
