using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Repositories;

namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlumnoController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Usuario> repoUsuario;
        Repository<Alumno> repoAlumno;
        Repository<Docente> repoDocente;
        public AlumnoController(Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            repoAlumno = new Repository<Alumno>(context);
            repoUsuario = new Repository<Usuario>(context);
            repoDocente = new Repository<Docente>(context);
        }
        [HttpPost("CrearAlumno")]
        public IActionResult CrearConUsuario(AlumnoDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Datos del alumno no proporcionados");
                }

                // Crear usuario
                Usuario usuario = new Usuario()
                {
                    NombreUsuario = dto.NombreUsuario,
                    Email = dto.Email,
                    Contraseña = dto.Contraseña, // Asegúrate de manejar el hash de la contraseña en una implementación real
                    Tipo = "A"
                };
                repoUsuario.Insert(usuario);

                // Crear alumno
                Alumno alumno = new Alumno()
                {
                    Nombre = dto.Nombre,
                    UsuarioId = usuario.Id,
                    Grado = dto.Grado,
                    Seccion = dto.Seccion
                };
                repoAlumno.Insert(alumno);

                // Asociar alumno con docente
                var docente = repoDocente.Get().FirstOrDefault(a => a.Id == dto.DocenteId);
                if (docente == null)
                {
                    return NotFound("Docente no encontrado");
                }

                docente.Alumno.Add(alumno);
                repoDocente.Update(docente); // Asegúrate de que el repositorio maneje la actualización de relaciones correctamente

                return Ok(alumno);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
