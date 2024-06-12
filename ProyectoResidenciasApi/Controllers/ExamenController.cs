using Microsoft.AspNetCore.Mvc;
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

        private readonly IWebHostEnvironment _env;
        private readonly Repository<Examengenerado> repoExamen;
        private readonly Repository<Historialexamen> repoHistorialExamen;
        private readonly Repository<Alumno> repoAlumno;
        public ExamenController(IWebHostEnvironment env, Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            _env = env;
            repoExamen = new Repository<Examengenerado>(context);
            repoHistorialExamen = new Repository<Historialexamen>(context);
            repoAlumno = new Repository<Alumno>(context);

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
                Calificacion = h.Calificacion
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
    }
}

