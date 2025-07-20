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
    public class EvidenciasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EvidenciasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evidencia>>> GetEvidencias()
        {
            return await _context.Evidencias
                                 .Include(e => e.Tarea)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Evidencia>> GetEvidencia(int id)
        {
            var evidencia = await _context.Evidencias
                                          .Include(e => e.Tarea)
                                          .FirstOrDefaultAsync(e => e.EvidenciaId == id);

            if (evidencia == null)
                return NotFound();

            return evidencia;
        }

        [HttpPost]
        public async Task<ActionResult<Evidencia>> PostEvidencia(EvidenciaDTO dto)
        {
            var tarea = await _context.Tareas.FindAsync(dto.TareaId);
            if (tarea == null)
            {
                return BadRequest("La tarea asociada no existe.");
            }

            var evidencia = new Evidencia
            {
                TareaId = dto.TareaId,
                NombreArchivo = dto.NombreArchivo,
                RutaArchivo = dto.RutaArchivo,
                FechaSubida = dto.FechaSubida
            };

            _context.Evidencias.Add(evidencia);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvidencia), new { id = evidencia.EvidenciaId }, evidencia);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvidencia(int id, EvidenciaDTO dto)
        {
            var evidencia = await _context.Evidencias.FindAsync(id);
            if (evidencia == null)
                return NotFound();

            evidencia.TareaId = dto.TareaId;
            evidencia.NombreArchivo = dto.NombreArchivo;
            evidencia.RutaArchivo = dto.RutaArchivo;
            evidencia.FechaSubida = dto.FechaSubida;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvidencia(int id)
        {
            var evidencia = await _context.Evidencias.FindAsync(id);
            if (evidencia == null)
                return NotFound();

            _context.Evidencias.Remove(evidencia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EvidenciaExists(int id)
        {
            return _context.Evidencias.Any(e => e.EvidenciaId == id);
        }

    }
}
