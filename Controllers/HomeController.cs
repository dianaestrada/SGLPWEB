using Microsoft.AspNetCore.Mvc;

namespace SGLPWEB.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            var nombre = HttpContext.Session.GetString("Nombre");
            var rol = HttpContext.Session.GetString("Rol");

            if (string.IsNullOrEmpty(nombre))
                return RedirectToAction("Index", "Login");

            ViewBag.Nombre = nombre;
            ViewBag.Rol = rol;

            return View();
        }
    }
}