namespace MeApuntoWeb.Models
    {
    public class Evento
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime Fecha_evento { get; set; }
        public DateTime Hora_inicio { get; set; }
        public DateTime Hora_termino { get; set; }
        public string? Direccion { get; set; }
        public string? Estado { get; set; }


        //Llaves foráneas
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }


        //Referencias a otras clases
        public Categoria? Categoria { get; set; }
        public Usuario? Usuario { get; set; }

        
    }
}
