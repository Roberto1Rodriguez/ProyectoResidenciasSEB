using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public string Tipo { get; set; } = null!;

    public virtual ICollection<Alumno> Alumno { get; set; } = new List<Alumno>();

    public virtual ICollection<Docente> Docente { get; set; } = new List<Docente>();
}
