using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;

namespace SGLPWEB.Controllers.MVC
{
    public class CasosMVCController : Controller
    {
        private readonly AppDbContext _context;

        public CasosMVCController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CasosMVC
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Casos.Include(c => c.Cliente);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CasosMVC/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caso = await _context.Casos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CasoId == id);
            if (caso == null)
            {
                return NotFound();
            }

            return View(caso);
        }

        // GET: CasosMVC/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre");
            return View();
        }

        // POST: CasosMVC/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CasoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var caso = new Caso
                {
                    ClienteId = dto.ClienteId,
                    TipoCaso = dto.TipoCaso,
                    FechaInicio = dto.FechaInicio,
                    Descripcion = dto.Descripcion,
                    Estado = dto.Estado
                };

                _context.Add(caso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", dto.ClienteId);
            return View(dto);
        }

        // GET: CasosMVC/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var caso = await _context.Casos.FindAsync(id);
            if (caso == null)
                return NotFound();

            var dto = new CasoDTO
            {
                ClienteId = caso.ClienteId,
                TipoCaso = caso.TipoCaso,
                FechaInicio = caso.FechaInicio,
                Descripcion = caso.Descripcion,
                Estado = caso.Estado
            };

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", dto.ClienteId);
            return View(dto);
        }


        // POST: CasosMVC/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CasoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", dto.ClienteId);
                return View(dto);
            }

            var caso = await _context.Casos.FindAsync(id);
            if (caso == null)
                return NotFound();

            caso.ClienteId = dto.ClienteId;
            caso.TipoCaso = dto.TipoCaso;
            caso.FechaInicio = dto.FechaInicio;
            caso.Descripcion = dto.Descripcion;
            caso.Estado = dto.Estado;

            try
            {
                _context.Update(caso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CasoExists(id))
                    return NotFound();
                throw;
            }
        }


        // GET: CasosMVC/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caso = await _context.Casos
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.CasoId == id);
            if (caso == null)
            {
                return NotFound();
            }

            return View(caso);
        }

        // POST: CasosMVC/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var caso = await _context.Casos.FindAsync(id);
            if (caso != null)
            {
                _context.Casos.Remove(caso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CasoExists(int id)
        {
            return _context.Casos.Any(e => e.CasoId == id);
        }
    }
}
