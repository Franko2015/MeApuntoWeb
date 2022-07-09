namespace MeApuntoWeb.ViewModels
{
    public class EventoViewModel
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha_evento { get; set; }
        public DateTime Hora_inicio { get; set; }
        public DateTime Hora_termino { get; set; }
        public string Comuna { get; set; }
        public string Direccion { get; set; }
        public string Numero { get; set; }


        //Llaves foráneas
        public int EstadoId { get; set; }
        public int CategoriaId { get; set; }
        public int UsuarioId { get; set; }
        public int LugarId { get; set; }

    }
}
