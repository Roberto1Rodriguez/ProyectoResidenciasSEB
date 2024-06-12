using Microsoft.AspNetCore.Mvc;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Repositories;

namespace ProyectoResidenciasApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CampoFormativoController : ControllerBase
    {


       private readonly Sistem21ResidenciasSebContext context;
            Repository<Camposformativos> repoCampo;

            public CampoFormativoController(Sistem21ResidenciasSebContext context)
            {
                this.context = context;
                repoCampo = new Repository<Camposformativos>(context);
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var asignaturas = repoCampo.Get();
                return Ok(asignaturas);
            }
        

            [HttpGet("{id}")]
            public IActionResult GetById(int id)
            {
                var asignatura = repoCampo.Get(id);
                if (asignatura == null)
                {
                    return NotFound();
                }
                return Ok(asignatura);
            }

            [HttpPost]
            public IActionResult Create(Camposformativos campo)
            {
            repoCampo.Insert(campo);
                return CreatedAtAction(nameof(GetById), new { id = campo.Id }, campo);
            }

            [HttpPut("{id}")]
            public IActionResult Update(int id, Camposformativos campo)
            {
                if (id != campo.Id)
                {
                    return BadRequest();
                }

            repoCampo.Update(campo);
                return NoContent();
            }

            [HttpDelete("{id}")]
            public IActionResult Delete(int id)
            {
                var asignatura = repoCampo.Get(id);
                if (asignatura == null)
                {
                    return NotFound();
                }

            repoCampo.Delete(asignatura);
                return NoContent();
            }
        }
    }



