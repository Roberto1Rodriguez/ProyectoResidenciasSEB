namespace ProyectoResidenciasApi.Models.Dto
{
    public class ExamenDto
    {
        public IFormFile Pdf { get; set; }
        public int DocenteId { get; set; }
        public string NombreEscuela { get; set; }
        public string Turno { get; set; }
        public string Grado { get; set; }
        public string Seccion { get; set; }
        public string? NivelEducativo { get; set; }

        public string? CampoFormativo { get; set; }

        public string? Asignatura { get; set; }
        public DateOnly? Fecha { get; set; }
        public List<int> PreguntaIds { get; set; }
    }
}
