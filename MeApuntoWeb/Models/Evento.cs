namespace MeApuntoWeb.Models
{
    public class Evento
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha_evento { get; set; }
        public DateTime Hora_inicio { get; set; }
        public DateTime Hora_termino { get; set; }


        //Llaves foráneas
        public int EstadoId { get; set; }
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public int LugarId { get; set; }


        //Referencias a otras clases
        public Estado? Estado { get; set; }
        public Categoria? Categoria { get; set; }
        public Usuario? Usuario { get; set; }
        public Lugar? Lugar { get; set; }

        
    }
}
