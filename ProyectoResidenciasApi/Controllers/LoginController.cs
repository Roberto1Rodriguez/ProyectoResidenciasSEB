using Microsoft.AspNetCore.Mvc;
using ProyectoResidenciasApi.Data;
using ProyectoResidenciasApi.Models.Dto;
using ProyectoResidenciasApi.Models;
using ProyectoResidenciasApi.Repositories;
using System.Net.Mail;

namespace ProyectoResidenciasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Sistem21ResidenciasSebContext context;
        Repository<Usuario> repoUsuario;
        Repository<Docente> repoDocente;
        Repository<Alumno> repoAlumno;
        public LoginController(Sistem21ResidenciasSebContext context)
        {
            this.context = context;
            repoUsuario = new Repository<Usuario>(context);
            repoDocente = new Repository<Docente>(context);
            repoAlumno= new Repository<Alumno>(context);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var usuario = repoUsuario.Get().SingleOrDefault(u => u.NombreUsuario == loginDto.NombreUsuario && u.Contraseña == loginDto.Contraseña); // Manejar el hash de la contraseña en una implementación real
            if (usuario == null)
            {
                return Unauthorized("Usuario o contraseña incorrectos");
            }

            var docente = repoDocente.Get().SingleOrDefault(d => d.UsuarioId == usuario.Id);
            var alumno = repoAlumno.Get().SingleOrDefault(d => d.UsuarioId == usuario.Id);
            // Devolver datos de usuario y docente
            return Ok(new { usuario, docente,alumno });
        }
        [HttpPut("restablecer")]
        public IActionResult Restablecer(JsonRequestModel model)
        {
            try
            {
                var usuario = repoUsuario.Get().SingleOrDefault(u => u.Email == model.Email);

                if (model == null || string.IsNullOrEmpty(model.Email))
                {
                    return BadRequest("Email no proporcionado en el cuerpo de la solicitud.");
                }
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }
                string nuevaContraseña = GenerarNuevaContraseña();
                usuario.Contraseña = nuevaContraseña;

                repoUsuario.Update(usuario);


                // Enviar correo electrónico con la nueva contraseña
                EnviarCorreoElectronico(usuario.Email, "Restablecimiento de Contraseña", $"Tu nueva contraseña es: {nuevaContraseña}");

                return Ok("Contraseña restablecida con éxito. Se ha enviado un correo electrónico con la nueva contraseña.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public class JsonRequestModel
        {
            public string? Email { get; set; }
        }
        private string GenerarNuevaContraseña()
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            return new string(Enumerable.Repeat(caracteres, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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

                    MailMessage mail = new MailMessage("residenciasTecRE@outlook.com", destinatario)
                    {
                        Subject = asunto,
                        Body = mensaje,
                        IsBodyHtml = false
                    };

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo electrónico: {ex.Message}");
            }
        }
        [HttpPut("cambiarcontrasena")]
        public IActionResult CambiarContraseña([FromBody] CambiarContraseñaModel model)
        {
            try
            {
                if (model == null || model.UserId <= 0 || string.IsNullOrEmpty(model.ContrasenaActual) || string.IsNullOrEmpty(model.ContrasenaNueva))
                {
                    return BadRequest("Se requiere el ID del usuario, la contraseña actual y la nueva contraseña.");
                }

                var usuario = repoUsuario.Get().SingleOrDefault(u => u.Id == model.UserId);

                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                if (usuario.Contraseña != model.ContrasenaActual)
                {
                    return BadRequest("La contraseña actual no es correcta.");
                }

                usuario.Contraseña = model.ContrasenaNueva;

                repoUsuario.Update(usuario);

                return Ok("Contraseña cambiada con éxito.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class CambiarContraseñaModel
        {
            public int UserId { get; set; }
            public string? ContrasenaActual { get; set; }
            public string? ContrasenaNueva { get; set; }
        }
    }

}
