using Asp.Versioning;
using Bnp.Paribas.Domain.DTOs;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bnp.Paribas.API.Controller;

[ApiController]
[ApiVersion(1)]
[Route("v{version:apiVersion}/movimentos")]
public class MovimentoManualController(IMovimentoManualRepository repository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InsertMovimentoManualDto dto)
    {
        await repository.InsertAsync(dto);
        return Created();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var movimentos = await repository.GetAllAsync();
        return Ok(movimentos);
    }
}
