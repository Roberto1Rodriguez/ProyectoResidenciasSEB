using Microsoft.AspNetCore.Mvc;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Repositories;
namespace ProyectoResidenciasApi.Controllers
{
        

    [ApiController]
    [Route("api/[controller]")]
    public class AsignaturaController : ControllerBase
    {
            private readonly Sistem21ResidenciasSebContext context;
        Repository<Asignatura> repoAsignatura;

            public AsignaturaController(Sistem21ResidenciasSebContext context)
            {
            this.context = context;
            repoAsignatura = new Repository<Asignatura>(context);
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var asignaturas = repoAsignatura.Get();
                return Ok(asignaturas);
            }
        [HttpGet("ByNivel")]
        public IActionResult GetByNivel(string nivel)
        {
            var asignaturas = repoAsignatura.Get().Where(a => a.NivelEducativo == nivel);
            return Ok(asignaturas);
        }

        [HttpGet("{id}")]
            public IActionResult GetById(int id)
            {
                var asignatura = repoAsignatura.Get(id);
                if (asignatura == null)
                {
                    return NotFound();
                }
                return Ok(asignatura);
            }

            [HttpPost]
            public IActionResult Create(Asignatura asignatura)
            {
            repoAsignatura.Insert(asignatura);
                return CreatedAtAction(nameof(GetById), new { id = asignatura.Id }, asignatura);
            }

            [HttpPut("{id}")]
            public IActionResult Update(int id, Asignatura asignatura)
            {
                if (id != asignatura.Id)
                {
                    return BadRequest();
                }

            repoAsignatura.Update(asignatura);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var asignatura = repoAsignatura.Get(id);
                if (asignatura == null)
                {
                    return NotFound();
                }

                repoAsignatura.Delete(asignatura);
                return NoContent();
            }
        }
    }

