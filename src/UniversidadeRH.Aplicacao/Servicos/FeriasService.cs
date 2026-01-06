using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Interfaces;

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class FeriasService : IFeriasService
    {
        private readonly IFeriasRepositorio _repositorio;

        public FeriasService(IFeriasRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Ferias> SolicitarFeriasAsync(Guid funcionarioId, DateTime inicio, DateTime fim)
        {
            // 1. Validação básica de coerência
            if (fim <= inicio)
                throw new ArgumentException("A data final deve ser posterior à data inicial.");

            // 2. Calcular a diferença em dias
            TimeSpan diferenca = fim.Date - inicio.Date;
            int dias = (int)diferenca.TotalDays;

            // 3. Agora passamos um INT (dias) para o construtor, como a Entidade exige
            // Se "dias" for menor que 5 ou maior que 30, a Entidade vai lançar o DomainException aqui
            var ferias = new Ferias(funcionarioId, inicio, dias); 
            
            await _repositorio.AdicionarAsync(ferias);
            return ferias;
        }

        public async Task<List<Ferias>> ConsultarFeriasDoFuncionarioAsync(Guid funcionarioId)
        {
            return await _repositorio.ObterPorFuncionarioAsync(funcionarioId);
        }
    }
}