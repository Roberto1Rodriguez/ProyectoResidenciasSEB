using Microsoft.AspNetCore.Mvc;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Repositories;

namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LecturaController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Lectura> repoLectura;
        public LecturaController(Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            repoLectura = new Repository<Lectura>(context);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var lecturas = repoLectura.Get();
            return Ok(lecturas);
        }
        [HttpGet("LecturasPorCampoFormativo")]
        public IActionResult GetLecturasPorCampoFormativo(int campoFormativoId, string nivelEducativo)
        {
            var lecturas = repoLectura.Get().Where(p => p.CamposFormativosId == campoFormativoId && p.NivelEducativo == nivelEducativo).ToList();
            return Ok(lecturas);
        }

        [HttpGet("LecturasPorAsignatura")]
        public IActionResult GetLecturassPorAsignatura(int asignaturaId, string nivelEducativo)
        {
            var lecturas = repoLectura.Get().Where(p => p.AsignaturaId == asignaturaId && p.NivelEducativo == nivelEducativo).ToList();
            return Ok(lecturas);
        }
        [HttpGet("LecturasByAll")]
        public IActionResult GetLecturassByAll(int campoFormativoId,int asignaturaId, string nivelEducativo)
        {
            var lecturas = repoLectura.Get().Where(p => p.CamposFormativosId==campoFormativoId && p.AsignaturaId == asignaturaId && p.NivelEducativo == nivelEducativo).ToList();
            return Ok(lecturas);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var lectura = repoLectura.Get(id);
            if (lectura == null)
            {
                return NotFound();
            }
            return Ok(lectura);
        }
        [HttpPost("CrearLectura")]
        public IActionResult CreateLectura(LecturaDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return NotFound(dto);
                }
                Lectura lectura = new Lectura()
                {

                    Titulo = dto.Titulo,
                    Contenido = dto.Contenido,
                    CamposFormativosId = dto.CamposFormativosId,
                    NivelEducativo = dto.NivelEducativo,            
                    AsignaturaId = dto.AsignaturaId,

                };
                repoLectura.Insert(lectura);
                return Ok(lectura);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, Lectura lectura)
        {
            if (id != lectura.Id)
            {
                return BadRequest();
            }

            repoLectura.Update(lectura);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var lectura = repoLectura.Get(id);
            if (lectura == null)
            {
                return NotFound();
            }

            repoLectura.Delete(lectura);
            return NoContent();
        }
    }
}
    
