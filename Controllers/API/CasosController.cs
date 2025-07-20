using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGLPWEB.Data;
using SGLPWEB.Models;
using SGLPWEB.Models.DTO;
using System.Threading.Tasks;

namespace SGLPWEB.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CasosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CasosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Caso>>> GetCasos()
        {
            return await _context.Casos
                                 .Include(c => c.Cliente)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Caso>> GetCaso(int id)
        {
            var caso = await _context.Casos
                                     .Include(c => c.Cliente)
                                     .FirstOrDefaultAsync(c => c.CasoId == id);

            if (caso == null)
                return NotFound();

            return caso;
        }

        [HttpPost]
        public async Task<ActionResult<Caso>> PostCaso(CasoDTO dto)
        {
            var caso = new Caso
            {
                ClienteId = dto.ClienteId,
                TipoCaso = dto.TipoCaso,
                FechaInicio = dto.FechaInicio,
                Descripcion = dto.Descripcion,
                Estado = dto.Estado
            };

            _context.Casos.Add(caso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCaso), new { id = caso.CasoId }, caso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCaso(int id, CasoDTO dto)
        {
            var casoExistente = await _context.Casos.FindAsync(id);
            if (casoExistente == null)
                return NotFound();

            casoExistente.ClienteId = dto.ClienteId;
            casoExistente.TipoCaso = dto.TipoCaso;
            casoExistente.FechaInicio = dto.FechaInicio;
            casoExistente.Descripcion = dto.Descripcion;
            casoExistente.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCaso(int id)
        {
            var caso = await _context.Casos.FindAsync(id);
            if (caso == null)
                return NotFound();

            _context.Casos.Remove(caso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CasoExists(int id)
        {
            return _context.Casos.Any(c => c.CasoId == id);
        }

    }
}
