using MeApuntoWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace MeApuntoWeb.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly EventosDbContext _context;
        public CategoriaController(EventosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Categorias = _context.tblCategoria.ToList();
            return View(Categorias);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Categoria c)
        {
            if(c == null) return View();

            _context.Add(c);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
