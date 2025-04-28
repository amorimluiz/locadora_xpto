using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraXpto.Models
{
    [Table("pagamentos")]
    public class Pagamento
    {
        [Key]
        public int PagamentoId { get; set; }

        [Required]
        public DateTime DataPagamento { get; set; }

        [Required, Column(TypeName = "decimal(12,2)")]
        public decimal ValorPago { get; set; }

        [StringLength(50)]
        public string MetodoPagamento { get; set; }

        // FK → Aluguel
        [ForeignKey(nameof(Aluguel))]
        public int AluguelId { get; set; }
        public Aluguel Aluguel { get; set; } = null!;
    }
}
