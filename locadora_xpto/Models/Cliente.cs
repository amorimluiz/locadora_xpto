using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraXpto.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        public int ClienteId { get; set; }

        [Required, StringLength(150)]
        public string Nome { get; set; }

        [Required, StringLength(14)]
        public string CPF { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; }

        // 1 Cliente : N Aluguéis
        public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
