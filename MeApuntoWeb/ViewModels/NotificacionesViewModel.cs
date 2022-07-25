using MeApuntoWeb.Models;

namespace MeApuntoWeb.ViewModels
{
    public class NotificacionesViewModel
    {
        public int Id { get; set; }
        public string Notificacion { get; set; }
        public int UsuarioReceptor { get; set; }
        public int UsuarioRemitente { get; set; }

        public Usuario Usuario { get; set; }
    }
}
