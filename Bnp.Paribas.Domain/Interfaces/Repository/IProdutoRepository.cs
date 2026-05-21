using Bnp.Paribas.Domain.DTOs;

namespace Bnp.Paribas.Domain.Interfaces.Repository;

public interface IProdutoRepository
{
    Task<IEnumerable<GetProdutoDto>> GetAllAsync();
    Task<IEnumerable<GetProdutoCosifDto>> GetCosifsByProdutoAsync(string codProduto);
}