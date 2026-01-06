using System.Collections.Generic;
using System.Threading.Tasks;
using UniversidadeRH.Dominio.Entidades; 
using UniversidadeRH.Dominio.Enums;

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface ITreinamentoRepositorio
    {
        // CRUD Básico
        Task AdicionarAsync(Treinamento treinamento);
        
        // Padronizamos este nome (usado nos testes e no repositório):
        Task<List<Treinamento>> ObterTodosAsync();

        // Métodos Específicos
        Task<List<FuncionarioTreinamento>> ObterPendentesPorFuncionarioAsync(Guid funcionarioId);
        
        Task<List<Treinamento>> ListarPorNivelETipoAsync(int nivel, TipoFuncionario tipo);
    }
}