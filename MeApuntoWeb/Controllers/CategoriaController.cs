   using MeApuntoWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoWeb.Controllers
{
    //[Authorize(Roles = "Soporte, Administrador")]
    public class CategoriaController : Controller
    {
        private readonly EventosDbContext _context;
        public CategoriaController(EventosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Categorias = _context.tblCategoria?.ToList();
            return View(Categorias);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Categoria c)
        {
            if(c == null) return View();

            _context.Add(c);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public IActionResult Edit(int idCategoria)
        {
            var Categoria = _context.tblCategoria?.FirstOrDefault(c => c.Id == idCategoria);
            if(Categoria == null) return NotFound();
            else
            {
                return View(Categoria);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Categoria c)
        {
            if (ModelState.IsValid)
            {
                _context.Update(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(c);
        }


        [HttpGet]
        public IActionResult Delete(int idCategoria)
        {
            var Categoria = _context.tblCategoria?.FirstOrDefault(c => c.Id == idCategoria);
            if (Categoria == null) return NotFound();
            else
            {
                return View(Categoria);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Categoria c)
        {
                _context.Remove(c);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
    }
}
