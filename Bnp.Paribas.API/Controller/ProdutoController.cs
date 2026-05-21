using Asp.Versioning;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bnp.Paribas.API.Controller;

[ApiController]
[ApiVersion(1)]
[Route("v{version:apiVersion}/produtos")]
public class ProdutoController(IProdutoRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var produtos = await repository.GetAllAsync();
        return Ok(produtos);
    }

    [HttpGet("{codProduto}/cosifs")]
    public async Task<IActionResult> GetCosifs(string codProduto)
    {
        var cosifs = await repository.GetCosifsByProdutoAsync(codProduto);
        return Ok(cosifs);
    }
}