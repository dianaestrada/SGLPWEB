using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models.ViewModels;

namespace SGLPWEB.Controllers.MVC
{
    public class PortalClienteMVCController : Controller
    {
        private readonly AppDbContext _context;

        public PortalClienteMVCController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (rol != "Cliente" || clienteId == null)
                return RedirectToAction("Index", "Login"); // ✅ corregido: evita bucle

            var casosDelCliente = await _context.Casos
                .Where(c => c.ClienteId == clienteId)
                .Include(c => c.Tareas)
                .Include(c => c.Plazos)
                .ToListAsync();

            return View(casosDelCliente);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (rol != "Cliente" || clienteId == null)
                return RedirectToAction("Index", "Login"); 

            var caso = await _context.Casos
                .Include(c => c.Tareas)
                .Include(c => c.Plazos)
                .Include(c => c.Evidencias)
                .FirstOrDefaultAsync(c => c.CasoId == id && c.ClienteId == clienteId);

            if (caso == null)
                return NotFound();

            return View(caso);
        }

        public async Task<IActionResult> Dashboard()
        {
            var rol = HttpContext.Session.GetString("Rol");
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (rol != "Cliente" || clienteId == null)
                return RedirectToAction("Index", "Login");

            var casos = await _context.Casos
              .Where(c => c.ClienteId == clienteId)
              .Include(c => c.Tareas)
              .Include(c => c.Plazos)
              .Include(c => c.Evidencias)
              .ToListAsync();

            var modelo = new DashboardClienteViewModel
            {
                TotalCasos = casos.Count,
                CasosActivos = casos.Count(c => c.Estado == "Activo"),
                CasosFinalizados = casos.Count(c => c.Estado == "Finalizado"),
                TareasPendientes = casos.SelectMany(c => c.Tareas).Count(t => t.Estado != "Completado"),
                PlazosProximos = casos.SelectMany(c => c.Plazos).Count(p => p.EstadoPlazo == "Próximo"),
                UltimaEvidencia = casos.SelectMany(c => c.Evidencias)
                    .OrderByDescending(e => e.FechaSubida)
                    .FirstOrDefault(),
                UltimoCaso = casos.OrderByDescending(c => c.FechaInicio).FirstOrDefault()
            };

            return View("DashboardCliente", modelo); // ✅ vista explicitada
        }

       
    }
}