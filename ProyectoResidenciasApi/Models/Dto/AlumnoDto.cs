namespace ProyectoResidenciasApi.Models.Dto
{
    public class AlumnoDto
    {
        public string? Nombre { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Email { get; set; }
        public string? Contraseña { get; set; }
        public string? Grado { get; set; }
        public string? Seccion { get; set; }
        public int DocenteId { get; set; }
    }
}
