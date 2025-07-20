using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGLPWEB.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlazosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlazosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plazo>>> GetPlazos()
        {
            return await _context.Plazos
                                 .Include(p => p.Caso)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Plazo>> GetPlazo(int id)
        {
            var plazo = await _context.Plazos
                                      .Include(p => p.Caso)
                                      .FirstOrDefaultAsync(p => p.PlazoId == id);

            if (plazo == null)
                return NotFound();

            return plazo;
        }

        [HttpPost]
        public async Task<ActionResult<Plazo>> PostPlazo(PlazoDTO dto)
        {
            var plazo = new Plazo
            {
                CasoId = dto.CasoId,
                FechaEvento = dto.FechaEvento,
                TipoEvento = dto.TipoEvento,
                Descripcion = dto.Descripcion
            };

            _context.Plazos.Add(plazo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlazo), new { id = plazo.PlazoId }, plazo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlazo(int id, PlazoDTO dto)
        {
            var plazo = await _context.Plazos.FindAsync(id);
            if (plazo == null)
                return NotFound();

            plazo.CasoId = dto.CasoId;
            plazo.FechaEvento = dto.FechaEvento;
            plazo.TipoEvento = dto.TipoEvento;
            plazo.Descripcion = dto.Descripcion;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlazo(int id)
        {
            var plazo = await _context.Plazos.FindAsync(id);
            if (plazo == null)
                return NotFound();

            _context.Plazos.Remove(plazo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlazoExists(int id)
        {
            return _context.Plazos.Any(p => p.PlazoId == id);
        }

    }
}
