namespace MeApuntoWeb.Models
{
    public class AsistenciaEventos
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int EventoId { get; set; }
        public Evento Evento { get; set; }

    }
}
