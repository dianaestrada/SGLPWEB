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
    public class TareasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TareasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            return await _context.Tareas
                                 .Include(t => t.Caso)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas
                                      .Include(t => t.Caso)
                                      .FirstOrDefaultAsync(t => t.TareaId == id);

            if (tarea == null)
                return NotFound();

            return tarea;
        }

        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(TareaDTO dto)
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

            return CreatedAtAction(nameof(GetTarea), new { id = tarea.TareaId }, tarea);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, TareaDTO dto)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound();

            tarea.CasoId = dto.CasoId;
            tarea.ResponsableId = dto.ResponsableId;
            tarea.Descripcion = dto.Descripcion;
            tarea.FechaLimite = dto.FechaLimite;
            tarea.Prioridad = dto.Prioridad;
            tarea.Estado = dto.Estado;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
                return NotFound();

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(t => t.TareaId == id);
        }

    }
}
