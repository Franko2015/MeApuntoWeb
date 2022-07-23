using MeApuntoWeb.Models;

namespace MeApuntoWeb.ViewModels
{
    public class UsuarioRegistroViewModel
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Rut { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Edad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Organizacion { get; set; }
        public string EstadoCuenta { get; set; }
        public string NombreUsuario { get; set; }
        public int Tipo_usuarioId { get; set; }
        public string Contrasena { get; set; }
    }
}
