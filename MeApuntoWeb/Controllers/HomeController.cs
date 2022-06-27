using MeApuntoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            var tipo = _context.tblTipo.ToList();
            Tipo_Usuario? TA = new Tipo_Usuario();
            if (tipo.Count == 0)
            {
                TA.Tipo = "Administrador";
                _context.Add(TA);
                await _context.SaveChangesAsync();
            }
            Tipo_Usuario? TS = new Tipo_Usuario();
            if (tipo.Count == 1)
            {
                TS.Tipo = "Soporte";
                _context.Add(TS);
                await _context.SaveChangesAsync();
            }
            Tipo_Usuario? TF = new Tipo_Usuario();
            if (tipo.Count == 2)
            {
                TF.Tipo = "Free";
                _context.Add(TF);
                await _context.SaveChangesAsync();
            }
            Tipo_Usuario? TP = new Tipo_Usuario();
            if (tipo.Count == 3)
            {
                TP.Tipo = "Premium";
                _context.Add(TP);
                await _context.SaveChangesAsync();
            }
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}