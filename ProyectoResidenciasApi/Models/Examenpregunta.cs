using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Examenpregunta
{
    public int Id { get; set; }

    public int ExamenGeneradoId { get; set; }

    public int PreguntaId { get; set; }

    public virtual Examengenerado ExamenGenerado { get; set; } = null!;

    public virtual Pregunta Pregunta { get; set; } = null!;
}
