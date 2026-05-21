using Bnp.Paribas.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bnp.Paribas.Infra.Config;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produto", t =>
            t.HasCheckConstraint("CK_Produto_StaStatus", "StaStatus IN ('A', 'I')"));

        builder.HasKey(p => p.CodProduto);

        builder.Property(p => p.CodProduto).IsRequired();
        builder.Property(p => p.DesProduto).IsRequired(false);
        builder.Property(p => p.StaStatus).IsRequired(false);

        builder.HasMany(p => p.ProdutoCosifs)
            .WithOne(pc => pc.Produto)
            .HasForeignKey(pc => pc.CodProduto);
    }
}