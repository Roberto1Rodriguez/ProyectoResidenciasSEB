using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Asignatura
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string NivelEducativo { get; set; } = null!;

    public virtual ICollection<Lectura> Lectura { get; set; } = new List<Lectura>();

    public virtual ICollection<Pregunta> Pregunta { get; set; } = new List<Pregunta>();
}
