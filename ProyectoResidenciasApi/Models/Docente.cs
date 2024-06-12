using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Docente
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Examengenerado> Examengenerado { get; set; } = new List<Examengenerado>();

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Alumno> Alumno { get; set; } = new List<Alumno>();
}
