using UniversidadeRH.Dominio.Entidades;

namespace UniversidadeRH.Dominio.Interfaces;

public interface IFuncionarioRepositorio
{
    // Métodos Básicos
    Task AdicionarAsync(Funcionario funcionario);
    Task AtualizarAsync(Funcionario funcionario); // Necessário para salvar alterações
    Task<Funcionario?> ObterPorEmailAsync(string email);
    Task<Funcionario?> ObterPorIdAsync(Guid id);

    Task<List<Funcionario>> ListarTodosAsync();

    // Métodos Otimizados (Com JOINs)
    
    // 1. Para Carreira Docente (Traz Atividades Academicas)
    Task<Funcionario?> ObterPorIdComAtividadesAsync(Guid id);
    
    // 2. Para Carreira Técnica (Traz Avaliações para calcular média)
    Task<Funcionario?> ObterPorIdComAvaliacoesAsync(Guid id);
    
    // 3. Para Treinamentos (Traz Histórico de Treinamentos e Avaliações)
    Task<Funcionario?> ObterPorIdComTreinamentosAsync(Guid id);
}