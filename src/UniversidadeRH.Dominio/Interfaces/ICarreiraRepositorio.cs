using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface ICarreiraRepositorio
    {
        Task AdicionarAtividadeAsync(AtividadeAcademica atividade);
        Task RegistrarPromocaoAsync(HistoricoPromocao promocao);
        Task<Funcionario?> ObterFuncionarioAsync(Guid id);
    }
}