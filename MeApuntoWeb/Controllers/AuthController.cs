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


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {

            //Admin
            var tipo = _context.tblTipo_usuario.ToList();
            TipoUsuario? T = new TipoUsuario();
            if (tipo.Count == 0)
            {
                T.Tipo = "Administrador";
                _context.Add(T);
                await _context.SaveChangesAsync();
            }

            //Soporte
            var tipoSoporte = _context.tblTipo_usuario.ToList();
            TipoUsuario? TS = new TipoUsuario();
            if (tipoSoporte.Count == 0)
            {
                TS.Tipo = "Soporte";
                _context.Add(TS);
                await _context.SaveChangesAsync();
            }

            //Cliente FREE
            var tipoClienteFree = _context.tblTipo_usuario.ToList();
            TipoUsuario? TCF = new TipoUsuario();
            if (tipoClienteFree.Count == 0)
            {
                TCF.Tipo = "FREE";
                _context.Add(TCF);
                await _context.SaveChangesAsync();
            }

            //Cliente PREMIUM
            var tipoClientePremium = _context.tblTipo_usuario.ToList();
            TipoUsuario? TCP = new TipoUsuario();
            if (tipoClientePremium.Count == 0)
            {
                TCP.Tipo = "PREMIUM";
                _context.Add(TCP);
                await _context.SaveChangesAsync();
            }


            var ADMIN = _context.tblUsuario.ToList();
            Usuario? UA = new Usuario();
            if (ADMIN.Count == 0)
            {
                //Creando usuario Administrador
                UA.Nombres = "Franco Alberto";
                UA.Apellidos = "Millanes Araya";
                UA.Correo = "Administrador@gmail.com";
                UA.Edad = "20";
                UA.NombreUsuario = "admin";
                UA.Organizacion = "Me Apunto";
                UA.EstadoCuenta = "Activa";
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
                US.Apellidos = "SOPORTE";
                US.Correo = "soporte@gmail.com";
                US.Edad = "20";
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
            UA = _context.tblUsuario.FirstOrDefault(u => (u.NombreUsuario) == lvm.Username); //admin
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
                    return RedirectToAction(nameof(Perfil));
                }
            }
        }

        public IActionResult Perfil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var Usuario = _context.tblUsuario.FirstOrDefault(u => (u.NombreUsuario).Trim() == User.Identity.Name);
                PerfilViewModel pvm = new PerfilViewModel()
                {
                    Usuario = Usuario
                };

                return View(pvm);
            }
            //return View();
            return RedirectToAction(nameof(Login));
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
