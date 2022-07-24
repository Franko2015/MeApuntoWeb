using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MeApuntoWeb.Controllers
{
    public class EventosController : Controller
    {
        private readonly EventosDbContext _context;

        public EventosController(EventosDbContext context)
        {
            _context = context;
        }

        // GET: Eventos
        public async Task<IActionResult> Index()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario);
            return View(await eventosDbContext.ToListAsync());
        }
        // GET: Eventos
        public async Task<IActionResult> Pendientes()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario).Where(evento => evento.Estado == "Pendiente");
            return View(await eventosDbContext.ToListAsync());
        }
        // GET: Eventos
        public async Task<IActionResult> Aceptados()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario).Where(evento => evento.Estado == "Aceptado");
            return View(await eventosDbContext.ToListAsync());
        }

        public async Task<IActionResult> MisEventos()
        {
            var eventosDbContext = _context.tblEvento.Where(u => u.Usuario.NombreUsuario == User.Identity.Name).Include(e => e.Categoria).Include(e => e.Usuario).OrderByDescending(a => a.Estado);
            return View(await eventosDbContext.ToListAsync());

        }



        // GET: Eventos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tblEvento == null)
            {
                return NotFound();
            }

            var evento = await _context.tblEvento
                .Include(e => e.Categoria)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "categoria");
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Nombres", "Apellidos");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel evento)
        {
            if (ModelState.IsValid)
            {
                var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

                Evento e = new Evento();

                e.Titulo = evento.Titulo;
                e.Estado = "Pendiente";
                e.Descripcion = evento.Descripcion;
                e.CategoriaId = evento.CategoriaId;
                e.FechaCreacion = DateTime.Now;
                e.Fecha_evento = Convert.ToDateTime(evento.Fecha_evento.Date.ToString("dd/MM/yyyy"));
                e.Hora_inicio = Convert.ToDateTime(evento.Hora_inicio.ToString("HH:mm"));
                e.Hora_termino = Convert.ToDateTime(evento.Hora_termino.ToString("HH:mm"));
                e.Direccion = evento.Comuna + " - " + evento.Direccion + " #" + evento.Numero;
                e.UsuarioId = user.Id;

                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "categoria");
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Nombres", "Apellidos");
            return View(RedirectToAction(nameof(MisEventos)));
        }


        [Authorize(Roles = "3, 4")]
        // GET: Eventos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tblEvento == null)
            {
                return NotFound();
            }

            var evento = await _context.tblEvento.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "Id", evento.CategoriaId);
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id", evento.UsuarioId);
            return View(evento);
        }



        [Authorize(Roles = "1,2")]
        public IActionResult NotificarBloqueado(int Id)
        {
            var notificar = _context.tblNotificaciones.FirstOrDefault(u => u.UsuarioReceptor == Id);
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            if (notificar == null) return NotFound();

            notificar.UsuarioReceptor = Id;
            notificar.Notificacion = "SU EVENTO HA SIDO ACEPTADO.";
            notificar.UsuarioRemitente = user.Id;

            _context.Update(notificar);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "1,2")]
        public IActionResult NotificarAceptado(int Id)
        {
            var notificar = _context.tblNotificaciones.FirstOrDefault(u => u.UsuarioReceptor == Id);
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            if (notificar == null) return NotFound();

            notificar.UsuarioReceptor = Id;
            notificar.Notificacion = "SU EVENTO HA SIDO BLOQUEADO. CONSULTE A LA ADMINISTRACIÓN PARA MÁS INFORMACIÓN";
            notificar.UsuarioRemitente = user.Id;

            _context.Update(notificar);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Aceptar(int Id)
        {
            var bloquear = _context.tblEvento.FirstOrDefault(u => u.Id == Id);
            if (bloquear == null) return NotFound();
            bloquear.Estado = "Aceptado";
            _context.Update(bloquear);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> Bloquear(int Id)
        {
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            var bloquear = _context.tblEvento.FirstOrDefault(u => u.Id == Id);
            if (bloquear == null) return NotFound();

            bloquear.Estado = "Bloqueado";


            if (bloquear.Estado.Equals("Bloqueado").ToString().Count() > 5)
            {
                user.EstadoCuenta = "BLOQUEADA";

                _context.Update(user);

                await _context.SaveChangesAsync();

                return RedirectToAction("Admin","Home");
            }
            else
            {
                _context.Update(bloquear);

                await _context.SaveChangesAsync();
                return RedirectToAction("Admin","Home");
            }
        }

    }
}
