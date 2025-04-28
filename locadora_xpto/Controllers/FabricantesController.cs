using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LocadoraXpto.Data;
using LocadoraXpto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraXpto.Controllers
{
    // DTOs para entrada
    public class CreateFabricanteDto
    {
        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;
    }

    public class UpdateFabricanteDto
    {
        [Required]
        public int FabricanteId { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; } = null!;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FabricantesController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public FabricantesController(LocadoraContext context) => _context = context;

        // GET: api/fabricantes?nome=xyz
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fabricante>>> Get([FromQuery] string? nome)
        {
            var query = _context.Fabricantes.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(f => f.Nome.Contains(nome));
            return await query.ToListAsync();
        }

        // GET: api/fabricantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fabricante>> Get(int id)
        {
            var fab = await _context.Fabricantes.FindAsync(id);
            if (fab == null) return NotFound();
            return fab;
        }

        // POST: api/fabricantes
        [HttpPost]
        public async Task<ActionResult<Fabricante>> Post([FromBody] CreateFabricanteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fabricante = new Fabricante { Nome = dto.Nome };
            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = fabricante.FabricanteId }, fabricante);
        }

        // PUT: api/fabricantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateFabricanteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.FabricanteId)
                return BadRequest();

            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null)
                return NotFound();

            fabricante.Nome = dto.Nome;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/fabricantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var fab = await _context.Fabricantes.FindAsync(id);
            if (fab == null)
                return NotFound();

            _context.Fabricantes.Remove(fab);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
