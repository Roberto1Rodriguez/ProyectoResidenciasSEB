using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Lectura
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string Contenido { get; set; } = null!;

    public string? NivelEducativo { get; set; }

    public int? AsignaturaId { get; set; }

    public int? CamposFormativosId { get; set; }

    public virtual Asignatura? Asignatura { get; set; }

    public virtual Camposformativos? CamposFormativos { get; set; }

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
