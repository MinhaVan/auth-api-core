using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Auth.Domain.ViewModels.Rota;

namespace Auth.Domain.Interfaces.Services;

public interface IEnderecoService
{
    Task AdicionarAsync(EnderecoAdicionarViewModel enderecoAdicionarViewModel);
    Task AtualizarAsync(EnderecoAtualizarViewModel enderecoAdicionarViewModel);
    Task DeletarAsync(int id);
    Task<EnderecoViewModel> Obter(int id);
    Task<List<EnderecoViewModel>> Obter();
    Task<Marcador> ObterMarcadorAsync(string endereco);
}