using Bnp.Paribas.Domain.Entity;
using Bnp.Paribas.Infra.Context;

namespace Bnp.Paribas.Infra.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(BnpContext context)
    {
        if (context.Produtos.Any())
            return;

        var produtos = new List<Produto>
        {
            new() { CodProduto = "PROD001", DesProduto = "Fundo de Renda Fixa", StaStatus = 'A' },
            new() { CodProduto = "PROD002", DesProduto = "Fundo de Ações", StaStatus = 'A' },
            new() { CodProduto = "PROD003", DesProduto = "Fundo Multimercado", StaStatus = 'I' },
        };

        var cosifs = new List<ProdutoCosif>
        {
            new() { CodProduto = "PROD001", CodCosif = "1.1.1.40.10", CodClassificacao = "Normal", StaStatus = 'A' },
            new() { CodProduto = "PROD001", CodCosif = "1.1.1.40.20", CodClassificacao = "MTM",    StaStatus = 'A' },
            new() { CodProduto = "PROD002", CodCosif = "1.1.2.50.10", CodClassificacao = "Normal", StaStatus = 'A' },
            new() { CodProduto = "PROD003", CodCosif = "1.1.3.60.10", CodClassificacao = "MTM",    StaStatus = 'I' },
        };

        context.Produtos.AddRange(produtos);
        context.ProdutosCosif.AddRange(cosifs);
        await context.SaveChangesAsync();
    }
}