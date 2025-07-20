using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;

namespace SGLPWEB.Controllers.MVC
{
    public class TareasMVCController : Controller
    {
        private readonly AppDbContext _context;

        public TareasMVCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TareasMVC
        public async Task<IActionResult> Index()
        {
            var tareas = _context.Tareas
                .Include(t => t.Caso)
                .Include(t => t.Responsable);

            return View(await tareas.ToListAsync());
        }

        // GET: Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var tarea = await _context.Tareas
                .Include(t => t.Caso)
                .Include(t => t.Responsable)
                .FirstOrDefaultAsync(m => m.TareaId == id);

            if (tarea == null) return NotFound();

            return View(tarea);
        }

        // GET: Create
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();

            CargarCombos();
            return View(new TareaDTO());
        }
        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TareaDTO dto)
        {
            if (ModelState.IsValid)
            {
                var tarea = new Tarea
                {
                    CasoId = dto.CasoId,
                    ResponsableId = dto.ResponsableId,
                    Descripcion = dto.Descripcion,
                    FechaLimite = dto.FechaLimite,
                    Prioridad = dto.Prioridad,
                    Estado = dto.Estado
                };

                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarCombos(dto.CasoId, dto.ResponsableId); 
            return View(dto);
        }

        // GET: Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();
            if (id == null) return NotFound();

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            var dto = new TareaDTO
            {
                TareaId = tarea.TareaId,
                CasoId = tarea.CasoId,
                ResponsableId = tarea.ResponsableId,
                Descripcion = tarea.Descripcion,
                FechaLimite = tarea.FechaLimite,
                Prioridad = tarea.Prioridad,
                Estado = tarea.Estado
            };

            CargarCombos(dto.CasoId, dto.ResponsableId);
            return View(dto);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TareaDTO dto)
        {
            if (id != dto.TareaId) return NotFound();
            if (!ModelState.IsValid)
            {
                CargarCombos(dto.CasoId, dto.ResponsableId);
                return View(dto);
            }

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null) return NotFound();

            tarea.CasoId = dto.CasoId;
            tarea.ResponsableId = dto.ResponsableId;
            tarea.Descripcion = dto.Descripcion;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Prioridad = dto.Prioridad;
            tarea.Estado = dto.Estado;

            try
            {
                _context.Update(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(dto.TareaId)) return NotFound();
                throw;
            }
        }

        // GET: Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador") return Forbid();
            if (id == null) return NotFound();

            var tarea = await _context.Tareas
                .Include(t => t.Caso)
                .Include(t => t.Responsable)
                .FirstOrDefaultAsync(m => m.TareaId == id);

            if (tarea == null) return NotFound();

            return View(tarea);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
        private void CargarCombos(int? casoId = null, int? responsableId = null)
        {
            ViewData["CasoId"] = new SelectList(_context.Casos, "CasoId", "TipoCaso", casoId);

            var empleados = _context.Usuarios
                .Include(u => u.Rol)
                .Where(u => u.Rol != null && u.Rol.Nombre == "Empleado")
                .Select(u => new Usuario { UsuarioId = u.UsuarioId, Nombre = u.Nombre })
                .ToList();


            ViewData["ResponsableId"] = new SelectList(empleados, "UsuarioId", "Nombre", responsableId);
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(t => t.TareaId == id);
        }
    }
}