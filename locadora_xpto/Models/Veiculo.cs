using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraXpto.Models
{
    [Table("veiculos")]
    public class Veiculo
    {
        [Key]
        public int VeiculoId { get; set; }

        [Required, StringLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int AnoFabricacao { get; set; }

        [Required]
        public int Quilometragem { get; set; }

        // FK → Fabricante
        [ForeignKey(nameof(Fabricante))]
        public int FabricanteId { get; set; }
        public Fabricante Fabricante { get; set; } = null!;

        // 1 Veículo : N Aluguéis
        public ICollection<Aluguel> Alugueis { get; set; } = new List<Aluguel>();
    }
}
