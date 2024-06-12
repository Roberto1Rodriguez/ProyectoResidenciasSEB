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
    public class UsuarioController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Usuario> repoUsuario;
        Repository<Docente> repoDocente;
        public UsuarioController(Sistem21ResidenciasSebContext context)
        {
           this.context=context;
            repoUsuario = new Repository<Usuario>(context);
            repoDocente = new Repository<Docente>(context);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = repoUsuario.Get().SingleOrDefault(u => u.NombreUsuario== loginDto.NombreUsuario && u.Contraseña == loginDto.Contraseña); // Manejar el hash de la contraseña en una implementación real
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var docente = repoDocente.Get().SingleOrDefault(d => d.UsuarioId == usuario.Id);

            // Devolver datos de usuario y docente
            return Ok(new { usuario, docente });
        }
    }

}

