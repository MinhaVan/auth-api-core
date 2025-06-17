using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.APIs;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Utils;
using Auth.Domain.ViewModels;
using Microsoft.Extensions.Logging;

namespace Auth.Data.APIs;

public class PessoasAPI : IPessoasAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PessoasAPI> _logger;
    private readonly IUserContext _context;

    public PessoasAPI(IHttpClientFactory httpClientFactory, IUserContext userContext, ILogger<PessoasAPI> logger)
    {
        _logger = logger;
        _context = userContext;
        _httpClient = httpClientFactory.CreateClient("api-pessoas");
    }

    public async Task<BaseResponse<MotoristaViewModel>> ObterMotoristaPorUsuarioIdAsync(int usuarioId)
    {
        _logger.LogInformation($"Enviando requisição para obter dados do motorista pelo usuarioId - Dados: {usuarioId}");
        _httpClient.DefaultRequestHeaders.Add("Authorization", _context.Token);
        var response = await _httpClient.GetAsync($"v1/Motorista/Usuario/{usuarioId}");

        if (response.IsSuccessStatusCode)
        {
            var motorista = await response.Content.ReadFromJsonAsync<BaseResponse<MotoristaViewModel>>();
            _logger.LogInformation($"Resposta da requisição para obter dados do motorista pelo usuarioId - Dados: {motorista.ToJson()}");
            return motorista;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter dados do motorista pelo usuarioId - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter dados do motorista pelo usuarioId!");
        }
    }
}