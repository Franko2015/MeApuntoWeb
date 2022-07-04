using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;

namespace MeApuntoWeb.Controllers
{
    public class LugarController : Controller
    {
        private readonly EventosDbContext _context;

        public LugarController(EventosDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
              return _context.tblLugar != null ? 
                          View(await _context.tblLugar.ToListAsync()) :
                          Problem("Entity set 'EventosDbContext.tblLugar'  is null.");
        }
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LugarViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                Lugar lugar = new Lugar();
                lugar.Direccion = (lvm.Comuna + " - " + lvm.lugar.ToString() + " #"+ lvm.numero).ToUpper();
                _context.Add(lugar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lvm);
        }

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

        //Falta el EDIT

        public async Task<IActionResult> Delete(int LugarId)
        {
            var L = _context.tblLugar.FirstOrDefault(u => u.Id == LugarId);
            if (L == null) return NotFound();
            _context.Remove(L);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
