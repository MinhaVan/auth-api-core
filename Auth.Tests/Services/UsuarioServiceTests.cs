using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Moq;
using Xunit;
using Auth.Domain.Models;
using Auth.Domain.Enums;
using Auth.Domain.Interfaces.Repositories;
using Amazon.Runtime;
using AutoMapper;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Interfaces.Services;
using Auth.Service.Implementations;
using Auth.Domain.ViewModels;
using Auth.Service.Exceptions;
using System.Linq;
using Auth.Service.Configurations;
using Auth.Service.Configuration;
using Auth.Tests.Fixtures;

namespace Auth.Tests.Services;

public class UsuarioServiceTest
{
    private readonly Mock<Domain.Interfaces.Services.IAmazonService> _amazonServiceMock = new();
    private readonly Mock<IRedisRepository> _redisRepositoryMock = new();
    private readonly IMapper _mapper = MapperFixture.Get();
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
    private readonly Mock<IBaseRepository<UsuarioPermissao>> _usuarioPermissaoRepositoryMock = new();
    private readonly Mock<IPermissaoRepository> _permissaoRepositoryMock = new();
    private readonly Mock<IBaseRepository<Motorista>> _motoristaRepositoryMock = new();
    private readonly Mock<IUserContext> _userContextMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly UsuarioService _usuarioService;

    public UsuarioServiceTest()
    {
        _usuarioService = new UsuarioService(
            _usuarioRepositoryMock.Object,
            _userContextMock.Object,
            _tokenServiceMock.Object,
            _amazonServiceMock.Object,
            _usuarioPermissaoRepositoryMock.Object,
            _permissaoRepositoryMock.Object,
            _redisRepositoryMock.Object,
            _motoristaRepositoryMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task BuscarPaginadoAsync_ShouldReturnMappedResult()
    {
        // Arrange
        var paginatedResult = new Paginado<Usuario>(pagina: 1, tamanho: 10, quantidade: 100, data: new List<Usuario>());
        _usuarioRepositoryMock
            .Setup(repo => repo.BuscarPaginadoAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Expression<Func<Usuario, bool>>>()))
            .ReturnsAsync(paginatedResult);

        var mappedResult = new PaginadoViewModel<UsuarioViewModel>(pagina: 1, tamanho: 10, quantidade: 100, data: new List<UsuarioViewModel>());

        // Act
        var result = await _usuarioService.BuscarPaginadoAsync(1, 10);

        // Assert
        Assert.Equal(mappedResult.Data.Count, result.Data.Count);
        _usuarioRepositoryMock.Verify(repo => repo.BuscarPaginadoAsync(1, 10, It.IsAny<Expression<Func<Usuario, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task Registrar_ShouldRegisterUserSuccessfully()
    {
        // Arrange
        var userViewModel = new UsuarioNovoViewModel { CPF = "12345678900", EmpresaId = 1, Senha = "encodedPassword", IsMotorista = true };
        var usuario = new Usuario { Id = 1, CPF = "12345678900", EmpresaId = 1 };
        var permissaoPadroes = new List<Permissao> { new Permissao { Id = 1 } };

        _tokenServiceMock.Setup(service => service.Base64ToString(userViewModel.Senha)).Returns("decodedPassword");
        _usuarioRepositoryMock.Setup(repo => repo.BuscarPorCpfEmpresaAsync(userViewModel.CPF, userViewModel.EmpresaId)).ReturnsAsync((Usuario)null);
        _permissaoRepositoryMock.Setup(repo => repo.ObterPermissoesPadraoPorEmpresaPerfilAsync(userViewModel.EmpresaId, userViewModel.IsMotorista)).ReturnsAsync(permissaoPadroes);
        _usuarioRepositoryMock.Setup(repo => repo.ComputeHash("decodedPassword")).Returns("hashedPassword");

        // Act
        var result = await _usuarioService.Registrar(userViewModel);

        // Assert
        Assert.NotNull(result);
        _usuarioRepositoryMock.Verify(repo => repo.AdicionarAsync(It.Is<Usuario>(u => u.Senha == "hashedPassword" && u.Perfil == PerfilEnum.Motorista)), Times.Once);
        _motoristaRepositoryMock.Verify(repo => repo.AdicionarAsync(It.IsAny<Motorista>()), Times.Once);
        _amazonServiceMock.Verify(service => service.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task Registrar_ShouldThrowException_WhenUserAlreadyExists()
    {
        // Arrange
        var userViewModel = new UsuarioNovoViewModel { CPF = "12345678900", EmpresaId = 1 };
        var existingUser = new Usuario { Id = 1, CPF = "12345678900", EmpresaId = 1 };

        _usuarioRepositoryMock.Setup(repo => repo.BuscarPorCpfEmpresaAsync(userViewModel.CPF, userViewModel.EmpresaId)).ReturnsAsync(existingUser);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessRuleException>(() => _usuarioService.Registrar(userViewModel));
    }

    [Fact]
    public async Task Atualizar_ShouldUpdateUserSuccessfully()
    {
        // Arrange
        var userViewModel = new UsuarioAtualizarViewModel { Id = 1, CPF = "12345678900", Email = "test@example.com" };
        var existingUser = new Usuario { Id = 1, CPF = "12345678900", Email = "old@example.com" };

        _usuarioRepositoryMock.Setup(repo => repo.BuscarUmAsync(It.IsAny<Expression<Func<Usuario, bool>>>(), It.IsAny<Expression<Func<Usuario, object>>[]>()))
            .ReturnsAsync(existingUser);

        // Act
        await _usuarioService.Atualizar(userViewModel);

        // Assert
        _usuarioRepositoryMock.Verify(repo => repo.AtualizarAsync(It.Is<Usuario>(u => u.Email == "test@example.com")), Times.Once);
        _redisRepositoryMock.Verify(repo => repo.RemoveAsync(It.IsAny<string>()), Times.Exactly(2));
    }

    [Fact]
    public async Task DeletarAsync_ShouldMarkUserAsDeleted()
    {
        // Arrange
        var userId = 1;
        var existingUser = new Usuario { Id = userId, Status = StatusEntityEnum.Ativo };

        _usuarioRepositoryMock.Setup(repo => repo.BuscarUmAsync(It.IsAny<Expression<Func<Usuario, bool>>>())).ReturnsAsync(existingUser);

        // Act
        await _usuarioService.DeletarAsync(userId);

        // Assert
        _usuarioRepositoryMock.Verify(repo => repo.AtualizarAsync(It.Is<Usuario>(u => u.Status == StatusEntityEnum.Deletado)), Times.Once);
    }

    // Additional tests for other methods can follow the same pattern.
}
