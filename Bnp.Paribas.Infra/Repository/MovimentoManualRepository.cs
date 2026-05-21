using Bnp.Paribas.Domain.DTOs;
using Bnp.Paribas.Domain.Entity;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Bnp.Paribas.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Bnp.Paribas.Infra.Repository;

public class MovimentoManualRepository(BnpContext context) : IMovimentoManualRepository
{
    public async Task InsertAsync(InsertMovimentoManualDto dto)
    {
        var maxLancamento = await context.MovimentosManuais
            .Where(m => m.DatMes == dto.DatMes && m.DatAno == dto.DatAno)
            .Select(m => (int?)m.NumLancamento)
            .MaxAsync();

        var numLancamento = (maxLancamento ?? 0) + 1;

        var movimento = new MovimentoManual
        {
            DatMes = dto.DatMes,
            DatAno = dto.DatAno,
            NumLancamento = numLancamento,
            CodProduto = dto.CodProduto,
            CodCosif = dto.CodCosif,
            ValValor = dto.ValValor,
            DesDescricao = dto.DesDescricao,
            CodUsuario = dto.CodUsuario,
            DatMovimento = DateTime.Now
        };

        context.MovimentosManuais.Add(movimento);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<GetMovimentoManualDto>> GetAllAsync()
    {
        return await context.MovimentosManuais
            .Include(m => m.ProdutoCosif)
                .ThenInclude(pc => pc.Produto)
            .Select(m => new GetMovimentoManualDto
            {
                DatMes = m.DatMes,
                DatAno = m.DatAno,
                CodProduto = m.CodProduto,
                DesProduto = m.ProdutoCosif.Produto.DesProduto,
                NumLancamento = m.NumLancamento,
                DesDescricao = m.DesDescricao,
                ValValor = m.ValValor
            })
            .ToListAsync();
    }
}