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

    public class UpdatePagamentoDto
    {
        [Required]
        public int PagamentoId { get; set; }

        [Required]
        public int AluguelId { get; set; }

        [Required]
        public System.DateTime DataPagamento { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal ValorPago { get; set; }

        [StringLength(50)]
        public string? MetodoPagamento { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public PagamentosController(LocadoraContext context) => _context = context;

        // GET: api/pagamentos?aluguelId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> Get([FromQuery] int? aluguelId)
        {
            var query = _context.Pagamentos.AsQueryable();
            if (aluguelId.HasValue)
                query = query.Where(p => p.AluguelId == aluguelId.Value);
            return await query.ToListAsync();
        }

        // GET: api/pagamentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> Get(int id)
        {
            var p = await _context.Pagamentos.FindAsync(id);
            if (p == null) return NotFound();
            return p;
        }

        // POST: api/pagamentos
        [HttpPost]
        public async Task<ActionResult<Pagamento>> Post([FromBody] CreatePagamentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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

        // PUT: api/pagamentos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePagamentoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.PagamentoId)
                return BadRequest();

            var pagamento = await _context.Pagamentos.FindAsync(id);
            if (pagamento == null)
                return NotFound();

            pagamento.AluguelId = dto.AluguelId;
            pagamento.DataPagamento = dto.DataPagamento;
            pagamento.ValorPago = dto.ValorPago;
            pagamento.MetodoPagamento = dto.MetodoPagamento;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/pagamentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Pagamentos.FindAsync(id);
            if (p == null)
                return NotFound();

            _context.Pagamentos.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
