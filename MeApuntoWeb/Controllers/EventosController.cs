using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;

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

        // GET: Eventos/Details/5
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

        // GET: Eventos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "Id");
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "Id");
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Id");
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id");
            return View();
        }

        // POST: Eventos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "Id", evento.EstadoId);
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Id", evento.LugarId);
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id", evento.UsuarioId);
            return View(evento);
        }

        // POST: Eventos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventoId,Titulo,Descripcion,Fecha_evento,Hora_inicio,Hora_termino,EstadoId,CategoriaId,UsuarioId,LugarId")] Evento evento)
        {
            if (id != evento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(evento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventoExists(evento.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.tblCategoria, "Id", "Id", evento.CategoriaId);
            ViewData["EstadoId"] = new SelectList(_context.tblEstado, "Id", "Id", evento.EstadoId);
            ViewData["LugarId"] = new SelectList(_context.tblLugar, "Id", "Id", evento.LugarId);
            ViewData["UsuarioId"] = new SelectList(_context.tblUsuario, "Id", "Id", evento.UsuarioId);
            return View(evento);
        }

        // GET: Eventos/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Eventos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tblEvento == null)
            {
                return Problem("Entity set 'EventosDbContext.tblEvento'  is null.");
            }
            var evento = await _context.tblEvento.FindAsync(id);
            if (evento != null)
            {
                _context.tblEvento.Remove(evento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventoExists(int id)
        {
          return (_context.tblEvento?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
