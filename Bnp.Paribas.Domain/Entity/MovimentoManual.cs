using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bnp.Paribas.Domain.Entity;

public class MovimentoManual
{
    public int DatMes { get; set; }

    public int DatAno { get; set; }

    public long NumLancamento { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string CodProduto { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string CodCosif { get; set; } = null!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal ValValor { get; set; }

    [Required]
    [Column(TypeName = "varchar(100)")]
    public string DesDescricao { get; set; } = null!;

    public DateTime DatMovimento { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string CodUsuario { get; set; } = null!;

    [ForeignKey($"{nameof(CodProduto)},{nameof(CodCosif)}")]
    public ProdutoCosif ProdutoCosif { get; set; } = null!;
}