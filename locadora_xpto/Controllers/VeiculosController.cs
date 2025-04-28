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
    /// Controller para gerenciar veículos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public VeiculosController(LocadoraContext context) => _context = context;

        /// <summary>
        /// Lista veículos com filtros opcionais: fabricante, ano ou quilometragem mínima.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Veiculo>>> Get(
            [FromQuery] int? fabricanteId,
            [FromQuery] int? ano,
            [FromQuery] int? quilometragemMinima)
        {
            var query = _context.Veiculos
                                .Include(v => v.Fabricante)
                                .AsQueryable();

            if (fabricanteId.HasValue)
                query = query.Where(v => v.FabricanteId == fabricanteId.Value);
            if (ano.HasValue)
                query = query.Where(v => v.AnoFabricacao == ano.Value);
            if (quilometragemMinima.HasValue)
                query = query.Where(v => v.Quilometragem >= quilometragemMinima.Value);

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Obtém um veículo por ID.
        /// </summary>
        /// <param name="id">ID do veículo.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Veiculo>> Get(int id)
        {
            var v = await _context.Veiculos
                                  .Include(vh => vh.Fabricante)
                                  .FirstOrDefaultAsync(vh => vh.VeiculoId == id);
            if (v == null) return NotFound();
            return Ok(v);
        }

        /// <summary>
        /// Cria um novo veículo.
        /// </summary>
        /// <param name="dto">DTO com dados do veículo.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Veiculo>> Post([FromBody] CreateVeiculoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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

        /// <summary>
        /// Atualiza um veículo existente.
        /// </summary>
        /// <param name="id">ID do veículo.</param>
        /// <param name="dto">DTO com dados para atualização.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateVeiculoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.VeiculoId) return BadRequest();

            var veiculo = await _context.Veiculos.FindAsync(id);
            if (veiculo == null) return NotFound();

            veiculo.Modelo = dto.Modelo;
            veiculo.AnoFabricacao = dto.AnoFabricacao;
            veiculo.Quilometragem = dto.Quilometragem;
            veiculo.FabricanteId = dto.FabricanteId;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um veículo.
        /// </summary>
        /// <param name="id">ID do veículo.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _context.Veiculos.FindAsync(id);
            if (v == null) return NotFound();

            _context.Veiculos.Remove(v);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DTOs de entrada
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

        public class UpdateVeiculoDto : CreateVeiculoDto
        {
            [Required]
            public int VeiculoId { get; set; }
        }
    }
}
