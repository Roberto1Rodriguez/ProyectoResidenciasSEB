using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Respuesta
{
    public int Id { get; set; }

    public string Texto { get; set; } = null!;

    public bool? EsCorrecta { get; set; }

    public int? PreguntaId { get; set; }

    public virtual Pregunta? Pregunta { get; set; }
}
