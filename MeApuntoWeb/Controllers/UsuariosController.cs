using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                User.FechaNacimiento = Convert.ToDateTime(Uvm.FechaNacimiento.Date.ToString("dd/MM/yyyy"));
                var edad = (Convert.ToInt16(Uvm.FechaNacimiento.Date.ToString("yyyy")) - DateTime.Now.Year) * -1;
                User.Edad = edad.ToString();
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

                var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, Uvm.Id.ToString()),
                    new Claim(ClaimTypes.Name, Uvm.NombreUsuario),
                    new Claim(ClaimTypes.Role , Uvm.Tipo_usuarioId.ToString())
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("Index", "Home");
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
                User.FechaNacimiento = Convert.ToDateTime(Uvm.FechaNacimiento.Date.ToString("dd/MM/yyyy"));
                var edad = (Convert.ToInt16(Uvm.FechaNacimiento.Date.ToString("yyyy")) - DateTime.Now.Year) * -1;
                User.Edad = edad.ToString();
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


                var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, Uvm.Id.ToString()),
                    new Claim(ClaimTypes.Name, Uvm.NombreUsuario),
                    new Claim(ClaimTypes.Role , Uvm.Tipo_usuarioId.ToString())
                    };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                return RedirectToAction("Index", "Home");

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
            if (usuario == null)
            {
                Usuario? User = new Usuario();

                //Creando usuario nuevo
                User.Nombres = Uvm.Nombres.ToUpper();
                User.Rut = Uvm.Rut;
                User.Apellidos = Uvm.Apellidos.ToUpper();
                User.Correo = Uvm.Correo.ToUpper();
                User.FechaNacimiento = Convert.ToDateTime(Uvm.FechaNacimiento.Date.ToString("dd/MM/yyyy"));
                var edad = (Convert.ToInt16(Uvm.FechaNacimiento.Date.ToString("yyyy")) - DateTime.Now.Year) * -1;
                User.Edad = edad.ToString();
                User.Telefono = "9 " + Uvm.Telefono;
                User.NombreUsuario = Uvm.NombreUsuario;
                User.Organizacion = Uvm.Organizacion;
                User.EstadoCuenta = "ACTIVA";
                User.Tipo_usuarioId = 3;
                CreatePasswordHash(Uvm.Contrasena, out byte[] passwordHash, out byte[] passworSalt);
                User.PasswordHash = passwordHash;
                User.PasswordSalt = passworSalt;
                _context.Add(User);
                await _context.SaveChangesAsync();


                var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, Uvm.Id.ToString()),
                    new Claim(ClaimTypes.Name, Uvm.NombreUsuario),
                    new Claim(ClaimTypes.Role , Uvm.Tipo_usuarioId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

                    return RedirectToAction("Index", "Home");

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

        public async Task<IActionResult> Edit(int id)
        {
            var U = _context.tblUsuario.FirstOrDefault(u => u.Id == id);
            if (U == null) return NotFound();
            UsuarioRegistroViewModel Uvm = new UsuarioRegistroViewModel()
            {
                Nombres = U.Nombres,
                Apellidos = U.Apellidos,
                Correo = U.Correo,
                Rut = U.Rut,
                Tipo_usuarioId = U.Tipo_usuarioId,
                NombreUsuario = U.NombreUsuario,
                Organizacion = U.Organizacion,
                Telefono = U.Telefono
            };
            return View(Uvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var Usuarios = _context.tblUsuario.Where(u => u.NombreUsuario == Uvm.NombreUsuario || u.Correo == Uvm.Correo || u.Rut == Uvm.Rut).ToList();
                if (Usuarios.Count >= 1)
                {
                    var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Uvm.Id);
                    U.Nombres = Uvm.Nombres;
                    U.Apellidos = Uvm.Apellidos;
                    U.Correo = Uvm.Correo;
                    U.Tipo_usuarioId = 3;
                    U.NombreUsuario = Uvm.NombreUsuario;
                    U.Organizacion = Uvm.Organizacion;
                    U.Telefono = Uvm.Telefono;
                    U.Rut = Uvm.Rut;
                    U.Id = Uvm.Id;

                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    U.PasswordSalt = Salt;
                    U.PasswordHash = Hash;

                    _context.Update(U);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Usuario ya tiene una cuenta creada. Nombre de usuario, correo o Rut ya están en uso");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditPremium(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var Usuarios = _context.tblUsuario.Where(u => u.NombreUsuario == Uvm.NombreUsuario || u.Correo == Uvm.Correo || u.Rut == Uvm.Rut).ToList();
                if (Usuarios.Count >= 1)
                {
                    var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Uvm.Id);
                    U.Nombres = Uvm.Nombres;
                    U.Apellidos = Uvm.Apellidos;
                    U.Correo = Uvm.Correo;
                    U.Tipo_usuarioId = 4;
                    U.NombreUsuario = Uvm.NombreUsuario;
                    U.Organizacion = Uvm.Organizacion;
                    U.Telefono = Uvm.Telefono;
                    U.Rut = Uvm.Rut;
                    U.Id = Uvm.Id;

                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    U.PasswordSalt = Salt;
                    U.PasswordHash = Hash;

                    _context.Update(U);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Usuario ya tiene una cuenta creada. Nombre de usuario, correo o Rut ya están en uso");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var Usuarios = _context.tblUsuario.Where(u => u.NombreUsuario == Uvm.NombreUsuario || u.Correo == Uvm.Correo || u.Rut == Uvm.Rut).ToList();
                if (Usuarios.Count >= 1)
                {
                    var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Uvm.Id);
                    U.Nombres = Uvm.Nombres;
                    U.Apellidos = Uvm.Apellidos;
                    U.Correo = Uvm.Correo;
                    U.Tipo_usuarioId = 1;
                    U.NombreUsuario = Uvm.NombreUsuario;
                    U.Organizacion = Uvm.Organizacion;
                    U.Telefono = Uvm.Telefono;
                    U.Rut = Uvm.Rut;
                    U.Id = Uvm.Id;

                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    U.PasswordSalt = Salt;
                    U.PasswordHash = Hash;

                    _context.Update(U);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Usuario ya tiene una cuenta creada. Nombre de usuario, correo o Rut ya están en uso");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }

        }



        [HttpPost]
        public async Task<IActionResult> EditSoporte(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var Usuarios = _context.tblUsuario.Where(u => u.NombreUsuario == Uvm.NombreUsuario || u.Correo == Uvm.Correo || u.Rut == Uvm.Rut).ToList();
                if (Usuarios.Count >= 1)
                {
                    var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Uvm.Id);
                    U.Nombres = Uvm.Nombres;
                    U.Apellidos = Uvm.Apellidos;
                    U.Correo = Uvm.Correo;
                    U.Tipo_usuarioId = 2;
                    U.NombreUsuario = Uvm.NombreUsuario;
                    U.Organizacion = Uvm.Organizacion;
                    U.Telefono = Uvm.Telefono;
                    U.Rut = Uvm.Rut;
                    U.Id = Uvm.Id;

                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    U.PasswordSalt = Salt;
                    U.PasswordHash = Hash;

                    _context.Update(U);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Usuario ya tiene una cuenta creada. Nombre de usuario, correo o Rut ya están en uso");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }

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