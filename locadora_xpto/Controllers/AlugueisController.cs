using System;
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
    public class CreateAluguelDto
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int VeiculoId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        [Required]
        public int QuilometragemInicial { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal ValorDiaria { get; set; }
    }

    public class UpdateAluguelDto
    {
        [Required]
        public int AluguelId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int VeiculoId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        public DateTime? DataDevolucao { get; set; }

        [Required]
        public int QuilometragemInicial { get; set; }

        public int? QuilometragemFinal { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal ValorDiaria { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ValorTotal { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class AlugueisController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public AlugueisController(LocadoraContext context) => _context = context;

        // GET: api/alugueis?clienteId=1&ativo=true&valorMin=500
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aluguel>>> Get(
            [FromQuery] int? clienteId,
            [FromQuery] bool? ativo,
            [FromQuery] decimal? valorMin)
        {
            var query = _context.Alugueis
                                .Include(a => a.Cliente)
                                .Include(a => a.Veiculo)
                                .AsQueryable();

            if (clienteId.HasValue)
                query = query.Where(a => a.ClienteId == clienteId.Value);
            if (ativo == true)
                query = query.Where(a => a.DataDevolucao == null);
            if (valorMin.HasValue)
                query = query.Where(a => a.ValorTotal >= valorMin.Value);

            return await query.ToListAsync();
        }

        // GET: api/alugueis/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aluguel>> Get(int id)
        {
            var a = await _context.Alugueis
                                   .Include(a => a.Cliente)
                                   .Include(a => a.Veiculo)
                                   .FirstOrDefaultAsync(a => a.AluguelId == id);
            if (a == null) return NotFound();
            return a;
        }

        // POST: api/alugueis
        [HttpPost]
        public async Task<ActionResult<Aluguel>> Post([FromBody] CreateAluguelDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var aluguel = new Aluguel
            {
                ClienteId = dto.ClienteId,
                VeiculoId = dto.VeiculoId,
                DataInicio = dto.DataInicio,
                DataFim = dto.DataFim,
                QuilometragemInicial = dto.QuilometragemInicial,
                ValorDiaria = dto.ValorDiaria
            };

            _context.Alugueis.Add(aluguel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = aluguel.AluguelId }, aluguel);
        }

        // PUT: api/alugueis/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateAluguelDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.AluguelId)
                return BadRequest();

            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null)
                return NotFound();

            aluguel.ClienteId = dto.ClienteId;
            aluguel.VeiculoId = dto.VeiculoId;
            aluguel.DataInicio = dto.DataInicio;
            aluguel.DataFim = dto.DataFim;
            aluguel.DataDevolucao = dto.DataDevolucao;
            aluguel.QuilometragemInicial = dto.QuilometragemInicial;
            aluguel.QuilometragemFinal = dto.QuilometragemFinal;
            aluguel.ValorDiaria = dto.ValorDiaria;
            aluguel.ValorTotal = dto.ValorTotal;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/alugueis/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _context.Alugueis.FindAsync(id);
            if (a == null)
                return NotFound();

            _context.Alugueis.Remove(a);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
