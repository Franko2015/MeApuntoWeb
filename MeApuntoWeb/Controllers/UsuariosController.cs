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
        public IActionResult Index()
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

        [HttpPost]
        //[Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> Create(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var U = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario);
                if (U == null)
                {
                    Usuario? UA = new Usuario();
                    
                        //Creando usuario NUEVO
                        UA.Nombres = Uvm.Nombres;
                        UA.Apellidos = Uvm.Apellidos;
                        UA.Rut = Uvm.Rut;
                        UA.Correo = Uvm.Correo;
                        UA.Edad = Uvm.Edad;
                        UA.Telefono = Uvm.Telefono;
                        UA.NombreUsuario = Uvm.NombreUsuario;
                        UA.Organizacion = Uvm.Organizacion;
                        UA.EstadoCuenta = "ACTIVA";
                        UA.Tipo_usuarioId = 3;
                        CreatePasswordHash(Uvm.Contrasena, out byte[] passwordHash, out byte[] passworSalt);
                        UA.PasswordHash = passwordHash;
                        UA.PasswordSalt = passworSalt;
                        _context.Add(UA);
                        await _context.SaveChangesAsync();
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Nombre de usuario ya existe!");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);
            if (U == null) return NotFound();
            _context.Remove(U);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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