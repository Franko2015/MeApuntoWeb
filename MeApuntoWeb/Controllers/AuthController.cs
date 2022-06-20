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

        public  IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {

            var ADMIN = _context.tblUsuario.ToList();
            Usuario? UA = new Usuario();
            if (ADMIN.Count == 0)
            {
                //Creando usuario Administrador
                UA.Nombres = "Franco Alberto";
                UA.Apellidos = "Millanes Araya";
                UA.Rut = "";
                UA.Correo = "Administrador@gmail.com";
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

            var SOPORTE = _context.tblUsuario.ToList();
            Usuario? US = new Usuario();
            if (SOPORTE.Count == 0)
            {
                //Creando usuario Soporte
                US.Nombres = "SOPORTE";
                US.Rut = "";
                US.Apellidos = "SOPORTE";
                US.Correo = "soporte@gmail.com";
                US.Edad = "20";
                US.Telefono = "";
                US.NombreUsuario = "soporte";
                US.Organizacion = "Me Apunto";
                US.EstadoCuenta = "Activa";
                US.Tipo_usuarioId = 1;
                CreatePasswordHash("soporte", out byte[] passwordHash, out byte[] passworSalt);
                US.PasswordHash = passwordHash;
                US.PasswordSalt = passworSalt;
                _context.Add(US);
                await _context.SaveChangesAsync();
            }


            //Login
            UA = null;
            UA = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == lvm.Username); //admin
            if (UA == null)
            {
                ModelState.AddModelError(String.Empty, "Usuario NO Encontrado");
                return View(lvm);
            }
            else
            {
                if (!VerifyPasswordHash(lvm.Password, UA.PasswordHash, UA.PasswordSalt)) //123456
                {
                    ModelState.AddModelError(String.Empty, "Password Incorrecta");
                    return View(lvm);
                }
                else
                {
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.NameIdentifier, UA.Id.ToString()),
                        new Claim(ClaimTypes.Name, UA.NombreUsuario),
                        new Claim(ClaimTypes.Role , UA.Tipo_usuarioId.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties { IsPersistent = true });
                    return RedirectToAction("Index","Home");
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
