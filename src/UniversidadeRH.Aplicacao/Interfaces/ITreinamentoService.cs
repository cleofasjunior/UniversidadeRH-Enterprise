using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniversidadeRH.Aplicacao.Dtos;
using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Aplicacao.Interfaces
{
    public interface ITreinamentoService
    {
        // 1. Criar Curso
        Task CriarCursoAsync(string descricao, int nivel, int tipoFuncionario, int horas);

        // 2. Listar Cursos
        Task<List<Treinamento>> ListarTodosCursosAsync();

        // 3. Avaliar (Assinatura atualizada para usar o DTO)
        Task RegistrarAvaliacaoDesempenhoAsync(RegistrarAvaliacaoDto dto);

        // 4. Listar Avaliações
        Task<List<AvaliacaoResumoDto>> ListarAvaliacoesPorFuncionarioAsync(Guid funcionarioId);
    }
}