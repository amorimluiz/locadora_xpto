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
    public class CreateClienteDto
    {
        [Required, StringLength(150)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(14)]
        public string CPF { get; set; } = null!;

        [Required, StringLength(200), EmailAddress]
        public string Email { get; set; } = null!;
    }

    public class UpdateClienteDto
    {
        [Required]
        public int ClienteId { get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; } = null!;

        [Required, StringLength(14)]
        public string CPF { get; set; } = null!;

        [Required, StringLength(200), EmailAddress]
        public string Email { get; set; } = null!;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly LocadoraContext _context;
        public ClientesController(LocadoraContext context) => _context = context;

        // GET: api/clientes?nome=ana
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Get([FromQuery] string? nome)
        {
            var query = _context.Clientes.AsQueryable();
            if (!string.IsNullOrEmpty(nome))
                query = query.Where(c => c.Nome.Contains(nome));
            return await query.ToListAsync();
        }

        // GET: api/clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> Get(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return NotFound();
            return c;
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult<Cliente>> Post([FromBody] CreateClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cliente = new Cliente
            {
                Nome = dto.Nome,
                CPF = dto.CPF,
                Email = dto.Email
            };

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = cliente.ClienteId }, cliente);
        }

        // PUT: api/clientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateClienteDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != dto.ClienteId)
                return BadRequest();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
                return NotFound();

            cliente.Nome = dto.Nome;
            cliente.CPF = dto.CPF;
            cliente.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null)
                return NotFound();

            _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
