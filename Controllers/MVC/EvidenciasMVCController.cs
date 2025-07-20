using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;

namespace SGLPWEB.Controllers.MVC
{
    public class EvidenciasMVCController : Controller
    {
        private readonly AppDbContext _context;

        public EvidenciasMVCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: EvidenciasMVC
        public async Task<IActionResult> Index()
        {
            var evidencias = await _context.Evidencias
                .Include(e => e.Tarea)
                .ToListAsync();

            return View(evidencias);
        }

        // GET: EvidenciasMVC/Create
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();

            var tareas = _context.Tareas
                .Include(t => t.Caso)
                .Select(t => new
                {
                    t.TareaId,
                    Nombre = $"[{t.TareaId}] {t.Caso.TipoCaso} - {t.Descripcion}"
                }).ToList();

            ViewData["TareaId"] = new SelectList(tareas, "TareaId", "Nombre");
            return View(new EvidenciaDTO());
        }

        // POST: EvidenciasMVC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EvidenciaDTO dto)
        {
            if (ModelState.IsValid)
            {
                var evidencia = new Evidencia
                {
                    TareaId = dto.TareaId,
                    NombreArchivo = dto.NombreArchivo,
                    RutaArchivo = dto.RutaArchivo,
                    FechaSubida = dto.FechaSubida
                };

                _context.Evidencias.Add(evidencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var tareas = _context.Tareas
                .Include(t => t.Caso)
                .Select(t => new
                {
                    t.TareaId,
                    Nombre = $"[{t.TareaId}] {t.Caso.TipoCaso} - {t.Descripcion}"
                }).ToList();

            ViewData["TareaId"] = new SelectList(tareas, "TareaId", "Nombre", dto.TareaId);
            return View(dto);
        }

        // GET: EvidenciasMVC/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();

            var evidencia = await _context.Evidencias.FindAsync(id);
            if (evidencia == null) return NotFound();

            var dto = new EvidenciaDTO
            {
                EvidenciaId = evidencia.EvidenciaId,
                TareaId = evidencia.TareaId,
                NombreArchivo = evidencia.NombreArchivo,
                RutaArchivo = evidencia.RutaArchivo,
                FechaSubida = evidencia.FechaSubida
            };

            var tareas = _context.Tareas
                .Include(t => t.Caso)
                .Select(t => new
                {
                    t.TareaId,
                    Nombre = $"[{t.TareaId}] {t.Caso.TipoCaso} - {t.Descripcion}"
                }).ToList();

            ViewData["TareaId"] = new SelectList(tareas, "TareaId", "Nombre", dto.TareaId);
            return View(dto);
        }

        // POST: EvidenciasMVC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EvidenciaDTO dto)
        {
            if (id != dto.EvidenciaId) return NotFound();

            if (!ModelState.IsValid)
            {
                var tareas = _context.Tareas
                    .Include(t => t.Caso)
                    .Select(t => new
                    {
                        t.TareaId,
                        Nombre = $"[{t.TareaId}] {t.Caso.TipoCaso} - {t.Descripcion}"
                    }).ToList();

                ViewData["TareaId"] = new SelectList(tareas, "TareaId", "Nombre", dto.TareaId);
                return View(dto);
            }

            var evidencia = await _context.Evidencias.FindAsync(id);
            if (evidencia == null) return NotFound();

            evidencia.TareaId = dto.TareaId;
            evidencia.NombreArchivo = dto.NombreArchivo;
            evidencia.RutaArchivo = dto.RutaArchivo;
            evidencia.FechaSubida = dto.FechaSubida;

            try
            {
                _context.Update(evidencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvidenciaExists(id)) return NotFound();
                throw;
            }
        }

        // GET: EvidenciasMVC/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador") return Forbid();

            var evidencia = await _context.Evidencias
                .Include(e => e.Tarea)
                .FirstOrDefaultAsync(e => e.EvidenciaId == id);

            if (evidencia == null) return NotFound();

            return View(evidencia);
        }

        // POST: EvidenciasMVC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var evidencia = await _context.Evidencias.FindAsync(id);
            if (evidencia != null)
            {
                _context.Evidencias.Remove(evidencia);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: EvidenciasMVC/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var evidencia = await _context.Evidencias
                .Include(e => e.Tarea)
                .FirstOrDefaultAsync(e => e.EvidenciaId == id);

            if (evidencia == null) return NotFound();

            return View(evidencia);
        }

        private bool EvidenciaExists(int id)
        {
            return _context.Evidencias.Any(e => e.EvidenciaId == id);
        }
    }
}