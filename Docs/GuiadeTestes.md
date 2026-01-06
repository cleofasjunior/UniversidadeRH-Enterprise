# 🧪 Guia de Testes de Aceitação e Arquitetura

Este documento orienta a validação funcional e técnica do **Universidade RH Enterprise**.

Diferente de testes simples de "CRUD", aqui validaremos **Regras de Negócio Complexas** (Promoção e Regime de Trabalho) e a **Resiliência Arquitetural** (correção de problemas de concorrência do EF Core).

**Pré-requisitos:**
1.  Aplicação rodando (`dotnet run` em `src/UniversidadeRH.API`).
2.  Swagger aberto: `http://localhost:5114/swagger`.

---

## 📝 Cenário 1: Gestão de Carreira Docente (Professores)

**Objetivo:** Validar se o sistema respeita o **Regime de Trabalho** (Dedicação Exclusiva) e calcula corretamente a evolução de nível.

### Passo 1: Contratar Professor com Dedicação Exclusiva (DE)
1.  Expanda `POST /api/Funcionarios`.
2.  JSON de Entrada:
    ```json
    {
      "nome": "Prof. Dr. Estranho",
      "email": "estranho@universidade.edu",
      "cpf": "123.456.789-00",
      "departamento": "Ciências Místicas",
      "tipoFuncionario": 1,
      "linkLattes": "[http://lattes.cnpq.br/1234](http://lattes.cnpq.br/1234)",
      "regime": 3,
      "dataAdmissao": "2024-01-01"
    }
    ```
    > **Nota:** `regime: 3` significa Dedicação Exclusiva (Limite de 40h para atividades).
3.  **⚠️ Importante:** Copie o **`id`** (GUID) retornado.

### Passo 2: Registrar Produção Acadêmica (Teste de Limite)
1.  Expanda `POST /api/carreira/docente/atividade`.
2.  Tente adicionar uma atividade válida (10h):
    ```json
    {
      "funcionarioId": "COLE_O_GUID_AQUI",
      "descricao": "Orientação de Mestrado",
      "tipoId": 1,
      "horasSemanais": 10
    }
    ```
3.  **Validação:** Deve retornar `200 OK`.
4.  *(Opcional)* Tente adicionar uma atividade de **50 horas**. O sistema deve retornar `400 Bad Request` informando que excede o regime.

### Passo 3: Adicionar Pontuação e Verificar Nível
1.  Expanda `POST /api/carreira/docente/pontuacao`.
2.  Adicione 150 pontos para promover de Auxiliar -> Assistente.
    ```json
    {
      "funcionarioId": "COLE_O_GUID_AQUI",
      "descricao": "Publicação de Livro Técnico",
      "pontos": 150
    }
    ```
3.  **Validação:** O retorno deve ser `200 OK` com a mensagem: *"Pontos computados. Nível de carreira verificado."*. Isso confirma que o método `AvaliarPromocaoAcademica()` rodou no domínio.

---

## 🛡️ Cenário 2: Gestão de Carreira Técnica (Administrativos)

**Objetivo:** Validar o motor de promoção automático que cruza **Tempo** (Interstício) e **Mérito** (Avaliação).

### Passo 1: Cadastrar Técnico "Veterano"
Simularemos alguém admitido em 2021 para ter o tempo necessário (> 2 anos).
1.  Expanda `POST /api/Funcionarios`.
    ```json
    {
      "nome": "Tony Stark (Técnico)",
      "email": "tony@infra.edu",
      "cpf": "999.888.777-66",
      "departamento": "Manutenção",
      "tipoFuncionario": 2,
      "dataAdmissao": "2021-01-01"
    }
    ```
2.  Copie o **GUID**.

### Passo 2: Avaliar Desempenho (Mérito)
O técnico precisa de nota média >= 7.0.
1.  Expanda `POST /api/Treinamentos/avaliacao-desempenho`.
    ```json
    {
      "funcionarioId": "COLE_O_GUID_AQUI",
      "nota": 9.5,
      "feedback": "Excelente desempenho na manutenção."
    }
    ```

### Passo 3: Processar Promoção
1.  Expanda `POST /api/carreira/tecnico/processar-promocao`.
    ```json
    {
      "funcionarioId": "COLE_O_GUID_AQUI",
      "motivo": "Solicitação via sistema"
    }
    ```
2.  **Validação:** Deve retornar `200 OK` -> *"Análise de promoção concluída com sucesso"*.
    > *Se você tentar fazer isso com um funcionário novo (2024), receberá um erro informando falta de interstício.*

---

## 🔧 Cenário 3: Teste de Robustez (Concorrência EF Core)

**Objetivo:** Garantir que o sistema não falha ao salvar dados complexos (o erro `DbUpdateConcurrencyException` que corrigimos).

1.  Use o mesmo ID do Professor do Cenário 1.
2.  Vá novamente em `POST /api/carreira/docente/atividade`.
3.  Adicione uma **Segunda Atividade** diferente (ex: "Aula na Graduação", 4 horas).
4.  Execute.
5.  **Resultado Esperado:** `200 OK`.
    > **Por que esse teste é importante?** Antes da correção no Repositório (`Force Insert`), essa segunda inserção falhava com erro 500, pois o EF Core se perdia com o ID gerado no cliente. O sucesso aqui prova a robustez da Camada de Infraestrutura.

---

## 👁️ Cenário 4: Observabilidade (Logs)

Verifique o console da aplicação (`dotnet run`). Você deve ver logs estruturados como:

> `[INF] Executing endpoint 'UniversidadeRH.API.Controllers.CarreiraController.AdicionarAtividadeDocente'`
> `[INF] Entity Framework Core ... Executed DbCommand (3ms) ... INSERT INTO [AtividadesAcademicas] ...`

Isso confirma que o sistema está auditando as operações de carreira.

---

**Responsável Técnico:** Prof. Dr. Cleófas Júnior