using Bnp.Paribas.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bnp.Paribas.Infra.Config;

public class ProdutoCosifConfiguration : IEntityTypeConfiguration<ProdutoCosif>
{
    public void Configure(EntityTypeBuilder<ProdutoCosif> builder)
    {
        builder.ToTable("ProdutoCosif", t =>
        {
            t.HasCheckConstraint("CK_ProdutoCosif_StaStatus", "StaStatus IN ('A', 'I')");
            t.HasCheckConstraint("CK_ProdutoCosif_CodClassificacao", "CodClassificacao IN ('Normal', 'MTM')");
        });

        builder.HasKey(pc => new { pc.CodProduto, pc.CodCosif });

        builder.Property(pc => pc.CodProduto).IsRequired();
        builder.Property(pc => pc.CodCosif).IsRequired();
        builder.Property(pc => pc.CodClassificacao).IsRequired(false);
        builder.Property(pc => pc.StaStatus).IsRequired(false);
    }
}