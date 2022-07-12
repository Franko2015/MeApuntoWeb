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
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario);
            return View(await eventosDbContext.ToListAsync());
        }
        // GET: Eventos
        public async Task<IActionResult> Aceptados()
        {
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario);
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

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventoViewModel evento)
        {
            if (ModelState.IsValid)
            {
                Evento e = new Evento();

                e.Titulo = evento.Titulo;
                e.Estado = "Pendiente";
                e.Descripcion = evento.Descripcion;
                e.CategoriaId = evento.CategoriaId;
                e.Fecha_evento = Convert.ToDateTime(evento.Fecha_evento.Date.ToString("dd/MM/yyyy"));
                e.Hora_inicio = Convert.ToDateTime(evento.Hora_inicio.ToString("H:mm"));
                e.Hora_termino = Convert.ToDateTime(evento.Hora_termino.ToString("H:mm"));
                e.Direccion = evento.Comuna +" - "+ evento.Direccion +" #"+ evento.Numero;
                e.UsuarioId = evento.UsuarioId;

                _context.Add(e);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Pendientes));
            }
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "categoria");
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Nombres", "Apellidos");
            return View(evento);
        }

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

        public IActionResult Aceptar(int Id)
        {
            var L = _context.tblEvento.FirstOrDefault(e => e.Id == Id);
            if (L == null) return RedirectToAction(nameof(Index));

            return View(L);
        }

        [HttpPost]
        public async Task<IActionResult> Aceptar(Evento evento)
        {
            if (evento == null) return RedirectToAction(nameof(Index));

            evento.Estado = "Aceptado";
            
            _context.Update(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        //Bloquear evento
        public IActionResult Bloquear(int Id)
        {
            var L = _context.tblEvento.FirstOrDefault(e => e.Id == Id);
            if (L == null) return RedirectToAction(nameof(Index));

            return View(L);
        }

        [HttpPost]
        public async Task<IActionResult> Bloquear(Evento evento)
        {
            if (evento == null) return RedirectToAction(nameof(Index));

            evento.Estado = "Bloqueado";

            _context.Update(evento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool EventoExists(int id)
        {
          return (_context.tblEvento?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
