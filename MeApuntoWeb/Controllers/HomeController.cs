using MeApuntoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MeApuntoWeb.Controllers
{

    public class HomeController : Controller
    {

        private readonly EventosDbContext _context;
        public HomeController(EventosDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //Carga los tipos de usuario

            //Administrador
            var tipo = _context.tblTipo.ToList();
            Tipo_Usuario? TA = new Tipo_Usuario();
            if (tipo.Count < 1)
            {
                TA.Tipo = "Administrador";
                _context.Add(TA);
                await _context.SaveChangesAsync();
            }

            //Soporte
            Tipo_Usuario? TS = new Tipo_Usuario();
            if (tipo.Count < 2)
            {
                TS.Tipo = "Soporte";
                _context.Add(TS);
                await _context.SaveChangesAsync();
            }

            //Free
            Tipo_Usuario? TF = new Tipo_Usuario();
            if (tipo.Count < 3)
            {
                TF.Tipo = "Free";
                _context.Add(TF);
                await _context.SaveChangesAsync();
            }

            //Premium
            Tipo_Usuario? TP = new Tipo_Usuario();
            if (tipo.Count < 4)
            {
                TP.Tipo = "Premium";
                _context.Add(TP);
                await _context.SaveChangesAsync();
            }

            var usuario = _context.tblUsuario.ToList();
            Usuario? US = new Usuario();
            if (usuario.Count == 0)
            {
                //Creando usuario Soporte
                US.Nombres = "soporte";
                US.Apellidos = "soporte";
                US.Rut = "";
                US.Correo = "soporte@gmail.com";
                US.Edad = "20";
                US.Telefono = "";
                US.NombreUsuario = "soporte";
                US.Organizacion = "Me Apunto";
                US.EstadoCuenta = "ACTIVA";
                US.Tipo_usuarioId = 2;
                CreatePasswordHash("soporte", out byte[] passwordHash, out byte[] passworSalt);
                US.PasswordHash = passwordHash;
                US.PasswordSalt = passworSalt;
                _context.Add(US);
                await _context.SaveChangesAsync();
            }


            //Carga de eventos
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario)
                .Where(evento => evento.Estado == "Aceptado").Where(evento => evento.Fecha_evento
                .CompareTo(DateTime.Now.AddDays(-1)) > 0)
                .OrderBy(evento => evento.Fecha_evento);

            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            var idsEventosApuntados = new List<int>();
            if (user != null)
            {
                var queryApuntados = _context.tblAsistenciaEventos.Where(asist => asist.UsuarioId == user.Id);
                idsEventosApuntados = queryApuntados.Select(asist => asist.EventoId).ToList();
            }
            ViewData["eventosApuntados"] = idsEventosApuntados;

            return View(await eventosDbContext.ToListAsync());
        }

        public IActionResult MeApunto(int id)
        {
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            if (_context.tblAsistenciaEventos.Any(asist => asist.EventoId == id && asist.UsuarioId == user.Id)) 
                return RedirectToAction(nameof(Index));

            AsistenciaEventos ae = new AsistenciaEventos();
            ae.EventoId = id;
            ae.UsuarioId = user.Id;
            _context.Add(ae);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Desapunto(int id)
        {
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            if (!_context.tblAsistenciaEventos.Any(asist => asist.EventoId == id && asist.UsuarioId == user.Id))
                return RedirectToAction(nameof(Index));

            var asistencia = _context.tblAsistenciaEventos.FirstOrDefault(asist => asist.EventoId == id && asist.UsuarioId == user.Id);

            _context.Remove(asistencia);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrderCategoria()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario).Where(evento => evento.Estado == "Aceptado").Where(evento => evento.Fecha_evento.CompareTo(DateTime.Now.AddDays(-1)) > 0).OrderBy(evento => evento.Categoria.categoria);

            await eventosDbContext.ToListAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> OrderLugar()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario).Where(evento => evento.Estado == "Aceptado").Where(evento => evento.Fecha_evento.CompareTo(DateTime.Now.AddDays(-1)) > 0).OrderBy(evento => evento.Direccion);

            await eventosDbContext.ToListAsync();

            return RedirectToAction(nameof(Index));
        }



        [Authorize(Roles = "1, 2")]
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}