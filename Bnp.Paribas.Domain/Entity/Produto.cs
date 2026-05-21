using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bnp.Paribas.Domain.Entity;

public class Produto
{
    [Key]
    [Column(TypeName = "varchar(20)")]
    public string CodProduto { get; set; } = null!;

    [Column(TypeName = "varchar(100)")]
    public string? DesProduto { get; set; }

    [Column(TypeName = "char(1)")]
    public char? StaStatus { get; set; }

    public ICollection<ProdutoCosif> ProdutoCosifs { get; set; } = [];
}