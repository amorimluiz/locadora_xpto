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
    public class CreateVeiculoDto
    {
        [Required, StringLength(50)]
        public string Modelo { get; set; } = null!;

        [Required]
        public int AnoFabricacao { get; set; }

        [Required]
        public int Quilometragem { get; set; }

        [Required]
        public int FabricanteId { get; set; }
    }

    public class UpdateVeiculoDto
    {
        [Required]
        public int VeiculoId { get; set; }

        [Required, StringLength(50)]
        public string Modelo { get; set; } = null!;

        [Required]
        public int AnoFabricacao { get; set; }

        [Required]
        public int Quilometragem { get; set; }

        [Required]
        public int FabricanteId { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public VeiculosController(LocadoraContext context) => _context = context;

        // GET: api/veiculos?fabricanteId=1&ano=2020&kmMin=10000
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veiculo>>> Get(
            [FromQuery] int? fabricanteId,
            [FromQuery] int? ano,
            [FromQuery] int? kmMin)
        {
            var query = _context.Veiculos.Include(v => v.Fabricante).AsQueryable();

            if (fabricanteId.HasValue)
                query = query.Where(v => v.FabricanteId == fabricanteId.Value);
            if (ano.HasValue)
                query = query.Where(v => v.AnoFabricacao == ano.Value);
            if (kmMin.HasValue)
                query = query.Where(v => v.Quilometragem >= kmMin.Value);

            return await query.ToListAsync();
        }

        // GET: api/veiculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Veiculo>> Get(int id)
        {
            var v = await _context.Veiculos
                .Include(vh => vh.Fabricante)
                .FirstOrDefaultAsync(vh => vh.VeiculoId == id);

            if (v == null) return NotFound();
            return v;
        }

        // POST: api/veiculos
        [HttpPost]
        public async Task<ActionResult<Veiculo>> Post([FromBody] CreateVeiculoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var veiculo = new Veiculo
            {
                Modelo = dto.Modelo,
                AnoFabricacao = dto.AnoFabricacao,
                Quilometragem = dto.Quilometragem,
                FabricanteId = dto.FabricanteId
            };

            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = veiculo.VeiculoId }, veiculo);
        }

        // PUT: api/veiculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateVeiculoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.VeiculoId)
                return BadRequest();

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null)
                return NotFound();

            veiculo.Modelo = dto.Modelo;
            veiculo.AnoFabricacao = dto.AnoFabricacao;
            veiculo.Quilometragem = dto.Quilometragem;
            veiculo.FabricanteId = dto.FabricanteId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/veiculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _context.Veiculos.FindAsync(id);
            if (v == null)
                return NotFound();

            _context.Veiculos.Remove(v);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
