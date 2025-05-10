using System.Collections.Generic;
using System.Threading.Tasks;
using Auth.Domain.Models;
using Auth.Domain.ViewModels;

namespace Auth.Domain.Interfaces.APIs;

public interface IRoutesAPI
{
    Task<BaseResponse<IEnumerable<EnderecoViewModel>>> ObterEnderecosAsync();
}