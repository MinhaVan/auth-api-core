using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Auth.Domain.Interfaces.APIs;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Models;
using Auth.Domain.Utils;
using Auth.Domain.ViewModels;
using Microsoft.Extensions.Logging;

namespace Routes.Data.APIs;

public class RoutesAPI : IRoutesAPI
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RoutesAPI> _logger;
    private readonly IUserContext _context;

    public RoutesAPI(IHttpClientFactory httpClientFactory, IUserContext context, ILogger<RoutesAPI> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("api-routes");
        _context = context;
    }

    public async Task<BaseResponse<IEnumerable<EnderecoViewModel>>> ObterEnderecosAsync()
    {
        _logger.LogInformation($"Enviando requisição para obter dados do endereco.");
        _httpClient.DefaultRequestHeaders.Add("Authorization", _context.Token);
        var response = await _httpClient.GetAsync($"v1/Endereco");

        if (response.IsSuccessStatusCode)
        {
            var enderecos = await response.Content.ReadFromJsonAsync<BaseResponse<IEnumerable<EnderecoViewModel>>>();
            _logger.LogInformation($"Resposta da requisição para obter os enderecos - Dados: {enderecos.ToJson()}");
            return enderecos;
        }
        else
        {
            var mensagemErro = await response.Content.ReadAsStringAsync();
            _logger.LogError($"Erro ao obter os enderecos - Mensagem: {mensagemErro}");
            throw new Exception("Ocorreu um erro ao tentar obter os enderecos!");
        }
    }
}