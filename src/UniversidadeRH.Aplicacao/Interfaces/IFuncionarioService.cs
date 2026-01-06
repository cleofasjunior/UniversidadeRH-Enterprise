using UniversidadeRH.Aplicacao.DTOs;


namespace UniversidadeRH.Aplicacao.Interfaces
{
    public interface IFuncionarioService
    {
       
        Task<FuncionarioViewModel> RegistrarFuncionarioAsync(RegistrarFuncionarioDto dto);
        
        Task<FuncionarioViewModel?> ObterPorIdAsync(Guid id);
        
        Task<List<FuncionarioViewModel>> ListarTodosAsync();
    }
}