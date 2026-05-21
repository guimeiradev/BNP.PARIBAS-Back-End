using Bnp.Paribas.API.Controller;
using Bnp.Paribas.Domain.DTOs;
using Bnp.Paribas.Domain.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bnp.Paribas.Test.Controllers;

public class ProdutoControllerTests
{
    private readonly Mock<IProdutoRepository> _repositoryMock;
    private readonly ProdutoController _controller;

    public ProdutoControllerTests()
    {
        _repositoryMock = new Mock<IProdutoRepository>();
        _controller = new ProdutoController(_repositoryMock.Object);
    }

    [Fact]
    public async Task Get_QuandoExistemProdutos_DeveRetornarOkComLista()
    {
        // Arrange
        var produtos = new List<GetProdutoDto>
        {
            new() { CodProduto = "0001", DesProduto = "Produto Teste" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(produtos);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetProdutoDto>>(okResult.Value);
        Assert.Single(retorno);
    }

    [Fact]
    public async Task Get_QuandoNaoExistemProdutos_DeveRetornarOkComListaVazia()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync([]);

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetProdutoDto>>(okResult.Value);
        Assert.Empty(retorno);
    }

    [Fact]
    public async Task GetCosifs_QuandoExistemCosifs_DeveRetornarOkComLista()
    {
        // Arrange
        var codProduto = "0001";
        var cosifs = new List<GetProdutoCosifDto>
        {
            new() { CodCosif = "12345678901", CodClassificacao = "Normal" }
        };

        _repositoryMock
            .Setup(r => r.GetCosifsByProdutoAsync(codProduto))
            .ReturnsAsync(cosifs);

        // Act
        var result = await _controller.GetCosifs(codProduto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetProdutoCosifDto>>(okResult.Value);
        Assert.Single(retorno);
    }

    [Fact]
    public async Task GetCosifs_QuandoNaoExistemCosifs_DeveRetornarOkComListaVazia()
    {
        // Arrange
        var codProduto = "0001";

        _repositoryMock
            .Setup(r => r.GetCosifsByProdutoAsync(codProduto))
            .ReturnsAsync([]);

        // Act
        var result = await _controller.GetCosifs(codProduto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<GetProdutoCosifDto>>(okResult.Value);
        Assert.Empty(retorno);
    }
}