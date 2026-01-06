namespace UniversidadeRH.Aplicacao.DTOs;
public record SolicitarFeriasDto(Guid FuncionarioId, DateTime DataInicio, int Dias);