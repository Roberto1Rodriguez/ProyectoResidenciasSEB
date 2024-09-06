using Microsoft.AspNetCore.Mvc;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Repositories;
using System.Net.Mail;

namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocenteController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Docente> repoDocente;
        Repository<Usuario> repoUsuario;

        public DocenteController(Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            repoDocente = new Repository<Docente>(context);
            repoUsuario = new Repository<Usuario>(context);
        }

        [HttpGet("docentes")]
        public IActionResult GetDocentes()
        {
            var docentes = repoDocente.Get()
                .Where(d => repoUsuario.Get().FirstOrDefault(u => u.Id == d.UsuarioId).Tipo == "D")
                .Select(d => new
                {
                    d.Id,
                    d.Nombre,
                    Usuario = repoUsuario.Get().FirstOrDefault(u => u.Id == d.UsuarioId)
                }).ToList();

            return Ok(docentes);
        }

        [HttpPost("CrearDocente")]
        public IActionResult CrearConUsuario(DocenteDto dto)
        {
            try
            {
                if (dto == null)
                {
                    return BadRequest("Datos del alumno no proporcionados");
                }

                // Crear usuario
                Usuario usuario = new Usuario()
                {
                    NombreUsuario = dto.NombreUsuario,
                    Email = dto.Email,
                    Contraseña = dto.Contraseña, // Asegúrate de manejar el hash de la contraseña en una implementación real
                    Tipo = "D"
                };
                repoUsuario.Insert(usuario);

                // Crear alumno
                Docente docente = new Docente()
                {
                    Nombre = dto.Nombre,
                    UsuarioId = usuario.Id,
 
                };
                repoDocente.Insert(docente);

                // Enviar correo electrónico
                string mensaje = $"Hola {dto.Nombre},\n\nTu registro en la plataforma se realizó de manera exitosa.\n\nTu usuario es: {dto.NombreUsuario}\nTu contraseña es: {dto.Contraseña}\n\nSaludos,\nEl equipo de soporte.";
                EnviarCorreoElectronico(dto.Email, "Registro Exitoso en la Plataforma", mensaje);

                return Ok(docente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void EnviarCorreoElectronico(string destinatario, string asunto, string mensaje)
        {
            try
            {
                using (SmtpClient client = new SmtpClient("smtp-mail.outlook.com"))
                {
                    client.Port = 587;
                    client.Credentials = new System.Net.NetworkCredential("residenciasTecRE@outlook.com", "ResidenciasTec");
                    client.EnableSsl = true;

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("residenciasTecRE@outlook.com", "Residencias Tec RE");
                    mail.To.Add(new MailAddress(destinatario));
                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    mail.IsBodyHtml = false;

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");
            }
        }
    }
}
    
