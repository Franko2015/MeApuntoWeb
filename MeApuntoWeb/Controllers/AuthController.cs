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

            //Login usuario Administrador, Soporte o User
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == lvm.Username);// admin/soporte/user
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "Nombre de usuario incorrecto");
                return View(lvm);
            }
            else
            {
                if (!VerifyPasswordHash(lvm.Password, user.PasswordHash, user.PasswordSalt)) //admin/soporte/user
                {
                    ModelState.AddModelError(String.Empty, "Contraseña de usuario incorrecta");
                    return View(lvm);
                }
                else
                {

                    var claims = new List<Claim>{
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.NombreUsuario),
                    new Claim(ClaimTypes.Role , user.Tipo_usuarioId.ToString())
                    };

                    
                    if (user.EstadoCuenta == "ACTIVA")
                    {
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            new AuthenticationProperties { IsPersistent = true });

                        return RedirectToAction("Index", "Home");
                    }
                    return RedirectToAction(nameof(LogOut));
                }
            }        
        }

        public async Task<IActionResult> Premium()
        {
            var user = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == User.Identity.Name);

            if (user == null) return NotFound();

            user.Tipo_usuarioId = 4;

            _context.Update(user);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
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
