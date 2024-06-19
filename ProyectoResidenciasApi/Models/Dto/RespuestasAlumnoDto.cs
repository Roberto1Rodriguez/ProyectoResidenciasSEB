namespace ProyectoResidenciasApi.Models.Dto
{
    public class RespuestaAlumnoDto
    {
        public int AlumnoId { get; set; }
        public int ExamenId { get; set; }
        public IFormFile Pdf { get; set; } // Archivo PDF con las respuestas
    }

}
