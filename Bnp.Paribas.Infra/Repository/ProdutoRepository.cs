using Bnp.Paribas.Domain.DTOs;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Bnp.Paribas.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Bnp.Paribas.Infra.Repository;

public class ProdutoRepository(BnpContext context) : IProdutoRepository
{
    public async Task<IEnumerable<GetProdutoDto>> GetAllAsync()
    {
        return await context.Produtos
            .Select(p => new GetProdutoDto
            {
                CodProduto = p.CodProduto,
                DesProduto = p.DesProduto
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<GetProdutoCosifDto>> GetCosifsByProdutoAsync(string codProduto)
    {
        return await context.ProdutosCosif
            .Where(pc => pc.CodProduto == codProduto)
            .Select(pc => new GetProdutoCosifDto
            {
                CodCosif = pc.CodCosif,
                CodClassificacao = pc.CodClassificacao
            })
            .ToListAsync();
    }
}