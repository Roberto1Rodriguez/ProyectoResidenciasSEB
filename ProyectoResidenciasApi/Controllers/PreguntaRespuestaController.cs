
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Repositories;


namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PreguntaRespuestaController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Pregunta> repoPregunta;
        Repository<Respuesta> _respuestaRepository;

        public PreguntaRespuestaController(Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            repoPregunta = new Repository<Pregunta>(context);
            _respuestaRepository = new Repository<Respuesta>(context);
        }

        [HttpGet("Preguntas")]
        public IActionResult GetAllPreguntas()
        {
            var preguntas = repoPregunta.Get().Where(p => p.Disponible == 1).ToList();
            return Ok(preguntas);
        }

        [HttpGet("PreguntasPorCampoFormativo")]
        public IActionResult GetPreguntasPorCampoFormativo(int campoFormativoId, string nivelEducativo)
        {
            var preguntas = repoPregunta.Get().Where(p => p.CamposFormativosId == campoFormativoId && p.NivelEducativo == nivelEducativo && p.Disponible == 1).ToList();
            return Ok(preguntas);
        }
        [HttpGet("PreguntasPorLecturaId")]
        public IActionResult GetPreguntasPorLecturaId(int lecturaId)
        {
            var preguntas = repoPregunta.Get().Where(p => p.LecturaId == lecturaId && p.Disponible == 1).ToList();
            return Ok(preguntas);
        }
        [HttpGet("PreguntasPorAsignatura")]
        public IActionResult GetPreguntasPorAsignatura(int asignaturaId, string nivelEducativo)
        {
            var preguntas = repoPregunta.Get().Where(p => p.AsignaturaId == asignaturaId && p.NivelEducativo == nivelEducativo && p.Disponible == 1).ToList();
            return Ok(preguntas);
        }

        [HttpGet("Preguntas/{id}")]
        public IActionResult GetPreguntaById(int id)
        {
            var pregunta = repoPregunta.Get(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            return Ok(pregunta);
        }

        [HttpPost("CrearPregunta")]
        public IActionResult CreatePregunta(PreguntaDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return NotFound(dto);
                }
                Pregunta pregunta = new Pregunta()
                {
                    Texto = dto.Texto,
                    CamposFormativosId = dto.CamposFormativosId,
                    NivelEducativo = dto.NivelEducativo,
                    TipoPregunta = dto.TipoPregunta,
                    LecturaId = dto.LecturaId,
                    AsignaturaId = dto.AsignaturaId,
                    Disponible = 1

                };
                repoPregunta.Insert(pregunta);
                return Ok(pregunta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("Preguntas/{id}")]
        public IActionResult UpdatePregunta(int id, PreguntaDto preguntaDto)
        {
            if (id != preguntaDto.Id)
            {
                return BadRequest();
            }

            // Convertir PreguntaDto a Pregunta
            var pregunta = new Pregunta
            {
                Id = preguntaDto.Id,
                Texto = preguntaDto.Texto,
                CamposFormativosId = preguntaDto.CamposFormativosId,
                NivelEducativo = preguntaDto.NivelEducativo,
                TipoPregunta = preguntaDto.TipoPregunta,
                AsignaturaId = preguntaDto.AsignaturaId,
                LecturaId = preguntaDto.LecturaId,
                Disponible = 1
            };

            repoPregunta.Update(pregunta);
            return Ok(pregunta);
        }
        [HttpDelete("Preguntas/{id}")]
        public IActionResult DeletePregunta(int id)
        {
            var pregunta = repoPregunta.Get(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            // Baja lógica
            pregunta.Disponible = 0;  // 0 para indicar no disponible
            repoPregunta.Update(pregunta);

            return NoContent();
        }

        [HttpGet("Respuestas")]
        public IActionResult GetAllRespuestas()
        {
            var respuestas = _respuestaRepository.Get();
            return Ok(respuestas);
        }

        [HttpGet("Respuestas/{id}")]
        public IActionResult GetRespuestaById(int id)
        {
            var respuesta = _respuestaRepository.Get(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            return Ok(respuesta);
        }

        [HttpGet("RespuestasPorPregunta/{preguntaId}")]
        public IActionResult GetRespuestasPorPreguntaId(int preguntaId)
        {
            // Obtener todas las respuestas asociadas a la pregunta con el ID especificado
            var respuestas = _respuestaRepository.Get().Where(r => r.PreguntaId == preguntaId).OrderBy(r => r.Id);
            return Ok(respuestas);
        }
        [HttpGet("PreguntasConRespuestas/{id}")]
        public IActionResult GetPreguntaConRespuestas(int id)
        {
            var pregunta = repoPregunta.Get(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            var respuestas = _respuestaRepository.Get().Where(r => r.PreguntaId == id).OrderBy(r => r.Id).ToList();
            var preguntaConRespuestas = new
            {
                Pregunta = pregunta,
                Respuestas = respuestas
            };

            return Ok(preguntaConRespuestas);
        }
        [HttpPost("CrearRespuesta")]
        public IActionResult CreateRespuesta(RespuestaDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return NotFound(dto);
                }
                Respuesta respuesta = new Respuesta()
                {
                    Texto = dto.Texto,
                    PreguntaId = dto.PreguntaId,
                    EsCorrecta = dto.Correcta,
                };
                _respuestaRepository.Insert(respuesta);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Respuestas/{id}")]
        public IActionResult UpdateRespuesta(int id, RespuestaDto respuesta)
        {
            if (id != respuesta.Id)
            {
                return BadRequest("ID de la respuesta no coincide");
            }

            try
            {
                var respuestaExistente = _respuestaRepository.Get(id);
                if (respuestaExistente == null)
                {
                    return NotFound("Respuesta no encontrada");
                }

                respuestaExistente.Texto = respuesta.Texto;
                respuestaExistente.EsCorrecta = respuesta.Correcta;

                _respuestaRepository.Update(respuestaExistente);

                return Ok(respuestaExistente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

            [HttpDelete("Respuestas/{id}")]
        public IActionResult DeleteRespuesta(int id)
        {
            var respuesta = _respuestaRepository.Get(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            _respuestaRepository.Delete(respuesta);
            return NoContent();
        }
    }
}
