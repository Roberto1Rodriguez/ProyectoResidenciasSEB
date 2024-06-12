using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Historialexamen
{
    public int Id { get; set; }

    public int? ExamenGeneradoId { get; set; }

    public int? AlumnoId { get; set; }

    public float? Calificacion { get; set; }

    public virtual Alumno? Alumno { get; set; }

    public virtual Examengenerado? ExamenGenerado { get; set; }
}
