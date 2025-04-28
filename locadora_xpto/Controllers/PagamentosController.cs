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
    /// Controller para gerenciar pagamentos de aluguéis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public PagamentosController(LocadoraContext context) => _context = context;

        /// <summary>
        /// Obtém pagamentos, opcionalmente filtrando por aluguel.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Pagamento>>> Get([FromQuery] int? aluguelId)
        {
            var query = _context.Pagamentos.AsQueryable();
            if (aluguelId.HasValue)
                query = query.Where(p => p.AluguelId == aluguelId.Value);
            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Obtém um pagamento pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Pagamento>> Get(int id)
        {
            var p = await _context.Pagamentos.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        /// <summary>
        /// Cria um novo pagamento.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagamento>> Post([FromBody] CreatePagamentoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var pagamento = new Pagamento
            {
                AluguelId = dto.AluguelId,
                DataPagamento = dto.DataPagamento,
                ValorPago = dto.ValorPago,
                MetodoPagamento = dto.MetodoPagamento
            };

            _context.Pagamentos.Add(pagamento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = pagamento.PagamentoId }, pagamento);
        }

        /// <summary>
        /// Atualiza um pagamento existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePagamentoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.PagamentoId) return BadRequest();

            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null) return NotFound();

            pagamento.AluguelId = dto.AluguelId;
            pagamento.DataPagamento = dto.DataPagamento;
            pagamento.ValorPago = dto.ValorPago;
            pagamento.MetodoPagamento = dto.MetodoPagamento;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove um pagamento.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Pagamentos.FindAsync(id);
            if (p == null) return NotFound();

            _context.Pagamentos.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DTOs de entrada
        public class CreatePagamentoDto
        {
            [Required]
            public int AluguelId { get; set; }

            [Required]
            public System.DateTime DataPagamento { get; set; }

            [Required, Range(0, double.MaxValue)]
            public decimal ValorPago { get; set; }

            [StringLength(50)]
            public string? MetodoPagamento { get; set; }
        }

        public class UpdatePagamentoDto : CreatePagamentoDto
        {
            [Required]
            public int PagamentoId { get; set; }
        }
    }
}
