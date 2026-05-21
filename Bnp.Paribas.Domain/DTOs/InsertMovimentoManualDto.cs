namespace Bnp.Paribas.Domain.DTOs;

public class InsertMovimentoManualDto
{
    public int DatMes { get; set; }
    public int DatAno { get; set; }
    public string CodProduto { get; set; } = null!;
    public string CodCosif { get; set; } = null!;
    public decimal ValValor { get; set; }
    public string DesDescricao { get; set; } = null!;
    public string CodUsuario { get; set; } = null!;
}
