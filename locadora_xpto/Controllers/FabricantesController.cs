using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using LocadoraXpto.Data;
using LocadoraXpto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocadoraXpto.Controllers
{
    /// <summary>
    /// Controller para gerenciar fabricantes de veículos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FabricantesController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public FabricantesController(LocadoraContext context) => _context = context;

        /// <summary>
        /// Obtém todos os fabricantes, com filtro opcional por nome.
        /// </summary>
        /// <param name="nome">Texto parcial ou completo do nome do fabricante.</param>
        /// <returns>Lista de fabricantes.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Fabricante>>> Get([FromQuery] string? nome)
        {
            var query = _context.Fabricantes.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(f => f.Nome.Contains(nome));
            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Obtém um fabricante pelo ID.
        /// </summary>
        /// <param name="id">ID do fabricante.</param>
        /// <returns>Objeto fabricante ou 404.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Fabricante>> Get(int id)
        {
            var fab = await _context.Fabricantes.FindAsync(id);
            if (fab == null) return NotFound();
            return Ok(fab);
        }

        /// <summary>
        /// Cria um novo fabricante.
        /// </summary>
        /// <param name="dto">DTO com os dados do fabricante.</param>
        /// <returns>Objeto criado.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Fabricante>> Post([FromBody] CreateFabricanteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fabricante = new Fabricante { Nome = dto.Nome };
            _context.Fabricantes.Add(fabricante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = fabricante.FabricanteId }, fabricante);
        }

        /// <summary>
        /// Atualiza um fabricante existente.
        /// </summary>
        /// <param name="id">ID do fabricante.</param>
        /// <param name="dto">DTO com dados para atualização.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateFabricanteDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.FabricanteId) return BadRequest();

            var fabricante = await _context.Fabricantes.FindAsync(id);
            if (fabricante == null) return NotFound();

            fabricante.Nome = dto.Nome;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um fabricante.
        /// </summary>
        /// <param name="id">ID do fabricante.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var fab = await _context.Fabricantes.FindAsync(id);
            if (fab == null) return NotFound();

            _context.Fabricantes.Remove(fab);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DTOs de entrada
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
    }
}
