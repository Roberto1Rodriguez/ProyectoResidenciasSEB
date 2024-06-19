using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Repositories;

namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamenController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;

        IWebHostEnvironment _env;
        Repository<Examengenerado> repoExamen;
        Repository<Historialexamen> repoHistorialExamen;
        private readonly Repository<Alumno> repoAlumno;
        Repository<Examenpregunta> repoExamenPregunta;
        Repository<Respuesta> repoRespuesta;
        Repository<Pregunta> repoPregunta;
        Repository<Lectura> repoLectura;
        public ExamenController(IWebHostEnvironment env, Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            _env = env;
            repoExamen = new Repository<Examengenerado>(context);
            repoHistorialExamen = new Repository<Historialexamen>(context);
            repoAlumno = new Repository<Alumno>(context);
            repoExamenPregunta = new Repository<Examenpregunta>(context);
            repoPregunta = new Repository<Pregunta>(context);
            repoRespuesta = new Repository<Respuesta>(context);
        }
        [HttpGet("examenesPorDocente/{docenteId}")]
        public IActionResult GetExamenesPorDocente(int docenteId)
        {
            var examenes = repoExamen.Get().Where(e => e.DocenteId == docenteId).ToList();
            return Ok(examenes);
        }
        [HttpGet("examen/{examenId}")]
        public IActionResult GetExamen(int examenId)
        {
            var examen = repoExamen.Get(examenId);
            if (examen == null)
            {
                return NotFound();
            }

            var historial = repoHistorialExamen.Get().Where(h => h.ExamenGeneradoId == examenId).ToList();
            var alumnos = historial.Select(h => new
            {
                Alumno = repoAlumno.Get(h.AlumnoId),
                Calificacion = h.Calificacion,
                UbicacionRespuestasPDF = h.UbicacionRespuestasPdf
            });

            return Ok(new { Examen = examen, Alumnos = alumnos });
        }

        [HttpPost("calificar")]
        public IActionResult Calificar([FromBody] CalificacionDto calificacionDto)
        {
            var historial = repoHistorialExamen.Get().FirstOrDefault(h => h.ExamenGeneradoId == calificacionDto.ExamenId && h.AlumnoId == calificacionDto.AlumnoId);
            if (historial == null)
            {
                return NotFound();
            }

            historial.Calificacion = calificacionDto.Calificacion;
            repoHistorialExamen.Update(historial);

            return Ok();
        }


        [HttpPost("GenerarExamen")]
        public async Task<IActionResult> GenerarExamen([FromForm] ExamenDto dto)
        {
            try
            {
                var file = dto.Pdf;
                if (file == null || file.Length == 0)
                    return BadRequest("No se ha subido un archivo válido.");

                var filePath = Path.Combine(_env.WebRootPath, "examenes", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var examenGenerado = new Examengenerado
                {
                    DocenteId = dto.DocenteId,
                    UbicacionPdf = filePath,
                    CampoFormativo = dto.CampoFormativo,
                    Asignatura = dto.Asignatura,
                    NivelEducativo = dto.NivelEducativo,
                    Fecha = dto.Fecha
                };

                repoExamen.Insert(examenGenerado);
                foreach (var preguntaId in dto.PreguntaIds)
                {
                    var examenPregunta = new Examenpregunta
                    {
                        ExamenGeneradoId = examenGenerado.Id,
                        PreguntaId = preguntaId
                    };
                    repoExamenPregunta.Insert(examenPregunta);
                }
                // Obtener alumnos por grado y sección
                var alumnos = repoAlumno.Get().Where(a => a.Grado == dto.Grado && a.Seccion == dto.Seccion).ToList();

                foreach (var alumno in alumnos)
                {
                    var historialExamen = new Historialexamen
                    {
                        ExamenGeneradoId = examenGenerado.Id,
                        AlumnoId = alumno.Id,
                        Calificacion = null // Inicialmente sin calificación
                    };
                    repoHistorialExamen.Insert(historialExamen);
                }

                return Ok(examenGenerado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ExamenPreguntas/{examenId}")]
        public IActionResult GetPreguntasPorExamen(int examenId)
        {
            var examenPreguntas = repoExamenPregunta.Get().Where(ep => ep.ExamenGeneradoId == examenId).ToList();
            var preguntas = examenPreguntas.Select(ep => repoPregunta.Get(ep.PreguntaId)).ToList();
            var preguntaConRespuestas = preguntas.Select(p => new
            {
                p.Id,
                p.Texto,
                p.TipoPregunta,
                Respuestas = repoRespuesta.Get().Where(r => r.PreguntaId == p.Id).ToList(),
                LecturaTexto = p.LecturaId.HasValue ? repoLectura.Get(p.LecturaId.Value).Contenido : null,
                SubPreguntas = p.TipoPregunta == "Lectura Multirreactivos" ? repoPregunta.Get().Where(sp => sp.LecturaId == p.LecturaId).ToList() : null
            });

            return Ok(new { preguntas = preguntaConRespuestas });
        }
        [HttpPost("EnviarRespuestas")]
        public async Task<IActionResult> EnviarRespuestas([FromForm] RespuestaAlumnoDto dto)
        {
            try
            {
                var file = dto.Pdf;
                if (file == null || file.Length == 0)
                    return BadRequest("No se ha subido un archivo válido.");

                var filePath = Path.Combine(_env.WebRootPath, "respuestas", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var historialExamen = repoHistorialExamen.Get()
                    .FirstOrDefault(h => h.ExamenGeneradoId == dto.ExamenId && h.AlumnoId == dto.AlumnoId);

                if (historialExamen == null)
                {
                    return NotFound("No se encontró el historial del examen para el alumno especificado.");
                }

                historialExamen.UbicacionRespuestasPdf = filePath;
                repoHistorialExamen.Update(historialExamen);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DescargarRespuestas")]
        public IActionResult DescargarRespuestas(int examenId, int alumnoId)
        {
            var historialExamen = repoHistorialExamen.Get()
                .FirstOrDefault(h => h.ExamenGeneradoId == examenId && h.AlumnoId == alumnoId);

            if (historialExamen == null || string.IsNullOrEmpty(historialExamen.UbicacionRespuestasPdf))
            {
                return NotFound("No se encontraron respuestas para este alumno.");
            }

            var filePath = historialExamen.UbicacionRespuestasPdf;
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", Path.GetFileName(filePath));
        }
    }
}

