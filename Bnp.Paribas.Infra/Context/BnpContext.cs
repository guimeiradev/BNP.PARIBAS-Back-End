using Bnp.Paribas.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Bnp.Paribas.Infra.Context;

public class BnpContext(DbContextOptions<BnpContext> options) : DbContext(options)
{
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<ProdutoCosif> ProdutosCosif { get; set; }
    public DbSet<MovimentoManual> MovimentosManuais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BnpContext).Assembly);
    }
}
