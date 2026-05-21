using Bnp.Paribas.Domain.Interfaces.Repository;
using Bnp.Paribas.Infra.Repository;

namespace Bnp.Paribas.API.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMovimentoManualRepository, MovimentoManualRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        return services;
    }
}