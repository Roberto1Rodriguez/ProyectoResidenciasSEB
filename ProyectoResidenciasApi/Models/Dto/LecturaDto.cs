namespace ProyectoResidenciasApi.Models.Dto
{
    public class LecturaDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = null!;

        public string Contenido { get; set; } = null!;

        public string? NivelEducativo { get; set; }

        public int? AsignaturaId { get; set; }

        public int? CamposFormativosId { get; set; }
    }
}
