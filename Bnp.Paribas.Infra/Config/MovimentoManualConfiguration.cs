using Bnp.Paribas.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bnp.Paribas.Infra.Config;

public class MovimentoManualConfiguration : IEntityTypeConfiguration<MovimentoManual>
{
    public void Configure(EntityTypeBuilder<MovimentoManual> builder)
    {
        builder.ToTable("MovimentoManual");

        builder.HasKey(m => new { m.DatMes, m.DatAno, m.NumLancamento });

        builder.Property(m => m.NumLancamento)
            .ValueGeneratedNever();

        builder.Property(m => m.ValValor)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(m => m.ProdutoCosif)
            .WithMany()
            .HasForeignKey(m => new { m.CodProduto, m.CodCosif });
    }
}