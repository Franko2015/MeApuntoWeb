using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;

namespace MeApuntoWeb.Controllers
{
    //[Authorize(Roles = "1,2,3,4")]
    public class UsuariosController : Controller
    {
        private readonly EventosDbContext _context;

        public UsuariosController(EventosDbContext context)
        {
            _context = context;
        }
        public IActionResult FreeAndPremium()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList();
            return View(users);
        }
        public IActionResult Administración()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList();
            return View(users);
        }
        public IActionResult Soporte()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "1,2")]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        public IActionResult TerminosYCondiciones()
        {
            return View();
        }

        [Authorize(Roles = "2")]
        public IActionResult CreateSoporte()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "1,2")]
        public async Task<IActionResult> CreateAdmin(UsuarioRegistroViewModel Uvm)
        {
            var usuario = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario || u.Rut == Uvm.Rut || (u.Nombres == Uvm.Nombres && u.Apellidos == Uvm.Apellidos));
            if (usuario == null)
            {
                Usuario? User = new Usuario();

                //Creando usuario nuevo
                User.Nombres = Uvm.Nombres.ToUpper();
                User.Rut = Uvm.Rut;
                User.Apellidos = Uvm.Apellidos.ToUpper();
                User.Correo = Uvm.Correo.ToUpper();
                User.Edad = Uvm.Edad.ToUpper();
                User.Telefono = Uvm.Telefono;
                User.NombreUsuario = Uvm.NombreUsuario;
                User.Organizacion = "ME APUNTO";
                User.EstadoCuenta = "ACTIVA";
                User.Tipo_usuarioId = 1;
                CreatePasswordHash(Uvm.Contrasena, out byte[] passwordHash, out byte[] passworSalt);
                User.PasswordHash = passwordHash;
                User.PasswordSalt = passworSalt;
                _context.Add(User);
                await _context.SaveChangesAsync();
                return RedirectToAction("Administración", "Usuarios"); ;

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> CreateSoporte(UsuarioRegistroViewModel Uvm)
        {
            var usuario = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario || u.Rut == Uvm.Rut || (u.Nombres == Uvm.Nombres && u.Apellidos == Uvm.Apellidos));
            if (usuario == null)
            {
                Usuario? User = new Usuario();

                //Creando usuario nuevo
                User.Nombres = Uvm.Nombres.ToUpper();
                User.Rut = Uvm.Rut;
                User.Apellidos = Uvm.Apellidos.ToUpper();
                User.Correo = Uvm.Correo.ToUpper();
                User.Edad = Uvm.Edad.ToUpper();
                User.Telefono = Uvm.Telefono;
                User.NombreUsuario = Uvm.NombreUsuario;
                User.Organizacion = "ME APUNTO";
                User.EstadoCuenta = "ACTIVA";
                User.Tipo_usuarioId = 2;
                CreatePasswordHash(Uvm.Contrasena, out byte[] passwordHash, out byte[] passworSalt);
                User.PasswordHash = passwordHash;
                User.PasswordSalt = passworSalt;
                _context.Add(User);
                await _context.SaveChangesAsync();
                return RedirectToAction("Soporte", "Usuarios"); ;

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        //[Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> Create(UsuarioRegistroViewModel Uvm)
        {
            var usuario = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario || u.Rut == Uvm.Rut || (u.Nombres == Uvm.Nombres && u.Apellidos == Uvm.Apellidos));
            if (usuario == null) { 
            Usuario? User = new Usuario();

                //Creando usuario nuevo
                User.Nombres = Uvm.Nombres.ToUpper();
                User.Rut = Uvm.Rut;
                User.Apellidos = Uvm.Apellidos.ToUpper();
                User.Correo = Uvm.Correo.ToUpper();
                User.Edad = Uvm.Edad;
                User.Telefono = "9 "+Uvm.Telefono;
                User.NombreUsuario = Uvm.NombreUsuario;
                User.Organizacion = Uvm.Organizacion;
                User.EstadoCuenta = "ACTIVA";
                User.Tipo_usuarioId = 3;
                CreatePasswordHash(Uvm.Contrasena, out byte[] passwordHash, out byte[] passworSalt);
                User.PasswordHash = passwordHash;
                User.PasswordSalt = passworSalt;
                _context.Add(User);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); ;

            }
            else
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var u = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);
            if (u == null) return NotFound();
            _context.Remove(u);
            await _context.SaveChangesAsync();
            return RedirectToAction("Admin","Home");
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}