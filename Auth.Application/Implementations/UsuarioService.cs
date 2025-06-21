using System;
using System.Threading.Tasks;
using AutoMapper;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Models;
using Auth.Domain.ViewModels;
using Auth.Domain.Interfaces.Services;
using System.Linq;
using Auth.Domain.Enums;
using Auth.Service.Exceptions;
using Auth.Domain.Interfaces.Repositories;
using Auth.Domain.Interfaces.APIs;
using System.Collections.Generic;
using Auth.Domain.ViewModels.Usuario;
using Auth.Domain.Utils;
using Auth.Domain.ViewModels.Email;

namespace Auth.Service.Implementations;

public class UsuarioService(
        IUsuarioRepository _usuarioRepository,
        IRoutesAPI _routesAPI,
        IPessoasAPI _pessoasAPI,
        IUserContext _userContext,
        ITokenService _tokenService,
        IBaseRepository<Empresa> _empresaRepository,
        IBaseRepository<UsuarioPermissao> _usuarioPermissaoRepository,
        IPermissaoRepository _permissaoRepository,
        IRabbitMqRepository _rabbitMqRepository,
        IMapper _mapper) : IUsuarioService
{
    public async Task<PaginadoViewModel<UsuarioViewModel>> BuscarPaginadoAsync(int pagina, int tamanho)
    {
        var usuarios = await _usuarioRepository
            .BuscarPaginadoAsync(pagina, tamanho, x => x.EmpresaId == _userContext.Empresa);

        return _mapper.Map<PaginadoViewModel<UsuarioViewModel>>(usuarios);
    }

    public async Task<UsuarioViewModel> RegistrarAsync(UsuarioNovoViewModel user)
    {
        user.Senha = _tokenService.Base64ToString(user.Senha);
        await ValidarUsuarioExistente(user.CPF, user.EmpresaId);

        var model = await CriarUsuario(user, isMotorista: false);

        await _usuarioRepository.AdicionarAsync(model);
        EnviarEmailConfirmacaoAsync(model);

        return _mapper.Map<UsuarioViewModel>(model);
    }

    public async Task<UsuarioViewModel> RegistrarMotoristaAsync(UsuarioMotoristaNovoViewModel user)
    {
        user.Senha = _tokenService.Base64ToString(GenerateRandomPassword());
        await ValidarUsuarioExistente(user.CPF, user.EmpresaId);

        var model = await CriarUsuario(user, isMotorista: true);

        await _usuarioRepository.AdicionarAsync(model);
        EnviarEmailConfirmacaoAsync(model);

        return _mapper.Map<UsuarioViewModel>(model);
    }

    private async Task<Usuario> CriarUsuario(UsuarioBaseViewModel user, bool isMotorista)
    {
        var model = _mapper.Map<Usuario>(user);

        var empresa = await _empresaRepository.BuscarUmAsync(x => x.Id > 0);
        model.EmpresaId = empresa.Id;
        model.Status = StatusEntityEnum.Ativo;
        model.Senha = _usuarioRepository.ComputeHash(user.Senha);
        model.UsuarioValidado = true;
        model.EnderecoPrincipalId = null;

        if (isMotorista)
            model.Perfil = PerfilEnum.Motorista;

        var permissoes = await _permissaoRepository.ObterPermissoesPadraoPorEmpresaPerfilAsync(user.EmpresaId, isMotorista);
        await _usuarioPermissaoRepository.AdicionarAsync(permissoes.Select(x => new UsuarioPermissao
        {
            UsuarioId = 0, // Será atualizado pelo banco ao salvar
            PermissaoId = x.Id
        }));

        return model;
    }

    public async Task Atualizar(UsuarioAtualizarViewModel user)
    {
        var model = await _usuarioRepository.BuscarUmAsync(x => x.Id == user.Id);

        model.CPF = user.CPF;
        model.Email = user.Email;
        model.PrimeiroNome = user.PrimeiroNome;
        model.UltimoNome = user.UltimoNome;
        model.PlanoId = user.PlanoId;
        model.Contato = user.Contato;
        model.EnderecoPrincipalId = user.EnderecoPrincipalId;

        await _usuarioRepository.AtualizarAsync(model);
    }

    public async Task DeletarAsync(int userId)
    {
        var model = await _usuarioRepository.BuscarUmAsync(x => x.Id == userId);
        model.Status = StatusEntityEnum.Deletado;
        await _usuarioRepository.AtualizarAsync(model);
    }

    public async Task<UsuarioViewModel> ObterPorId(int userId, bool obterDadosMotorista = true, bool obterDadosEndereco = true)
    {
        var enderecoTask = obterDadosEndereco ? _routesAPI.ObterEnderecosAsync() : Task.FromResult(new BaseResponse<IEnumerable<EnderecoViewModel>>());
        var motoristaTask = obterDadosMotorista ? _pessoasAPI.ObterMotoristaPorUsuarioIdAsync(userId) : Task.FromResult(new BaseResponse<MotoristaViewModel>());
        var usuarioTask = _usuarioRepository.BuscarUmAsync(x => x.Id == userId);

        await Task.WhenAll(motoristaTask, enderecoTask, usuarioTask);

        var dto = _mapper.Map<UsuarioViewModel>(usuarioTask.Result);
        dto.Motorista = motoristaTask.Result.Data;
        dto.Enderecos = enderecoTask.Result.Data;
        return dto;
    }

    public async Task<UsuarioViewModel> ObterDadosDoUsuario()
    {
        var user = await _usuarioRepository.ObterPorIdAsync(_userContext.UserId);
        return _mapper.Map<UsuarioViewModel>(user);
    }

    public async Task VincularPermissao(PermissaoViewModel user)
    {
        var usuario = await _usuarioRepository.BuscarUmAsync(x => x.Id == user.UsuarioId);
        if (usuario is null)
            throw new Exception("Usuário já existente!");

        var permissoesDoUsuario = await _usuarioPermissaoRepository
            .BuscarAsync(x => x.UsuarioId == user.UsuarioId && user.PermissaoId.Contains(x.Id));

        System.Linq.Expressions.Expression<Func<Permissao, bool>> predicate =
            x => x.EmpresaId == _userContext.Empresa &&
                usuario.Perfil == PerfilEnum.Motorista ? x.PadraoMotorista :
                usuario.Perfil == PerfilEnum.Passageiro ? x.PadraoPassageiros :
                usuario.Perfil == PerfilEnum.Responsavel ? x.PadraoResponsavel :
                usuario.Perfil == PerfilEnum.Suporte ? x.PadraoSuporte : false;

        // Logica para pegar as permissoes padrão pra o perfil do usuário
        var permissoesPadroes = await _permissaoRepository.BuscarAsync(predicate);
        var permissoesPadraoQueNaoEstaoNaRequest = permissoesPadroes.Where(x => !user.PermissaoId.Contains(x.Id)).Select(x => x.Id);

        // Remove permissoes
        permissoesDoUsuario.ToList().ForEach(item => _usuarioPermissaoRepository.RemoverAsync(item).GetAwaiter().GetResult());

        // Logica para adicionar as permissoes novamente
        var usuarioPermissao = user.PermissaoId.Concat(permissoesPadraoQueNaoEstaoNaRequest).Select(x => new UsuarioPermissao
        {
            PermissaoId = x,
            UsuarioId = user.UsuarioId
        });

        await _usuarioPermissaoRepository.AdicionarAsync(usuarioPermissao);
    }

    private async Task ValidarUsuarioExistente(string cpf, int empresaId)
    {
        var usuarioExistente = await _usuarioRepository.BuscarPorCpfEmpresaAsync(cpf, empresaId);
        if (usuarioExistente != null && usuarioExistente.Id > 0)
            throw new BusinessRuleException("Usuário já cadastrado!!");
    }

    private void EnviarEmailConfirmacaoAsync(Usuario model)
    {
        var request = new
        {
            nome = model.PrimeiroNome + " " + model.UltimoNome ?? string.Empty,
            urlValidacao = "https://www.gateway.coopertrasmig.coop.br/Auth/v1/Token/Confirmar/Usuario/" + model.Id,
            urlCriarSenha = "https://app.coopertrasmig.coop.br/confirmar-senha?usuarioId=" + model.Id,
        };

        var emailRequest = new EmailRequest()
        {
            TipoEmail = model.Perfil == PerfilEnum.Motorista ? TipoEmailEnum.NovoMotorista : TipoEmailEnum.NovoResponsavel,
            Data = request.ToJson(),
            Destinos = new List<string> { model.Email },
            Assunto = "Confirmação de Cadastro"
        };

        _rabbitMqRepository.Publish(RabbitMqQueues.Email, emailRequest.NewQueue());
    }

    public async Task ConfirmarCadastroAsync(int userId)
    {
        var usuario = await _usuarioRepository.BuscarUmAsync(x => x.Id == userId);
        if (usuario is null)
            throw new BusinessRuleException($"Usuário não encontrado para o identificador '{userId}'!");

        if (usuario.UsuarioValidado == false)
        {
            usuario.UsuarioValidado = true;
            await _usuarioRepository.AtualizarAsync(usuario);
        }
    }

    private string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var password = new char[12];
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }
        return new string(password);
    }
}