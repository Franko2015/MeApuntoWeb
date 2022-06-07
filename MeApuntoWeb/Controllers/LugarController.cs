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
    public class LugarController : Controller
    {
        private readonly EventosDbContext _context;

        public LugarController(EventosDbContext context)
        {
            _context = context;
        }

        // GET: Lugar
        public async Task<IActionResult> Index()
        {
              return _context.tblLugar != null ? 
                          View(await _context.tblLugar.ToListAsync()) :
                          Problem("Entity set 'EventosDbContext.tblLugar'  is null.");
        }

        // GET: Lugar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tblLugar == null)
            {
                return NotFound();
            }

            var lugar = await _context.tblLugar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lugar == null)
            {
                return NotFound();
            }

            return View(lugar);
        }

        // GET: Lugar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lugar/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Direccion")] Lugar lugar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lugar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lugar);
        }

        // GET: Lugar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tblLugar == null)
            {
                return NotFound();
            }

            var lugar = await _context.tblLugar.FindAsync(id);
            if (lugar == null)
            {
                return NotFound();
            }
            return View(lugar);
        }

        // POST: Lugar/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Direccion")] Lugar lugar)
        {
            if (id != lugar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lugar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LugarExists(lugar.Id))
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
            return View(lugar);
        }

        // GET: Lugar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tblLugar == null)
            {
                return NotFound();
            }

            var lugar = await _context.tblLugar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lugar == null)
            {
                return NotFound();
            }

            return View(lugar);
        }

        // POST: Lugar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tblLugar == null)
            {
                return Problem("Entity set 'EventosDbContext.tblLugar'  is null.");
            }
            var lugar = await _context.tblLugar.FindAsync(id);
            if (lugar != null)
            {
                _context.tblLugar.Remove(lugar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LugarExists(int id)
        {
          return (_context.tblLugar?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
