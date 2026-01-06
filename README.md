# 🏛️ Universidade RH - Sistema Inteligente de Gestão de Talentos

![.NET Version](https://img.shields.io/badge/.NET-9.0-purple?style=for-the-badge&logo=dotnet)
![Architecture](https://img.shields.io/badge/Architecture-Clean%20Arch%20%7C%20DDD-orange?style=for-the-badge)
![Security](https://img.shields.io/badge/Security-JWT%20Bearer-red?style=for-the-badge)
![Tests](https://img.shields.io/badge/Tests-xUnit%20%7C%20Moq-blue?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

## 📄 Visão Geral do Projeto

O **Universidade RH Enterprise** é um ecossistema backend de alta fidelidade desenvolvido em **.NET 9**, projetado com um objetivo claro: **resolver a maior dor administrativa de qualquer universidade** — a gestão fragmentada, manual e burocrática da vida funcional de seus servidores.

Diferente de sistemas de RH genéricos, esta solução foi arquitetada para suportar a **complexidade legislativa e estatutária** do ensino superior, orquestrando ciclos de vida distintos para **Docentes** (Professores) e **Técnicos Administrativos**. O projeto demonstra consistência arquitetural ao integrar 7 módulos distintos em uma única API robusta e escalável.

---

## 🎯 O Desafio de Negócio (A Dor Real)

A gestão de servidores públicos envolve regras rígidas que sistemas de prateleira (SaaS) raramente atendem:

* **O Problema:** O cálculo de progressão de carreira de um professor (baseado em produção acadêmica, publicações e regime de trabalho) é totalmente diferente da promoção de um técnico (baseada em tempo de serviço e avaliação de desempenho). Gerenciar isso manualmente gera erros de folha, retrabalho e passivos trabalhistas.
* **A Solução:** Um motor de regras de domínio encapsulado (`Core Domain`), que aplica **Polimorfismo** para validar automaticamente requisitos de promoção, férias e benefícios, garantindo Compliance e integridade de dados.

---

## 🧩 Módulos da Aplicação (Enterprise Capabilities)

A API segue a especificação OpenAPI 3.0 e está segmentada em **Bounded Contexts** (Contextos Delimitados), cobrindo toda a jornada do servidor:

### 🔐 00. Autenticação e Segurança
* Gestão de identidade e acesso (IAM).
* Autenticação via **JWT (JSON Web Token)** para proteção de endpoints críticos.

### 👥 01. Gestão de Funcionários (Core)
* Cadastro unificado com distinção de perfis (Docente/Técnico).
* Validação de regras imutáveis (CPF, E-mail Institucional e Vínculos).

### 🎁 02. Módulo de Benefícios
* Catálogo dinâmico de benefícios (Vale Transporte, Saúde, Alimentação).
* Regras de elegibilidade automáticas para vinculação ao colaborador.

### 🏖️ 03. Módulo de Férias
* Cálculo automático de **Período Aquisitivo**.
* O sistema bloqueia solicitações caso o servidor não tenha completado 1 ano de exercício.

### 📈 04. Gestão de Carreira e Cargos (Destaque)
O coração da inteligência do sistema, tratando as duas carreiras de forma distinta:

* **Docentes (Professores):**
    * **Regime de Trabalho:** Validação estrita de carga horária (20h, 40h ou Dedicação Exclusiva).
    * **Produção Acadêmica:** Registro de atividades (`/api/carreira/docente/atividade`) com controle de teto de horas.
    * **Pontuação:** Endpoint exclusivo para cômputo de pontos e mudança de nível (`/api/carreira/docente/pontuacao`).

* **Técnicos Administrativos:**
    * **Promoção Automática:** Endpoint inteligente (`/api/carreira/tecnico/processar-promocao`) que cruza dados de **Interstício** (Tempo de Casa) com a média da **Avaliação de Desempenho**. A promoção só ocorre se ambos os critérios forem atendidos.

### 🏥 05. Saúde e Segurança do Trabalho
* **Gestão de Afastamentos:** Registro seguro de atestados médicos (`/api/atestados`) para controle de absenteísmo e suporte ao servidor.
* **Histórico de Saúde:** Consulta de histórico de licenças por funcionário.

### 📚 06. Desenvolvimento e Treinamento (LMS)
* **Catálogo de Cursos:** Criação e gestão de treinamentos institucionais (`/api/Treinamentos/criar-curso`).
* **Avaliação de Desempenho:** Registro de notas e feedbacks qualitativos (`/api/Treinamentos/avaliacao-desempenho`), dados estes que alimentam diretamente o motor de promoção dos técnicos.

---

## 🛡️ Engenharia e Robustez Técnica

Este projeto demonstra domínio sobre cenários avançados de desenvolvimento backend:

### 1. Solução de Concorrência Otimista (EF Core)
Implementação de um mecanismo customizado no Repositório para evitar o erro `DbUpdateConcurrencyException`. O sistema detecta se grafos de objetos complexos (como a lista de Atividades de um Professor ou Avaliações) são novos ou existentes, aplicando uma estratégia de **"Force Insert"** para IDs gerados no cliente, garantindo a persistência correta sem conflitos.

### 2. Arquitetura Limpa (Onion Architecture)
Isolamento total das regras de negócio. A camada de `Dominio` não conhece o banco de dados nem a API.
* **DTOs (Data Transfer Objects):** Blindam o domínio de dados externos.
* **ViewModels:** Otimizam o retorno de dados para o frontend.

### 3. Fail Fast com FluentValidation
Validação defensiva na entrada da API. Requisições com dados inconsistentes (ex: datas futuras inválidas, cargas horárias negativas ou notas fora do range 0-10) são rejeitadas imediatamente com **HTTP 400**, economizando recursos de processamento.

### 4. Tratamento de Erros (RFC 7807)
Middleware global de exceções que padroniza os retornos de erro usando a especificação **Problem Details**, garantindo interoperabilidade e facilidade de debug para clientes da API.

---

## 🚀 Stack Tecnológica

* **Linguagem:** C# 13 (.NET 9)
* **Banco de Dados:** SQL Server
* **ORM:** Entity Framework Core 9
* **Logs/Observabilidade:** Serilog
* **Testes:** xUnit, Moq, FluentAssertions
* **Doc:** Swagger / OpenAPI

---

## ⚙️ Como Executar

### Pré-requisitos
* .NET SDK 9.0
* SQL Server (LocalDB ou Container Docker)

### Passo a Passo

1.  **Clonar o repositório:**
    ```bash
    git clone [https://github.com/seu-usuario/UniversidadeRH.git](https://github.com/seu-usuario/UniversidadeRH.git)
    ```

2.  **Restaurar dependências:**
    ```bash
    dotnet restore
    ```

3.  **Executar a API:**
    ```bash
    cd src/UniversidadeRH.API
    dotnet run
    ```

4.  **Acessar o Swagger:**
    Navegue para `http://localhost:5114/swagger` para visualizar a documentação interativa de todos os módulos.

---

## 👨‍💻 Autoria e Desenvolvimento

**Prof. Dr. Cleófas Júnior**
*.NET Backend Developer | Doutor em Educação*

Este projeto demonstra a aplicação prática de padrões avançados de **Engenharia de Software** e **Arquitetura de Sistemas**. O objetivo foi construir uma solução que unisse o rigor analítico à excelência técnica, evidenciando domínio sobre **Clean Architecture**, **Testes Automatizados (Unitários e Mocks)** e **Integração Contínua (CI/CD)** para entregar software escalável e de alto valor agregado.