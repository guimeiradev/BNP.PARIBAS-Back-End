# Desafio BNP Paribas — Back-End

API REST desenvolvida em **.NET 10** para gerenciamento de movimentos manuais contábeis, com produtos e seus COSIFs associados. O projeto segue uma **Arquitetura em Camadas com domínio isolado**, inspirada nos princípios do Clean Architecture e utilizando padrões táticos do DDD como Repository Pattern e separação de entidades de domínio.

---

## Sumário

- [Arquitetura](#arquitetura)
- [Tecnologias e Pacotes](#tecnologias-e-pacotes)
- [Estrutura de Pastas](#estrutura-de-pastas)
- [Domínio](#domínio)
- [Banco de Dados](#banco-de-dados)
- [Extensions — Organização do Program.cs](#extensions--organização-do-programcs)
- [Middleware de Logging](#middleware-de-logging)
- [Serilog — Log Estruturado](#serilog--log-estruturado)
- [Versionamento de API](#versionamento-de-api)
- [CORS](#cors)
- [Endpoints](#endpoints)
- [Testes](#testes)
- [Como Executar](#como-executar)

---

## Arquitetura

O projeto é dividido em quatro camadas com responsabilidades bem definidas:

```
DesafioBnpParibasBackEnd/
├── Bnp.Paribas.API        → Ponto de entrada: controllers, middlewares, extensions, configuração
├── Bnp.Paribas.Domain     → Núcleo: entidades, interfaces e DTOs (sem dependência de framework)
├── Bnp.Paribas.Infra      → Infraestrutura: EF Core, repositórios, migrations, seed e mapeamentos
└── Bnp.Paribas.Test       → Testes unitários dos controllers com xUnit e Moq
```

A camada **Domain** não referencia nenhuma outra camada do projeto. Ela define os contratos (interfaces de repositório) que a **Infra** implementa e que a **API** consome via injeção de dependência. O fluxo de dependência aponta sempre para dentro:

```
API  ──→  Domain  ←──  Infra
```

Isso garante que o núcleo da aplicação não depende de detalhes de persistência, framework ou transporte — um dos pilares do Clean Architecture. Os padrões táticos aproveitados do DDD são o **Repository Pattern** (interfaces no domínio, implementações na infraestrutura) e a **separação clara das entidades de domínio**.

---

## Tecnologias e Pacotes

| Pacote | Finalidade |
|---|---|
| `Microsoft.EntityFrameworkCore.Sqlite` | ORM com banco SQLite |
| `Microsoft.EntityFrameworkCore.Design` | Suporte a migrations via CLI |
| `Serilog.AspNetCore` | Log estruturado em console e arquivo |
| `Asp.Versioning.Mvc` | Versionamento de API por URL |
| `Asp.Versioning.Mvc.ApiExplorer` | Integração do versionamento com Swagger |
| `Swashbuckle.AspNetCore` | Documentação automática via Swagger/OpenAPI |
| `xunit` | Framework de testes unitários |
| `Moq` | Criação de mocks nos testes |
| `coverlet.collector` | Coleta de cobertura de código |

---

## Estrutura de Pastas

```
Bnp.Paribas.API/
├── Controller/
│   ├── ProdutoController.cs
│   └── MovimentoManualController.cs
├── Extensions/
│   ├── ApiExtension.cs          → Swagger, controllers e pipeline HTTP
│   ├── CorsExtension.cs         → Política de CORS por ambiente
│   ├── DatabaseExtension.cs     → DbContext, migrations e seed
│   ├── MiddlewareExtension.cs   → Registro do middleware de logging
│   ├── RepositoryExtension.cs   → Injeção dos repositórios
│   ├── SerilogExtension.cs      → Configuração do Serilog
│   └── VersioningExtension.cs   → Versionamento da API
├── Middlewares/
│   └── LoggingMiddleware.cs     → Interceptação e log de todas as requisições
├── Logs/                        → Arquivos de log diários gerados em runtime
└── Program.cs                   → Composição da aplicação

Bnp.Paribas.Domain/
├── Entity/
│   ├── Produto.cs
│   ├── ProdutoCosif.cs
│   └── MovimentoManual.cs
├── DTOs/
│   ├── GetProdutoDto.cs
│   ├── GetProdutoCosifDto.cs
│   ├── GetMovimentoManualDto.cs
│   └── InsertMovimentoManualDto.cs
└── Interfaces/Repository/
    ├── IProdutoRepository.cs
    └── IMovimentoManualRepository.cs

Bnp.Paribas.Infra/
├── Context/
│   └── BnpContext.cs
├── Config/
│   ├── ProdutoConfiguration.cs
│   ├── ProdutoCosifConfiguration.cs
│   └── MovimentoManualConfiguration.cs
├── Repository/
│   ├── ProdutoRepository.cs
│   └── MovimentoManualRepository.cs
├── Migrations/
└── Seed/
    └── DatabaseSeeder.cs

Bnp.Paribas.Test/
└── Controllers/
    ├── ProdutoControllerTests.cs
    └── MovimentoManualControllerTests.cs
```

---

## Domínio

### Entidades

#### `Produto`
Representa um produto financeiro cadastrado no sistema.

| Campo | Tipo | Descrição |
|---|---|---|
| `CodProduto` | `varchar(20)` | Chave primária |
| `DesProduto` | `varchar(100)` | Descrição do produto |
| `StaStatus` | `char(1)` | `'A'` (Ativo) ou `'I'` (Inativo) |

#### `ProdutoCosif`
Representa o código COSIF (Plano de Contas do Sistema Financeiro Nacional) vinculado a um produto. A chave primária é composta por `CodProduto + CodCosif`.

| Campo | Tipo | Descrição |
|---|---|---|
| `CodProduto` | `varchar(20)` | FK para Produto |
| `CodCosif` | `varchar(20)` | Código COSIF |
| `CodClassificacao` | `varchar(20)` | `'Normal'` ou `'MTM'` |
| `StaStatus` | `char(1)` | `'A'` (Ativo) ou `'I'` (Inativo) |

#### `MovimentoManual`
Representa um lançamento contábil manual. A chave primária é composta por `DatMes + DatAno + NumLancamento`.

| Campo | Tipo | Descrição |
|---|---|---|
| `DatMes` | `int` | Mês de competência |
| `DatAno` | `int` | Ano de competência |
| `NumLancamento` | `long` | Número sequencial do lançamento no mês/ano |
| `CodProduto` | `varchar(20)` | FK para ProdutoCosif |
| `CodCosif` | `varchar(20)` | FK para ProdutoCosif |
| `ValValor` | `decimal(18,2)` | Valor do movimento |
| `DesDescricao` | `varchar(100)` | Descrição do lançamento |
| `DatMovimento` | `DateTime` | Data/hora de inserção (preenchida automaticamente) |
| `CodUsuario` | `varchar(20)` | Código do usuário que criou o lançamento |

### Relacionamentos

```
Produto (1) ──── (N) ProdutoCosif (1) ──── (N) MovimentoManual
```

- Um `Produto` pode ter vários `ProdutoCosif`.
- Um `MovimentoManual` referencia obrigatoriamente um par `CodProduto + CodCosif` existente em `ProdutoCosif`.

### Geração do `NumLancamento`

O número do lançamento **não é uma identity do banco**. A cada novo `POST`, o repositório consulta o maior `NumLancamento` existente para o `DatMes` e `DatAno` informados e incrementa em 1. Isso simula o comportamento de uma sequence por período, equivalente ao que seria feito por uma stored procedure em um banco de produção.

```csharp
var maxLancamento = await context.MovimentosManuais
    .Where(m => m.DatMes == dto.DatMes && m.DatAno == dto.DatAno)
    .Select(m => (int?)m.NumLancamento)
    .MaxAsync();

var numLancamento = (maxLancamento ?? 0) + 1;
```

---

## Banco de Dados

### SQLite + Entity Framework Core

O banco utilizado é **SQLite**, armazenado em arquivo local (`MovimentosManuais.db`), configurado via connection string no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=MovimentosManuais.db"
}
```

### Fluent API — Configurações de Mapeamento

Cada entidade possui uma classe de configuração separada em `Bnp.Paribas.Infra/Config/`, todas registradas automaticamente via:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(BnpContext).Assembly);
```

As configurações definem nomes de tabelas, chaves primárias compostas, tipos de coluna, check constraints e relacionamentos:

- **`ProdutoConfiguration`** — define check constraint `StaStatus IN ('A', 'I')`
- **`ProdutoCosifConfiguration`** — define check constraints para `StaStatus` e `CodClassificacao IN ('Normal', 'MTM')`
- **`MovimentoManualConfiguration`** — define chave composta `(DatMes, DatAno, NumLancamento)`, impede geração automática do `NumLancamento` com `ValueGeneratedNever()` e configura a FK composta para `ProdutoCosif`

### Migrations e Seed Automático

Ao iniciar a aplicação, duas operações acontecem automaticamente via `DatabaseExtension`:

```csharp
await context.Database.MigrateAsync();   // aplica migrations pendentes
await DatabaseSeeder.SeedAsync(context); // insere dados iniciais se o banco estiver vazio
```

O `DatabaseSeeder` popula o banco com 3 produtos e 4 COSIFs de exemplo para que a API já esteja funcional logo após subir, sem necessidade de inserção manual.

---

## Extensions — Organização do Program.cs

Em vez de concentrar toda a configuração no `Program.cs`, cada responsabilidade foi extraída para uma **extension method estática** dentro da pasta `Extensions/`. Isso mantém o `Program.cs` enxuto e facilita encontrar ou alterar cada configuração isoladamente.

```csharp
// Program.cs
builder.Host.AddSerilog();

builder.Services.AddApi();
builder.Services.AddVersioning();
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddCorsPolicy(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseApi();

await app.SeedDatabaseAsync();
```

Cada linha mapeia diretamente para uma extension:

| Chamada | Arquivo | O que faz |
|---|---|---|
| `AddSerilog()` | `SerilogExtension.cs` | Configura o Serilog como provider de log |
| `AddApi()` | `ApiExtension.cs` | Registra controllers e Swagger |
| `AddVersioning()` | `VersioningExtension.cs` | Configura versionamento por URL |
| `AddDatabase()` | `DatabaseExtension.cs` | Registra o `BnpContext` com SQLite |
| `AddRepositories()` | `RepositoryExtension.cs` | Registra os repositórios no container DI |
| `AddCorsPolicy()` | `CorsExtension.cs` | Configura a política de CORS por ambiente |
| `UseApi()` | `ApiExtension.cs` | Monta o pipeline HTTP (Swagger, middleware, CORS, controllers) |
| `SeedDatabaseAsync()` | `DatabaseExtension.cs` | Aplica migrations e popula o banco |

---

## Middleware de Logging

O `LoggingMiddleware` intercepta **todas as requisições HTTP** que passam pela aplicação e registra informações detalhadas de cada uma delas.

### Como funciona

O middleware segue o padrão do ASP.NET Core: recebe o `RequestDelegate next` (que representa o próximo passo no pipeline) e o `ILogger`. Ele é registrado via `UseMiddleware<LoggingMiddleware>()` dentro da extensão `MiddlewareExtension`.

### O que é capturado

Para cada requisição, são coletados:

| Campo | Descrição |
|---|---|
| `TraceId` | Identificador único da requisição (`context.TraceIdentifier`) |
| `Method` | Verbo HTTP (`GET`, `POST`, etc.) |
| `Path` | Caminho da URL |
| `StatusCode` | Código HTTP da resposta |
| `Duration` | Tempo de execução em milissegundos (via `Stopwatch`) |
| `Body` | Corpo da requisição (apenas para `POST`, `PUT` e `PATCH`) |
| `QueryParams` | Query string (apenas para `GET`) |
| `Error` | Mensagem de exceção, se houver |

### Leitura do body sem consumir o stream

Para métodos que enviam body (`POST`, `PUT`, `PATCH`), o middleware precisa ler o `Request.Body` antes de repassar a requisição para o controller — mas sem "consumir" o stream, pois o controller também precisa lê-lo. Para isso, é utilizado `EnableBuffering()`, que permite que o stream seja lido múltiplas vezes, e o ponteiro é resetado para `0` após a leitura:

```csharp
context.Request.EnableBuffering();
using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
requestBody = await reader.ReadToEndAsync();
context.Request.Body.Position = 0;
```

### Nível de log por status HTTP

O nível do log é determinado automaticamente pelo status code da resposta:

```csharp
var logLevel = context.Response.StatusCode >= 500 ? LogLevel.Error
             : context.Response.StatusCode >= 400 ? LogLevel.Warning
             : LogLevel.Information;
```

### Captura de exceções

O middleware envolve a chamada ao próximo handler em um `try/catch/finally`. Se uma exceção não tratada for lançada, o erro é registrado, o status é forçado para `500` e a exceção é re-lançada para não suprimir o comportamento original do framework:

```csharp
try
{
    await next(context);
}
catch (Exception ex)
{
    errorMessage = ex.Message;
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    throw;
}
finally
{
    // o log é sempre registrado, com ou sem erro
}
```

---

## Serilog — Log Estruturado

O Serilog é configurado como provider de log do host na extensão `SerilogExtension`, substituindo o logging padrão do ASP.NET Core.

### Saídas configuradas

- **Console** — exibe os logs em tempo real durante o desenvolvimento
- **Arquivo** — salva em `Logs/log-{data}.txt`, com rotação diária (`RollingInterval.Day`) e retenção de até 30 arquivos

### Formato do log em arquivo

```
2026-05-22 14:30:00 [INF] Http Request | TraceId: ... | POST /v1/movimentos | Status: 201 | Duration: 45ms | Body: {...} | QueryParams: - | Error: -
```

---

## Versionamento de API

O versionamento é configurado na extensão `VersioningExtension` usando o pacote `Asp.Versioning`. A versão é informada diretamente na URL no formato `v{version}`.

```
/v1/produtos
/v1/movimentos
```

Configurações aplicadas:

- **Versão padrão**: `v1`
- **`AssumeDefaultVersionWhenUnspecified = true`** — requisições sem versão na URL usam v1 automaticamente
- **`ReportApiVersions = true`** — a resposta inclui o header `api-supported-versions`

Para criar uma nova versão futuramente, basta decorar o controller com `[ApiVersion(2)]` e o Swagger gera a documentação separada automaticamente.

---

## CORS

A política de CORS é configurada na extensão `CorsExtension` e varia conforme o ambiente de execução.

### Development

Aceita requisições de **qualquer origem**, sem restrição de porta ou domínio. Isso permite integrar com o front-end Angular em qualquer porta local:

```csharp
policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
```

### Production

Aceita apenas as origens explicitamente listadas em `appsettings.json`:

```json
"Cors": {
  "AllowedOrigins": [ "https://seu-dominio.com" ]
}
```

### Posição no pipeline

O `UseCorsPolicy()` é chamado antes do `UseHttpsRedirection()` e do `MapControllers()`, garantindo que o header `Access-Control-Allow-Origin` seja adicionado inclusive em requisições preflight (`OPTIONS`).

---

## Endpoints

### Produtos — `v1/produtos`

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/v1/produtos` | Lista todos os produtos |
| `GET` | `/v1/produtos/{codProduto}/cosifs` | Lista os COSIFs de um produto |

**Exemplo de resposta — `GET /v1/produtos`:**
```json
[
  { "codProduto": "PROD001", "desProduto": "Fundo de Renda Fixa" },
  { "codProduto": "PROD002", "desProduto": "Fundo de Ações" }
]
```

**Exemplo de resposta — `GET /v1/produtos/PROD001/cosifs`:**
```json
[
  { "codCosif": "1.1.1.40.10", "codClassificacao": "Normal" },
  { "codCosif": "1.1.1.40.20", "codClassificacao": "MTM" }
]
```

---

### Movimentos Manuais — `v1/movimentos`

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/v1/movimentos` | Cria um novo movimento manual |
| `GET` | `/v1/movimentos` | Lista todos os movimentos com join no produto |

**Exemplo de body — `POST /v1/movimentos`:**
```json
{
  "datMes": 5,
  "datAno": 2026,
  "codProduto": "PROD001",
  "codCosif": "1.1.1.40.10",
  "valValor": 1500.00,
  "desDescricao": "Lançamento de ajuste",
  "codUsuario": "guilherme"
}
```

**Resposta:** `201 Created`

**Exemplo de resposta — `GET /v1/movimentos`:**
```json
[
  {
    "datMes": 5,
    "datAno": 2026,
    "codProduto": "PROD001",
    "desProduto": "Fundo de Renda Fixa",
    "numLancamento": 1,
    "desDescricao": "Lançamento de ajuste",
    "valValor": 1500.00
  }
]
```

O `GET` de movimentos executa uma **query SQL raw com JOIN** entre `MovimentoManual` e `Produto`, simulando o comportamento de uma stored procedure — decisão tomada pois o SQLite não suporta procedures nativamente.

---

## Testes

Os testes ficam em `Bnp.Paribas.Test` e cobrem os dois controllers com xUnit e Moq. As interfaces de repositório são mockadas, isolando completamente os testes da camada de banco de dados.

### `ProdutoControllerTests`

| Teste | Cenário |
|---|---|
| `Get_QuandoExistemProdutos_DeveRetornarOkComLista` | Retorna `200 OK` com a lista quando há produtos |
| `Get_QuandoNaoExistemProdutos_DeveRetornarOkComListaVazia` | Retorna `200 OK` com lista vazia |
| `GetCosifs_QuandoExistemCosifs_DeveRetornarOkComLista` | Retorna `200 OK` com COSIFs do produto |
| `GetCosifs_QuandoNaoExistemCosifs_DeveRetornarOkComListaVazia` | Retorna `200 OK` com lista vazia |

### `MovimentoManualControllerTests`

| Teste | Cenário |
|---|---|
| `Post_QuandoDtoValido_DeveRetornarCreated` | Retorna `201 Created` e chama o repositório exatamente uma vez |
| `Get_QuandoExistemMovimentos_DeveRetornarOkComLista` | Retorna `200 OK` com a lista de movimentos |
| `Get_QuandoNaoExistemMovimentos_DeveRetornarOkComListaVazia` | Retorna `200 OK` com lista vazia |

### Executando os testes

```bash
dotnet test
```

---

## Como Executar

### Pré-requisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Passos

```bash
# 1. Clone o repositório
git clone <url-do-repositorio>
cd DesafioBnpParibasBackEnd

# 2. Execute a API
dotnet run --project Bnp.Paribas.API

# 3. Acesse o Swagger (ambiente de desenvolvimento)
# http://localhost:{porta}/swagger
```

O banco de dados SQLite é criado automaticamente na primeira execução, as migrations são aplicadas e os dados de seed são inseridos — nenhuma configuração adicional é necessária.

### Variáveis de configuração relevantes (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=MovimentosManuais.db"
  },
  "Cors": {
    "AllowedOrigins": []
  }
}
```

> Em produção, preencha `Cors.AllowedOrigins` com o domínio do front-end Angular.