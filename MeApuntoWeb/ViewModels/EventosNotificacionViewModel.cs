using MeApuntoWeb.Models;

namespace MeApuntoWeb.ViewModels
{
    public class EventosNotificacionViewModel
    {
        //Llaves foráneas
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public int NotificacionId { get; set; }
        public int EventoId { get; set; }


        //Referencias a otras clases
        public Categoria? Categoria { get; set; }
        public Evento? Evento { get; set; }
        public Usuario? Usuario { get; set; }
        public Notificaciones Notificacion { get; set; }
    }
}
