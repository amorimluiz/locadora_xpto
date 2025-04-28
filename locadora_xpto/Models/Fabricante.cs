using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocadoraXpto.Models
{
    [Table("fabricantes")]
    public class Fabricante
    {
        [Key]
        public int FabricanteId { get; set; }

        [Required, StringLength(100)]
        public string Nome { get; set; }

        // 1 Fabricante : N Veículos
        public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}