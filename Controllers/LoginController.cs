using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.ViewModels;

namespace SGLPWEB.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.Correo == model.Correo &&
                                     u.ClaveHash == model.Clave &&
                                     u.Activo);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrecta.");
                return View(model);
            }

            // Guardar datos base en sesión
            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
            HttpContext.Session.SetString("Nombre", usuario.Nombre);
            HttpContext.Session.SetString("Rol", usuario.Rol.Nombre);
            HttpContext.Session.SetString("Correo", usuario.Correo);

            // Redirigir según rol
            switch (usuario.Rol.Nombre)
            {
                case "Cliente":
                    var cliente = _context.Clientes
                        .FirstOrDefault(c => c.Correo == usuario.Correo);

                    if (cliente != null)
                        HttpContext.Session.SetInt32("ClienteId", cliente.ClienteId);

                    return RedirectToAction("Dashboard", "PortalClienteMVC");

                case "Empleado":
                case "Administrador":
                    return RedirectToAction("Dashboard", "Home");

                default:
                    HttpContext.Session.Clear();
                    return RedirectToAction("Index", "Login");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}