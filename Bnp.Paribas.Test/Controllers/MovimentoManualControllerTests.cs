using Bnp.Paribas.API.Controller;
using Bnp.Paribas.Domain.DTOs;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bnp.Paribas.Test.Controllers;

public class MovimentoManualControllerTests
{
    private readonly Mock<IMovimentoManualRepository> _repositoryMock;
    private readonly MovimentoManualController _controller;

    public MovimentoManualControllerTests()
    {
        _repositoryMock = new Mock<IMovimentoManualRepository>();
        _controller = new MovimentoManualController(_repositoryMock.Object);
    }

    [Fact]
    public async Task Post_QuandoDtoValido_DeveRetornarCreated()
    {
        // Arrange
        var dto = new InsertMovimentoManualDto
        {
            DatMes = 5,
            DatAno = 2012,
            CodProduto = "0001",
            CodCosif = "12345678901",
            ValValor = 500.00m,
            DesDescricao = "Teste Movimentos",
            CodUsuario = "candidato"
        };

        _repositoryMock
            .Setup(r => r.InsertAsync(dto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Post(dto);

        // Assert
        Assert.IsType<CreatedResult>(result);
        _repositoryMock.Verify(r => r.InsertAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Get_QuandoExistemMovimentos_DeveRetornarOkComLista()
    {
        // Arrange
        var movimentos = new List<GetMovimentoManualDto>
        {
            new()
            {
                DatMes = 5,
                DatAno = 2012,
                CodProduto = "0001",
                DesProduto = "Produto Teste",
                NumLancamento = 1,
                DesDescricao = "Teste Movimentos",
                ValValor = 500.00m
            }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(movimentos);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetMovimentoManualDto>>(okResult.Value);
        Assert.Single(retorno);
    }

    [Fact]
    public async Task Get_QuandoNaoExistemMovimentos_DeveRetornarOkComListaVazia()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetMovimentoManualDto>>(okResult.Value);
        Assert.Empty(retorno);
    }
}