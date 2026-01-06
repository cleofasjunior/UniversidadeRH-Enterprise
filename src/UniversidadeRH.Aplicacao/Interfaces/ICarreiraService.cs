using UniversidadeRH.Aplicacao.DTOs;

namespace UniversidadeRH.Aplicacao.Interfaces;

public interface ICarreiraService
{
    // Docentes
    Task RegistrarAtividadeAcademicaAsync(RegistrarAtividadeAcademicaDto dto);
    Task AdicionarPontuacaoAcademicaAsync(RegistrarProducaoAcademicaDto dto);
    
    // Técnicos
    Task ProcessarPromocaoTecnicaAsync(ProcessarPromocaoTecnicaDto dto);
}