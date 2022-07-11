using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MeApuntoWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly EventosDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(EventosDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {


            var usuario = _context.tblUsuario.ToList();
            Usuario? UA = new Usuario();
            if (usuario.Count == 0)
            {
                //Creando usuario Administrador
                UA.Nombres = "admin";
                UA.Apellidos = "admin";
                UA.Rut = "";
                UA.Correo = "admin@gmail.com";
                UA.Edad = "20";
                UA.Telefono = "";
                UA.NombreUsuario = "admin";
                UA.Organizacion = "Me Apunto";
                UA.EstadoCuenta = "ACTIVA";
                UA.Tipo_usuarioId = 1;
                CreatePasswordHash("admin", out byte[] passwordHash, out byte[] passworSalt);
                UA.PasswordHash = passwordHash;
                UA.PasswordSalt = passworSalt;
                _context.Add(UA);
                await _context.SaveChangesAsync();
            }

            Usuario? US = new Usuario();
            if (usuario.Count == 1)
            {
                //Creando usuario Soporte
                US.Nombres = "soporte";
                US.Apellidos = "soporte";
                US.Rut = "";
                US.Correo = "soporte@gmail.com";
                US.Edad = "20";
                US.Telefono = "";
                US.NombreUsuario = "soporte";
                US.Organizacion = "Me Apunto";
                US.EstadoCuenta = "ACTIVA";
                US.Tipo_usuarioId = 2;
                CreatePasswordHash("soporte", out byte[] passwordHash, out byte[] passworSalt);
                US.PasswordHash = passwordHash;
                US.PasswordSalt = passworSalt;
                _context.Add(US);
                await _context.SaveChangesAsync();
            }

            Usuario? u = new Usuario();
            if (usuario.Count == 2)
            {
                //Creando usuario normal
                u.Nombres = "user";
                u.Apellidos = "user";
                u.Rut = "";
                u.Correo = "user@gmail.com";
                u.Edad = "20";
                u.Telefono = "";
                u.NombreUsuario = "user";
                u.Organizacion = "Me Apunto";
                u.EstadoCuenta = "ACTIVA";
                u.Tipo_usuarioId = 3;
                CreatePasswordHash("user", out byte[] passwordHash, out byte[] passworSalt);
                u.PasswordHash = passwordHash;
                u.PasswordSalt = passworSalt;
                _context.Add(u);
                await _context.SaveChangesAsync();
            }

            //Login usuario Administrador, Soporte o User
                var userAdmin = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == lvm.Username);// admin/soporte/user
                if (userAdmin == null)
                {
                    ModelState.AddModelError(String.Empty, "Nombre de usuario incorrecto");
                    return View(lvm);
                }
                else
                {
                    if (!VerifyPasswordHash(lvm.Password, userAdmin.PasswordHash, userAdmin.PasswordSalt)) //admin/soporte/user
                    {
                        ModelState.AddModelError(String.Empty, "Contraseña de usuario incorrecta");
                        return View(lvm);
                    }
                    else
                    {

                        var claims = new List<Claim>{
                        new Claim(ClaimTypes.NameIdentifier, userAdmin.Id.ToString()),
                        new Claim(ClaimTypes.Name, userAdmin.NombreUsuario),
                        new Claim(ClaimTypes.Role , userAdmin.Tipo_usuarioId.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties { IsPersistent = true });
                        return RedirectToAction("Index", "Home");

                    }
                }
                
        }

        public IActionResult Perfil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);
                PerfilViewModel pvm = new PerfilViewModel()
                {
                    Usuario = user
                };

                return View(pvm);
            }
            //return View();
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> AccesoDenegado()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }



    }
}
