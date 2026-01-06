# 🧠 Jornada de Aprendizado e Decisões Arquiteturais

> *"A tecnologia é a ferramenta, mas a arquitetura é o pensamento estruturado que a torna perene."*
>
> — **Prof. Dr. Cleófas Júnior**

Neste documento, narramos os desafios técnicos de "trincheira", as barreiras de persistência encontradas e as evoluções arquiteturais implementadas para transformar o **Universidade RH** em uma solução Enterprise.

---

## 1. O Desafio da Concorrência e Identidade (EF Core vs. GUIDs)

Talvez o maior desafio técnico enfrentado. Ao optarmos por gerar os IDs (Guid) no **Construtor da Entidade** (seguindo boas práticas de DDD para garantir que o objeto já nasça com identidade), criamos um conflito com o Entity Framework Core.

* **O Problema:** O EF Core, por padrão, assume que se uma entidade já tem ID, ela existe no banco e tenta fazer um `UPDATE`. Como o registro era novo, o banco retornava "0 linhas afetadas", disparando a exceção `DbUpdateConcurrencyException`.
* **A Solução (Pattern: Force Insert):**
    * Não abrimos mão do DDD (o ID continua sendo gerado no domínio).
    * Implementamos uma lógica inteligente no **Repositório**: antes de salvar, o sistema verifica grafos de objetos complexos (como Avaliações e Atividades). Se o objeto não existe no banco, forçamos o estado `EntityState.Added`.
    * **Lição:** O ORM deve servir ao Domínio, e não o contrário. Adaptamos a infraestrutura para respeitar a regra de negócio.

---

## 2. Do "Jeito que Funciona" para o "Jeito Profissional" (Estratégia de Testes)

Inicialmente, adotamos uma abordagem de **Testes de Integração** conectados diretamente ao banco de dados (`LocalDB`). Embora funcionais, eles eram lentos e frágeis.

* **A Evolução:** Migramos para **Testes de Unidade** com **Moq**.
* **O Ganho:** Passamos a testar a **Lógica Pura** (ex: "Um técnico com menos de 2 anos não pode ser promovido"). Isso nos deu feedback instantâneo (milissegundos) e permitiu validar cenários de borda sem precisar "sujar" o banco de dados real.

---

## 3. Domain-Driven Design (DDD) na Prática

Não criamos apenas tabelas; modelamos **Comportamentos**.

* **Entidades Ricas:** Abandonamos classes anêmicas. A entidade `Funcionario` possui métodos como `DefinirRegime()` e `AdicionarAtividadeAcademica()`.
* **Invariantes de Negócio:** O próprio método de adicionar atividade verifica se o professor estourou o teto de 40h. Isso impede que o sistema entre em estado inconsistente, independente da camada de apresentação.
* **Bounded Contexts (Contextos Delimitados):** Percebemos que "Carreira Docente" e "Carreira Técnica" eram mundos diferentes. Em vez de misturar tudo, separamos os endpoints e as regras, respeitando a realidade do negócio acadêmico.

---

## 4. API Enterprise e Organização Modular

Começamos com um controlador monolítico e evoluímos para uma estrutura segmentada baseada em módulos funcionais (Swagger/OpenAPI):

1.  **Core:** Gestão de Pessoas.
2.  **Carreira:** Regras de promoção.
3.  **LMS:** Treinamento.
4.  **Benefícios/Saúde:** Módulos de apoio.

Essa separação facilita a manutenção e permitiria, no futuro, quebrar essa API em **Microserviços** com baixo esforço, pois os contextos já estão logicamente isolados.

---

## 5. Integração Contínua (CI/CD)

Para garantir a "Saúde do Repositório", implementamos um pipeline no **GitHub Actions**.

* **O Fluxo:** A cada *push*, o sistema restaura pacotes, compila o projeto e roda a bateria de testes.
* **O Valor:** Isso elimina o famoso "na minha máquina funciona". Se o código não compila ou quebra um teste unitário, ele é rejeitado antes de chegar à produção.

---

## 6. Gerenciamento de Dependências

Aprendemos a importância da reprodutibilidade. Utilizamos o arquivo `.config/dotnet-tools.json` para versionar ferramentas como o `dotnet-ef`. Isso garante que todo o time utilize a mesma versão da CLI, evitando erros de migração de banco de dados causados por incompatibilidade de versões do SDK.