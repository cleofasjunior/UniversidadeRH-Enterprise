using FluentValidation;
using UniversidadeRH.Aplicacao.DTOs;
using UniversidadeRH.Aplicacao.Interfaces;
using UniversidadeRH.Dominio.Entidades;
using UniversidadeRH.Dominio.Enums; 
using UniversidadeRH.Dominio.Interfaces; 

namespace UniversidadeRH.Aplicacao.Servicos
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepositorio _repositorio;
        private readonly IValidator<Funcionario> _validator;

        public FuncionarioService(IFuncionarioRepositorio repositorio, IValidator<Funcionario> validator)
        {
            _repositorio = repositorio;
            _validator = validator;
        }

        public async Task<FuncionarioViewModel> RegistrarFuncionarioAsync(RegistrarFuncionarioDto dto)
        {
            // 1. Instanciar a Entidade (Construtor Básico)
            var funcionario = new Funcionario(
                dto.Nome, 
                dto.Email, 
                dto.Cpf, 
                dto.Departamento, 
                (TipoFuncionario)dto.TipoFuncionario, 
                dto.LinkLattes
            );

            // === AQUI ESTÁ O AJUSTE SOLICITADO ===
            
            // 2. Configurar Regime de Trabalho (Se informado no DTO)
            if (dto.Regime > 0)
            {
                // Converte int (DTO) para Enum (Dominio)
                funcionario.DefinirRegime((RegimeTrabalho)dto.Regime);
            }

            // 3. Configurar Data de Admissão (Se informada no DTO)
            // Útil para simular funcionários antigos nos testes de promoção
            if (dto.DataAdmissao.HasValue)
            {
                funcionario.AlterarDataAdmissao(dto.DataAdmissao.Value);
            }
            // ======================================

            // 4. Validação (FluentValidation)
            // Validamos AGORA, pois o objeto já está completo com Regime e Data
            var validacao = await _validator.ValidateAsync(funcionario);
            
            if (!validacao.IsValid)
            {
                var erros = string.Join(";", validacao.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(erros);
            }

            // 5. Persistência
            await _repositorio.AdicionarAsync(funcionario);

            // 6. Retorno do ViewModel
            return new FuncionarioViewModel(
                funcionario.Id,
                funcionario.Nome,
                funcionario.Email,
                funcionario.Departamento,
                funcionario.Tipo.ToString(),
                funcionario.LinkLattes,
                funcionario.DataAdmissao
            );
        }

        public async Task<FuncionarioViewModel?> ObterPorIdAsync(Guid id)
        {
            var funcionario = await _repositorio.ObterPorIdAsync(id);

            if (funcionario == null) return null;

            return new FuncionarioViewModel(
                funcionario.Id,
                funcionario.Nome,
                funcionario.Email,
                funcionario.Departamento,
                funcionario.Tipo.ToString(),
                funcionario.LinkLattes,
                funcionario.DataAdmissao
            );
        }

        public async Task<List<FuncionarioViewModel>> ListarTodosAsync()
        {
            var lista = await _repositorio.ListarTodosAsync();
            return lista.Select(f => new FuncionarioViewModel(
                f.Id,
                f.Nome,
                f.Email,
                f.Departamento,
                f.Tipo.ToString(),
                f.LinkLattes,
                f.DataAdmissao
            )).ToList();
        }
    }
}