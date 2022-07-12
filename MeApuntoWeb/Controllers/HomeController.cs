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
            if (tipo.Count<1)
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




            //Carga de eventos
            var eventosDbContext = _context.tblEvento.Include(e => e.Categoria).Include(e => e.Usuario);

            return View(await eventosDbContext.ToListAsync());
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

    }
}