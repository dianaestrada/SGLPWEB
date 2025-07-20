using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;

namespace SGLPWEB.Controllers.MVC
{
    public class PlazosMVCController : Controller
    {
        private readonly AppDbContext _context;

        public PlazosMVCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PlazosMVC
        // GET: PlazosMVC
        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            var plazos = await _context.Plazos
                .Include(p => p.Caso)
                .ToListAsync();

            return View(plazos);
        }

        // GET: PlazosMVC/Create
        [HttpGet]
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();

            ViewData["CasoId"] = new SelectList(_context.Casos, "CasoId", "TipoCaso");
            return View(new PlazoDTO());
        }

        // POST: PlazosMVC/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlazoDTO dto)
        {
            if (dto.FechaEvento < DateTime.Today)
            {
                ModelState.AddModelError("FechaEvento", "La fecha del evento no puede estar en el pasado.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CasoId"] = new SelectList(_context.Casos, "CasoId", "TipoCaso", dto.CasoId);
                return View(dto);
            }

            var plazo = new Plazo
            {
                CasoId = dto.CasoId,
                FechaEvento = dto.FechaEvento,
                TipoEvento = dto.TipoEvento,
                Descripcion = dto.Descripcion
            };

            _context.Plazos.Add(plazo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: PlazosMVC/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var plazo = await _context.Plazos
                .Include(p => p.Caso)
                .FirstOrDefaultAsync(p => p.PlazoId == id);

            if (plazo == null) return NotFound();

            return View(plazo);
        }

        // GET: PlazosMVC/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol == "Cliente") return Forbid();

            var plazo = await _context.Plazos.FindAsync(id);
            if (plazo == null) return NotFound();

            var dto = new PlazoDTO
            {
                PlazoId = plazo.PlazoId,
                CasoId = plazo.CasoId,
                FechaEvento = plazo.FechaEvento,
                TipoEvento = plazo.TipoEvento,
                Descripcion = plazo.Descripcion
            };

            ViewData["CasoId"] = new SelectList(_context.Casos, "CasoId", "TipoCaso", dto.CasoId);
            return View(dto);
        }

        // POST: PlazosMVC/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlazoDTO dto)
        {
            if (id != dto.PlazoId) return NotFound();

            if (dto.FechaEvento < DateTime.Today)
            {
                ModelState.AddModelError("FechaEvento", "La fecha del evento no puede estar en el pasado.");
            }

            if (!ModelState.IsValid)
            {
                ViewData["CasoId"] = new SelectList(_context.Casos, "CasoId", "TipoCaso", dto.CasoId);
                return View(dto);
            }

            var plazo = await _context.Plazos.FindAsync(id);
            if (plazo == null) return NotFound();

            plazo.CasoId = dto.CasoId;
            plazo.FechaEvento = dto.FechaEvento;
            plazo.TipoEvento = dto.TipoEvento;
            plazo.Descripcion = dto.Descripcion;

            try
            {
                _context.Update(plazo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlazoExists(id)) return NotFound();
                throw;
            }
        }

        // GET: PlazosMVC/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "Administrador") return Forbid();

            var plazo = await _context.Plazos
                .Include(p => p.Caso)
                .FirstOrDefaultAsync(p => p.PlazoId == id);

            if (plazo == null) return NotFound();

            return View(plazo);
        }

        // POST: PlazosMVC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plazo = await _context.Plazos.FindAsync(id);
            if (plazo != null)
            {
                _context.Plazos.Remove(plazo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PlazoExists(int id)
        {
            return _context.Plazos.Any(p => p.PlazoId == id);
        }
    }
}