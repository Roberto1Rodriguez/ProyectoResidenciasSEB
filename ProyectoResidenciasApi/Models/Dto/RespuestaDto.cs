namespace ProyectoResidenciasApi.Models.Dto
{
    public class RespuestaDto
    {
        public int Id { get; set; }

        public string Texto { get; set; } = null!;

        public bool Correcta { get; set; }

        public int? PreguntaId { get; set; }
    }
}
