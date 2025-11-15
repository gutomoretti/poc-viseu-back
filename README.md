# PocViseu – Backend

API em ASP.NET Core 6 voltada ao controle de operações agrícolas, com autenticação JWT, integrações com robôs de automação e persistência em MySQL via Entity Framework Core.

---

## Visão Geral

- `PocViseu.Api`: camada de apresentação, com controllers, autenticação e Swagger.
- `PocViseu.Infrastructure`: infraestrutura de dados, `WebControlDbContext` e `Seeder`.
- `PocViseu.Core` e `PocViseu.Model`: utilidades e modelos compartilhados.

O projeto utiliza `EntityFrameworkCore` com `Database.EnsureCreated()` para garantir a existência das tabelas e, em seguida, executa o `Seeder` que popula dados iniciais sempre que o banco está vazio.

---

## Pré-requisitos

- .NET 6 SDK
- XAMPP com MySQL e phpMyAdmin (ou outro servidor MySQL 8+ compatível)
- Git (opcional, para versionamento)

---

## Banco de Dados MySQL

1. Inicie os serviços `Apache` e `MySQL` no painel do XAMPP.
2. Acesse o phpMyAdmin (`http://localhost/phpmyadmin`).
3. Crie manualmente o banco de dados `poc_viseu` (recomendado collation `utf8mb4_general_ci`).

> **Importante:** o projeto executa `Database.EnsureCreated()` durante o startup. Esse método cria apenas tabelas e relacionamentos quando o banco já existe. Por isso é obrigatório criar o banco previamente pelo phpMyAdmin no ambiente local.

Na primeira execução, com o banco vazio, o `Seeder` insere usuários padrão, configurações de sistema e cadastros auxiliares.

---

## Configuração da Aplicação

Os parâmetros principais ficam em `src/PocViseu.Api/appsettings.Development.json`:

- `DbConnection`: string de conexão MySQL. Ajuste usuário/senha conforme sua instalação XAMPP.
- `TokenKey`: chave usada para assinar tokens JWT.
- `UrlBot` e `UrlPing`: endpoints utilizados pelos serviços de automação.

Para outros ambientes, configure `appsettings.{Ambiente}.json` ou variáveis de ambiente equivalentes.

---

## Executando o Projeto

No diretório `backend`, execute:

```powershell
dotnet restore PocViseu.sln
dotnet build PocViseu.sln
dotnet run --project src/PocViseu.Api/PocViseu.Api.csproj
```

Durante o boot a aplicação:

- Cria um escopo DI e resolve `WebControlDbContext`;
- Chama `Database.EnsureCreated();`
- Executa `Seeder.Seed();` para inserir dados iniciais.

O Swagger fica disponível em `http://localhost:5227/swagger`.

---

## Dados Seedados

Após a primeira execução, os seguintes dados são criados:

- Usuário administrador: `admin`
- Senha inicial: `123456`

Altere essas credenciais após o primeiro login. Também são adicionados registros de `WebcorpConfig`, `Culture`, `Prague`, `ApplicationType` e outros cadastros essenciais.

---

## Testes e Health Check

- Endpoint de saúde: `GET /api/health`.
- Swagger UI com suporte a autenticação Bearer: `/swagger`.

---

## Próximos Passos

- Ajustar strings de conexão e segredos para homologação/produção.
- Avaliar migração de `EnsureCreated()` para `Migrate()` caso haja scripts de evolução de schema.
- Configurar variáveis sensíveis via Secret Manager ou variáveis de ambiente.

---

## Suporte

Em caso de dúvidas, abra uma issue no repositório ou contate o time responsável pela automação Viseu.

