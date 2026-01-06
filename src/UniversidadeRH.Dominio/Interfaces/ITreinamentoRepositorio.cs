using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums; 

namespace UniversidadeRH.Dominio.Interfaces
{
    public interface ITreinamentoRepositorio
    {
        // Métodos que estavam faltando:
        Task AdicionarAsync(Treinamento treinamento);
        Task<List<Treinamento>> ListarTodosAsync();

        // Métodos que provavelmente já existiam (mantenha-os):
        Task<List<FuncionarioTreinamento>> ObterPendentesPorFuncionarioAsync(Guid funcionarioId);
        
        // Se houver outros métodos antigos como ListarPorNivelETipoAsync, mantenha-os aqui também.
        Task<List<Treinamento>> ListarPorNivelETipoAsync(int nivel, TipoFuncionario tipo);
    }
}