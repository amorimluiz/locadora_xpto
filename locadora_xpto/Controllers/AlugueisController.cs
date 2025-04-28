using System;
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
    /// Controller para gerenciar aluguéis.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AlugueisController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public AlugueisController(LocadoraContext context) => _context = context;

        /// <summary>
        /// Lista aluguéis com filtros opcionais (cliente, ativo, valor mínimo).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

            return Ok(await query.ToListAsync());
        }

        /// <summary>
        /// Obtém um aluguel pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Aluguel>> Get(int id)
        {
            var a = await _context.Alugueis
                                   .Include(a => a.Cliente)
                                   .Include(a => a.Veiculo)
                                   .FirstOrDefaultAsync(a => a.AluguelId == id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        /// <summary>
        /// Cria um novo aluguel.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Aluguel>> Post([FromBody] CreateAluguelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

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

        /// <summary>
        /// Atualiza um aluguel existente.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateAluguelDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != dto.AluguelId) return BadRequest();

            var aluguel = await _context.Alugueis.FindAsync(id);
            if (aluguel == null) return NotFound();

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

        /// <summary>
        /// Remove um aluguel.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _context.Alugueis.FindAsync(id);
            if (a == null) return NotFound();

            _context.Alugueis.Remove(a);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DTOs de entrada
        public class CreateAluguelDto
        {
            [Required]
            public int ClienteId { get; set; }

            [Required]
            public int VeiculoId { get; set; }

            [Required]
            public System.DateTime DataInicio { get; set; }

            [Required]
            public System.DateTime DataFim { get; set; }

            [Required]
            public int QuilometragemInicial { get; set; }

            [Required, Range(0, double.MaxValue)]
            public decimal ValorDiaria { get; set; }
        }

        public class UpdateAluguelDto : CreateAluguelDto
        {
            [Required]
            public int AluguelId { get; set; }

            public int? QuilometragemFinal { get; set; }

            public System.DateTime? DataDevolucao { get; set; }

            [Range(0, double.MaxValue)]
            public decimal ValorTotal { get; set; }
        }
    }
}
