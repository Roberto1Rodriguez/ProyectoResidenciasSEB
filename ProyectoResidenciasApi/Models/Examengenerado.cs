using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Examengenerado
{
    public int Id { get; set; }

    public int? DocenteId { get; set; }

    public string? UbicacionPdf { get; set; }

    public string? NivelEducativo { get; set; }

    public string? CampoFormativo { get; set; }

    public string? Asignatura { get; set; }

    public DateOnly? Fecha { get; set; }

    public virtual Docente? Docente { get; set; }

    public virtual ICollection<Examenpregunta> Examenpregunta { get; set; } = new List<Examenpregunta>();

    public virtual ICollection<Historialexamen> Historialexamen { get; set; } = new List<Historialexamen>();
}
