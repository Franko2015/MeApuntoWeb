using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
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
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Estado).Include(e => e.Lugar).Include(e => e.Usuario);
            return View(await eventosDbContext.ToListAsync());
        }

        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tblEvento == null)
            {
                return NotFound();
            }

            var evento = await _context.tblEvento
                .Include(e => e.Categoria)
                .Include(e => e.Estado)
                .Include(e => e.Lugar)
                .Include(e => e.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }

        [Authorize(Roles = "1,2,3,4")]
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "categoria");
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "estado");
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Direccion");
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Nombres", "Apellidos");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> Create([Bind("EventoId,Titulo,Descripcion,Fecha_evento,Hora_inicio,Hora_termino,EstadoId,CategoriaId,UsuarioId,LugarId")] Evento evento)
        {
            if (ModelState.IsValid)
            {

                _context.Add(evento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "Id", evento.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "Id", evento.EstadoId);
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Id", evento.LugarId);
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id", evento.UsuarioId);
            return View(evento);
        }

        [Authorize(Roles = "1,2,3,4")]
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
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "Id", evento.EstadoId);
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Id", evento.LugarId);
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id", evento.UsuarioId);
            return View(evento);
        }
        
    }
}
