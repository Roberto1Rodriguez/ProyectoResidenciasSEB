using System;
using System.Collections.Generic;

namespace ProyectoResidenciasApi.Models;

public partial class Alumno
{
    public int Id { get; set; }

    public int? UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Grado { get; set; } = null!;

    public string Seccion { get; set; } = null!;

    public virtual ICollection<Historialexamen> Historialexamen { get; set; } = new List<Historialexamen>();

    public virtual Usuario? Usuario { get; set; }

    public virtual ICollection<Docente> Docente { get; set; } = new List<Docente>();
}
