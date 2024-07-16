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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Usuario> usuarios = new();
            usuarios = await repoUsuario.Get().Include(u => u.Alumno).ToListAsync();
            if (usuarios == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(usuarios);
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            Usuario? usuario = new();
            usuario = await repoUsuario.Get().Where(x => x.Id == id).Include(u => u.Alumno).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(usuario);
            }

        }

    }

}

