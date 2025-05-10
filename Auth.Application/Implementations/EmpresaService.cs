using System.Threading.Tasks;
using AutoMapper;
using Auth.Domain.Interfaces.Repository;
using Auth.Domain.Models;
using Auth.Domain.Interfaces.Services;
using Auth.Domain.ViewModels;

namespace Auth.Service.Implementations;

public class EmpresaService : IEmpresaService
{
    private readonly IBaseRepository<Empresa> _empresaRepository;
    private readonly IMapper _mapper;
    public EmpresaService(
        IBaseRepository<Empresa> empresaRepository,
        IMapper mapper)
    {
        _empresaRepository = empresaRepository;
        _mapper = mapper;
    }

    public async Task<EmpresaViewModel> ObterPorIdAsync(int id)
    {
        var empresa = await _empresaRepository.BuscarUmAsync(x => x.Id == id);
        var viewModel = _mapper.Map<EmpresaViewModel>(empresa);
        return viewModel;
    }

    public async Task<EmpresaViewModel> CriarAsync(EmpresaAdicionarViewModel empresa)
    {
        var model = _mapper.Map<Empresa>(empresa);
        await _empresaRepository.AdicionarAsync(model);
        var viewModel = _mapper.Map<EmpresaViewModel>(model);
        return viewModel;
    }

    public async Task AtualizarAsync(int empresaId, EmpresaAdicionarViewModel empresa)
    {
        var model = _mapper.Map<Empresa>(empresa);
        model.Id = empresaId;
        await _empresaRepository.AtualizarAsync(model);
    }
}