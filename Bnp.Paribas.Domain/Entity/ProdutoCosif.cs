using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bnp.Paribas.Domain.Entity;

public class ProdutoCosif
{
    [Column(TypeName = "varchar(20)")]
    public string CodProduto { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string CodCosif { get; set; } = null!;

    [Column(TypeName = "varchar(20)")]
    public string? CodClassificacao { get; set; }

    [Column(TypeName = "char(1)")]
    public char? StaStatus { get; set; }

    [ForeignKey(nameof(CodProduto))]
    public Produto Produto { get; set; } = null!;
}