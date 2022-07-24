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
        [Authorize(Roles = "1,2")]
        public IActionResult FreeAndPremium()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList().Where(e => e.Tipo_usuarioId == 3 || e.Tipo_usuarioId == 4);
            return View(users);
        }
        [Authorize(Roles = "1,2")]
        public IActionResult Administración()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList().Where(e => e.Tipo_usuarioId == 1);
            return View(users);
        }

        [Authorize(Roles = "2")]
        public IActionResult Soporte()
        {
            var users = _context.tblUsuario
                .Include(e => e.Tipo_Usuario)
                .ToList().Where(e => e.Tipo_usuarioId == 2);
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

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> BloquearUsuario(int Id)
        {
            var bloquear = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (bloquear == null) return NotFound();

            bloquear.EstadoCuenta = "BLOQUEADA";

            _context.Update(bloquear);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FreeAndPremium));

        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> BloquearAdmin(int Id)
        {

            var bloquear = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (bloquear == null) return NotFound();

            bloquear.EstadoCuenta = "BLOQUEADA";

            _context.Update(bloquear);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Administración));

        }

        [Authorize(Roles = "2")]
        public async Task<IActionResult> BloquearSoporte(int Id)
        {

            var bloquear = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (bloquear == null) return NotFound();

            bloquear.EstadoCuenta = "BLOQUEADA";

            _context.Update(bloquear);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Soporte));

        }

        [Authorize(Roles = "1, 2")]
        public async Task<IActionResult> ActivarUsuario(int Id)
        {

            var bloquear = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (bloquear == null) return NotFound();

            bloquear.EstadoCuenta = "ACTIVA";

            _context.Update(bloquear);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FreeAndPremium));

        }

        [Authorize(Roles = "2")]
        public async Task<IActionResult> ActivarAdmin(int Id)
        {

            var activar = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (activar == null) return NotFound();

            activar.EstadoCuenta = "ACTIVA";

            _context.Update(activar);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Administración));

        }

        [Authorize(Roles = "2")]
        public async Task<IActionResult> ActivarSoporte(int Id)
        {

            var activar = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);

            if (activar == null) return NotFound();

            activar.EstadoCuenta = "ACTIVA";

            _context.Update(activar);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Soporte));

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tblUsuario == null)
            {
                return NotFound();
            }

            var evento = await _context.tblUsuario.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }
            ViewData["TipoId"] = new SelectList(_context.tblUsuario, "Id", "Tipo", evento.Tipo_Usuario);
            return View(evento);
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