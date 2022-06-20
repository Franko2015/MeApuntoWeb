﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MeApuntoWeb.Models;
using MeApuntoWeb.ViewModels;

namespace MeApuntoWeb.Controllers
{
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

        public async Task<IActionResult> CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var U = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario);
                if (U == null)
                {

                    Usuario Us = new Usuario();
                    Us.NombreUsuario = Uvm.NombreUsuario;
                    Us.Rut = Uvm.Rut;
                    Us.Apellidos = Uvm.Apellidos;
                    Us.Nombres = Uvm.Nombres;
                    Us.Telefono = Uvm.Telefono;
                    Us.Edad = Uvm.Edad;
                    Us.EstadoCuenta = "ACTIVA";
                    Us.Organizacion = Uvm.Organizacion;
                    Us.Tipo_usuarioId = 1;
                    Us.Correo = Uvm.Correo;


                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    Us.PasswordSalt = Salt;
                    Us.PasswordHash = Hash;

                    _context.Add(Us);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Username o Correo ya Existen!");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateSoporte(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var U = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario);
                if (U == null)
                {
                    Usuario Us = new Usuario();
                    Us.NombreUsuario = Uvm.NombreUsuario;
                    Us.Rut = Uvm.Rut;
                    Us.Apellidos = Uvm.Apellidos;
                    Us.Nombres = Uvm.Nombres;
                    Us.Telefono = Uvm.Telefono;
                    Us.EstadoCuenta = "ACTIVA";
                    Us.Organizacion = Uvm.Organizacion;
                    Us.Tipo_usuarioId = 2;
                    Us.Correo = Uvm.Correo;


                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    Us.PasswordSalt = Salt;
                    Us.PasswordHash = Hash;

                    _context.Add(Us);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Username o Correo ya Existen!");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFree(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var U = _context.tblUsuario.FirstOrDefault(u => u.NombreUsuario == Uvm.NombreUsuario);
                if (U == null)
                {
                    Usuario Us = new Usuario();
                    Us.NombreUsuario = Uvm.NombreUsuario;
                    Us.Rut = Uvm.Rut;
                    Us.Apellidos = Uvm.Apellidos;
                    Us.Nombres = Uvm.Nombres;
                    Us.Telefono = Uvm.Telefono;
                    Us.EstadoCuenta = "ACTIVA";
                    Us.Organizacion = Uvm.Organizacion;
                    Us.Tipo_usuarioId = 3;
                    Us.Correo = Uvm.Correo;


                    CreatePasswordHash(Uvm.Contrasena, out byte[] Hash, out byte[] Salt);

                    Us.PasswordSalt = Salt;
                    Us.PasswordHash = Hash;

                    _context.Add(Us);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Ya existe el mismo Username o Email
                    ModelState.AddModelError(String.Empty, "Username o Correo ya Existen!");
                    return View(Uvm);
                }

            }
            else
            {
                return View(Uvm);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Id);
            if (U == null) return NotFound();
            UsuarioRegistroViewModel Uvm = new UsuarioRegistroViewModel()
            {
            Id = U.Id,
            NombreUsuario = U.NombreUsuario,
            Rut = U.Rut,
            Apellidos = U.Apellidos,
            Nombres = U.Nombres,
            Telefono = U.Telefono,
            EstadoCuenta = "ACTIVA",
            Organizacion = U.Organizacion,
            Tipo_usuarioId = U.Tipo_usuarioId,
            Correo = U.Correo
            };
            return View(Uvm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioRegistroViewModel Uvm)
        {
            if (ModelState.IsValid)
            {
                var Usuarios = _context.tblUsuario.Where(u => u.NombreUsuario == Uvm.NombreUsuario).ToList();
                if (Usuarios.Count >= 1)
                {
                    var U = _context.tblUsuario.FirstOrDefault(u => u.Id == Uvm.Id);
                    U.Nombres = Uvm.Nombres;
                    U.Apellidos = Uvm.Apellidos;
                    U.EstadoCuenta = Uvm.EstadoCuenta;
                    U.Correo = Uvm.Correo;
                    U.Rut = Uvm.Rut;
                    U.Organizacion = Uvm.Organizacion;
                    U.NombreUsuario = Uvm.NombreUsuario;
                    U.Telefono = Uvm.Telefono;
                    U.Edad = Uvm.Edad;
                    U.Tipo_usuarioId = Uvm.Tipo_usuarioId;
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
                    ModelState.AddModelError(String.Empty, "Username o Correo ya Existen!");
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