using Microsoft.EntityFrameworkCore;
using LocadoraXpto.Models;

namespace LocadoraXpto.Data
{
    public class LocadoraContext : DbContext
    {
        public LocadoraContext(DbContextOptions<LocadoraContext> options)
            : base(options)
        {
        }

        public DbSet<Fabricante> Fabricantes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Aluguel> Alugueis { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
                        .HasIndex(c => c.CPF)
                        .IsUnique();

            modelBuilder.Entity<Veiculo>()
                        .HasIndex(v => new { v.Modelo, v.AnoFabricacao });
        }
    }
}
