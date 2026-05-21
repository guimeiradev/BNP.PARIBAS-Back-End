using Bnp.Paribas.Domain.DTOs;

namespace Bnp.Paribas.Domain.Interfaces.Repository;

public interface IMovimentoManualRepository
{
    Task InsertAsync(InsertMovimentoManualDto dto);
    Task<IEnumerable<GetMovimentoManualDto>> GetAllAsync();
}
