using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraXpto.Models
{
    [Table("alugueis")]
    public class Aluguel
    {
        [Key]
        public int AluguelId { get; set; }

        [Required]
        public DateTime DataInicio { get; set; }

        [Required]
        public DateTime DataFim { get; set; }

        public DateTime? DataDevolucao { get; set; }

        [Required]
        public int QuilometragemInicial { get; set; }

        public int? QuilometragemFinal { get; set; }

        [Required, Column(TypeName = "decimal(10,2)")]
        public decimal ValorDiaria { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal ValorTotal { get; set; }

        // FK → Cliente
        [ForeignKey(nameof(Cliente))]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        // FK → Veículo
        [ForeignKey(nameof(Veiculo))]
        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; } = null!;

        // 1 Aluguel : N Pagamentos
        public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    }
}
