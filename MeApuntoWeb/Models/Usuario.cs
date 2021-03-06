using System.ComponentModel.DataAnnotations;

namespace MeApuntoWeb.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Rut { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string? Edad { get; set; }
        public string? Organizacion { get; set; }
        public string? EstadoCuenta { get; set; }
        public string? NombreUsuario { get; set; }
        public int Tipo_usuarioId { get; set; }
        public Tipo_Usuario? Tipo_Usuario { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}