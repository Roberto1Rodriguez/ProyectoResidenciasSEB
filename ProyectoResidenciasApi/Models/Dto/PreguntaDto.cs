namespace ProyectoResidenciasApi.Models.Dto
{
    public class PreguntaDto
    {
         public int Id { get; set; }

    public string Texto { get; set; } = null!;

        public string TipoPregunta { get; set; } = null!;

        public string? NivelEducativo { get; set; }
        
        public int? AsignaturaId { get; set; }

    public int? LecturaId { get; set; }

        public int? CamposFormativosId {  get; set; }
        public string? CreadoPor { get; set; }

        public string? ModificadoPor { get; set; }

    }
}
