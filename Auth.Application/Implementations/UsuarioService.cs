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

namespace Auth.Service.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IEmailService _emailService;
    private readonly IRoutesAPI _routesAPI;
    private readonly IPessoasAPI _pessoasAPI;
    private readonly IMapper _mapper;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IBaseRepository<Empresa> _empresaRepository;
    private readonly IBaseRepository<UsuarioPermissao> _usuarioPermissaoRepository;
    private readonly IPermissaoRepository _permissaoRepository;
    private readonly IUserContext _userContext;
    private readonly ITokenService _tokenService;
    public UsuarioService(
        IUsuarioRepository repo,
        IRoutesAPI routesAPI,
        IPessoasAPI pessoasAPI,
        IUserContext userContext,
        ITokenService ServiceToken,
        IEmailService emailService,
        IBaseRepository<Empresa> empresaRepository,
        IBaseRepository<UsuarioPermissao> usuarioPermissaoRepository,
        IPermissaoRepository permissaoRepository,
        IMapper map)
    {
        _emailService = emailService;
        _permissaoRepository = permissaoRepository;
        _empresaRepository = empresaRepository;
        _usuarioPermissaoRepository = usuarioPermissaoRepository;
        _userContext = userContext;
        _mapper = map;
        _routesAPI = routesAPI;
        _pessoasAPI = pessoasAPI;
        _usuarioRepository = repo;
        _tokenService = ServiceToken;
    }

    public async Task<PaginadoViewModel<UsuarioViewModel>> BuscarPaginadoAsync(int pagina, int tamanho)
    {
        var usuarios = await _usuarioRepository
            .BuscarPaginadoAsync(pagina, tamanho, x => x.EmpresaId == _userContext.Empresa);

        return _mapper.Map<PaginadoViewModel<UsuarioViewModel>>(usuarios);
    }

    public async Task<UsuarioViewModel> Registrar(UsuarioNovoViewModel user)
    {
        var model = _mapper.Map<Usuario>(user);
        user.Senha = _tokenService.Base64ToString(user.Senha);

        var usuario = await _usuarioRepository.BuscarPorCpfEmpresaAsync(user.CPF, user.EmpresaId);
        if (usuario != null && usuario.Id > 0)
            throw new BusinessRuleException("Usuário já cadastrado!!");

        var permissaoPadroes = await _permissaoRepository
            .ObterPermissoesPadraoPorEmpresaPerfilAsync(user.EmpresaId, user.IsMotorista);

        if (user.IsMotorista)
        {
            model.Perfil = PerfilEnum.Motorista;
        }

        await _usuarioPermissaoRepository.AdicionarAsync(
            permissaoPadroes.Select(x => new UsuarioPermissao
            {
                UsuarioId = usuario.Id,
                PermissaoId = x.Id
            })
        );

        model.EmpresaId = (await _empresaRepository.BuscarUmAsync(x => x.Id > 0)).Id;
        model.Status = StatusEntityEnum.Ativo;
        model.Senha = _usuarioRepository.ComputeHash(user.Senha);
        model.UsuarioValidado = true;
        model.EnderecoPrincipalId = null;

        await _usuarioRepository.AdicionarAsync(model);
        if (user.IsMotorista)
        {
            var motorista = new MotoristaViewModel
            {
                UsuarioId = model.Id,
                Vencimento = DateTime.MaxValue,
                TipoCNH = TipoCNHEnum.Nenhum,
                CNH = string.Empty
            };
            await _pessoasAPI.AdicionarMotoristaAsync(motorista);
        }

        await EnviarEmailConfirmacaoAsync(model);

        return _mapper.Map<UsuarioViewModel>(model);
    }

    public async Task Atualizar(UsuarioAtualizarViewModel user)
    {
        var model = await _usuarioRepository.BuscarUmAsync(x => x.Id == user.Id);
        var isMotorista = user.Perfil == PerfilEnum.Motorista;

        // var chaveLogin = string.Format(Cache.ChaveLogin, model.CPF, model.Email, model.Senha, model.EmpresaId, isMotorista);
        // var chaveBuscarPorCpfEmpresa = string.Format(Cache.ChaveBuscarPorCpfEmpresa, model.CPF, model.EmpresaId);
        // await Task.WhenAll(
        //     _redisRepository.RemoveAsync(chaveLogin),
        //     _redisRepository.RemoveAsync(chaveBuscarPorCpfEmpresa)
        // );

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

    private async Task EnviarEmailConfirmacaoAsync(Usuario model)
    {
        return;

        var now = DateTime.Now;
        var linkDeConfirmacao = "https://www.gateway.coopertrasmig.coop.br/Auth/v1/Token/Confirmar/" + model.Id;
        var titulo = "Confirmação de Cadastro";
        var mensagem = $@"
            <!DOCTYPE html>
            <html lang='pt-br'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Confirmação de Cadastro</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        color: #333;
                        padding: 20px;
                    }}
                    .container {{
                        background-color: #fff;
                        max-width: 600px;
                        margin: 0 auto;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                    }}
                    h1 {{
                        color: #007bff;
                    }}
                    p {{
                        font-size: 16px;
                    }}
                    .button {{
                        background-color: #007bff;
                        color: white;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 5px;
                        font-size: 16px;
                        display: inline-block;
                        margin-top: 20px;
                    }}
                    .footer {{
                        text-align: center;
                        font-size: 14px;
                        color: #777;
                        margin-top: 20px;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <h1>Bem-vindo ao Coopertrasmig!</h1>
                    <p>Olá, {model.ObterNomeInteiro()}!</p>
                    <p>Obrigado por se cadastrar no Coopertrasmig. Para concluir seu cadastro, por favor, confirme seu endereço de e-mail clicando no botão abaixo:</p>
                    
                    <p><a href='{linkDeConfirmacao}' class='button'>Confirmar E-mail</a></p>
                    
                    <p>Se você não se cadastrou no nosso site, por favor, ignore este e-mail.</p>
                    
                    <div class='footer'>
                        <p>&copy; {now.Year} Coopertrasmig. Todos os direitos reservados.</p>
                    </div>
                </div>
            </body>
            </html>";

        await _emailService.SendEmailAsync(model.Email, titulo, mensagem);
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
}