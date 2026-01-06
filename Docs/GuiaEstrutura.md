# 🏗️ Guia de Arquitetura e Estrutura

Este documento detalha a engenharia por trás do **Universidade RH Enterprise**, desenhada sob os princípios estritos da **Clean Architecture** e **Domain-Driven Design (DDD)**.

O objetivo desta estrutura não é apenas "organizar pastas", mas proteger o núcleo do sistema (Regras de Negócio de Carreira e RH) de oscilações tecnológicas externas, garantindo um software testável, agnóstico ao banco de dados e preparado para escalabilidade.

---

## 🧅 Diagrama de Dependências (Onion Architecture)

O diagrama abaixo ilustra o fluxo de dependência. A regra de ouro é: **As dependências apontam sempre para dentro.** O Domínio não conhece ninguém; a Infraestrutura conhece todo mundo.

```mermaid
graph TD
    subgraph Core [Núcleo Protegido]
        Dom[Layer: Domínio <br/> (Regras de Carreira e Entidades)]
        App[Layer: Aplicação <br/> (Casos de Uso e Orquestração)]
    end

    subgraph External [Camadas de Suporte]
        Infra[Layer: Infraestrutura <br/> (EF Core e Repositórios)]
        API[Layer: Apresentação/API <br/> (Controllers e Swagger)]
    end
    
    subgraph Quality [Garantia de Qualidade]
        Tests[Layer: Testes Automatizados <br/> (xUnit + Moq)]
    end

    API --> App
    API --> Infra
    Infra --> Dom
    Infra --> App
    App --> Dom
    Tests --> App
    Tests --> Dom
    
    style Dom fill:#f9f,stroke:#333,stroke-width:2px,color:black
    style App fill:#bbf,stroke:#333,stroke-width:2px,color:black
    style Infra fill:#dfd,stroke:#333,stroke-width:2px,color:black
    style API fill:#fdd,stroke:#333,stroke-width:2px,color:black
    style Tests fill:#ffd700,stroke:#333,stroke-width:2px,color:black

```

---

## 📂 Anatomia das Camadas

### 1. `src/UniversidadeRH.Dominio` (O Coração)

Esta é a camada soberana. Ela ignora completamente a existência de banco de dados, APIs ou frameworks externos.

* **Responsabilidade:** Definir a verdade do negócio. Se uma regra muda aqui, o sistema inteiro deve se adaptar.
* **Componentes Chave:**
* **Entidades Ricas:** Classes como `Funcionario` que não são apenas dados, mas possuem métodos de negócio (`DefinirRegime`, `AdicionarAtividadeAcademica`, `TentarPromocao`).
* **Enums Estratégicos:** `RegimeTrabalho`, `NivelCarreira`, `TipoFuncionario`.
* **Interfaces de Repositório:** Contratos (`IFuncionarioRepositorio`) que dizem *o que* precisa ser salvo, mas não *como*.
* **Exceções de Domínio:** `DomainException` para bloquear estados inválidos (ex: Professor 40h tentando pegar 60h de aulas).



### 2. `src/UniversidadeRH.Aplicacao` (O Maestro)

Camada responsável por orquestrar os casos de uso e traduzir o mundo externo para o domínio.

* **Responsabilidade:** Receber DTOs, validar dados, chamar o Domínio e persistir as alterações.
* **Componentes Chave:**
* **Serviços de Aplicação:** `FuncionarioService`, `CarreiraService`.
* **DTOs (Data Transfer Objects):** Objetos simples (`RegistrarAtividadeDto`, `ProcessarPromocaoDto`) que blindam o domínio.
* **Validadores (FluentValidation):** Implementação do padrão *Fail Fast*. Rejeita dados ruins antes de chamar o domínio.



### 3. `src/UniversidadeRH.Infraestrutura` (O Motor Técnico)

Aqui residem as implementações concretas que fazem o sistema "funcionar" no mundo físico.

* **Responsabilidade:** Persistência de dados, mapeamento ORM e comunicação com serviços externos.
* **Destaque Técnico (Concorrência):**
* Implementação customizada no `FuncionarioRepositorio` para lidar com **Grafos de Objetos** (Funcionario -> Lista de Atividades).
* Utilização de estratégia **"Force Insert"** para detectar e salvar corretamente entidades filhas geradas com GUIDs no cliente, evitando erros de concorrência (`DbUpdateConcurrencyException`) do EF Core.


* **Mapeamento:** `EntityTypeConfiguration` para definir chaves, índices e relacionamentos no SQL Server.

### 4. `src/UniversidadeRH.API` (A Porta de Entrada)

A interface RESTful organizada em **7 Módulos Funcionais**.

* **Responsabilidade:** Expor os endpoints, gerenciar autenticação (JWT) e tratar erros globais.
* **Estrutura:** Controllers separados por contexto (`Auth`, `Carreira`, `Beneficios`, `Treinamentos`).
* **Middlewares:**
* `GlobalExceptionHandler`: Implementa a RFC 7807 (Problem Details) para padronizar erros.
* `Serilog`: Logging estruturado de todas as operações.



### 5. `tests/` (A Rede de Segurança)

A estratégia de testes foi refinada para garantir velocidade e confiança.

#### 🧪 `UniversidadeRH.Testes.Unidade`

Focado na validação de regras de negócio puras, sem tocar no banco de dados.

* **Uso de Mocks (Moq):** Simulamos o comportamento do repositório para testar cenários complexos (ex: "O que acontece se o banco falhar ao salvar uma promoção?").
* **Cobertura Crítica:**
* `FuncionarioDomainTests.cs`: Valida se a lógica de pontos para docentes e interstício para técnicos está funcionando.
* `CarreiraServiceTests.cs`: Garante que o serviço só processa promoções elegíveis.
* `ValidatorTests.cs`: Garante que dados inválidos são barrados na entrada.



---

## 🧠 Decisões Arquiteturais Importantes

1. **Polimorfismo no Domínio:** Em vez de espalhar `if (tipo == Professor)` pelo código, encapsulamos as regras específicas dentro da Entidade, mantendo o código limpo e extensível.
2. **Identidade no Cliente vs. Banco:** Optamos por gerar GUIDs no construtor da Entidade (DDD Puro). Isso exigiu uma adaptação robusta na Infraestrutura para que o EF Core compreendesse o ciclo de vida dos objetos (transição de *Detached* para *Added*).
3. **Segregação de Interfaces:** Criamos DTOs específicos para cada operação (`RegistrarAtividade`, `SolicitarFerias`), evitando o vazamento de dados desnecessários.

---

## 👨‍💻 Autoria

**Prof. Dr. Cleófas Júnior**
*.NET Backend Developer | Especialista em Arquitetura de Software*

Este guia reflete a maturidade técnica do projeto, demonstrando como padrões de design resolvem problemas reais de manutenibilidade e evolução de software.


