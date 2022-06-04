namespace MeApuntoWeb.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Rut { get; set; }
        public string? Correo { get; set; }
        public string? Contraseña { get; set; }
        public string? Edad { get; set; }
        public string? Organizacion { get; set; }
        public string? EstadoCuenta { get; set; }
        public int Tipo_usuarioId { get; set; }
        public TipoUsuario? Tipo_usuario { get; set; }
    }
}