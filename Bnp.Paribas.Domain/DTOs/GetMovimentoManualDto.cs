namespace Bnp.Paribas.Domain.DTOs;

public class GetMovimentoManualDto
{
    public int DatMes { get; set; }
    public int DatAno { get; set; }
    public string CodProduto { get; set; } = null!;
    public string? DesProduto { get; set; }
    public long NumLancamento { get; set; }
    public string DesDescricao { get; set; } = null!;
    public decimal ValValor { get; set; }
}