
# RadarBolsa

Sistema para rastrear e analisar oportunidades de investimento.

## Objetivo
O MVP do RadarBolsa organiza oportunidades priorizadas por score, setor e tese resumida, criando a base para evoluir ingestao de sinais, filtros e watchlists.

## Stack
- .NET 9 no backend
- Vue 3 no frontend
- MySQL para persistencia
- Docker para ambiente local

## Estrutura
```text
.
|-- docs/
|   `-- arquitetura.md
|-- infra/
|   `-- mysql/init/
|       |-- 001_create_schema.sql
|       |-- 002_seed_opportunities.sql
|       `-- 003_seed_manual_signals.sql
|-- src/
|   |-- backend/RadarBolsa.Api
|   |-- backend/RadarBolsa.Application
|   |-- backend/RadarBolsa.Domain
|   |-- backend/RadarBolsa.Infrastructure
|   `-- frontend
|-- docker-compose.yml
`-- README.md
```

## Entregas desta etapa
- API estruturada em Clean Architecture simples com `/health` e `/api/opportunities`
- API de ativos monitorados em `/api/tracked-assets`
- API de sinais manuais em `/api/signals`
- Frontend Vue com painel demonstrativo e ingestao manual de sinais
- Script inicial de banco para ativos monitorados, oportunidades e sinais
- Seed inicial de oportunidades e sinais no MySQL para validacao local
- Docker Compose com MySQL e API

## Como executar
### Backend
```powershell
$env:DOTNET_CLI_HOME=(Resolve-Path '.').Path
if (!(Test-Path '.nuget')) { New-Item -ItemType Directory -Path '.nuget' | Out-Null }
$env:NUGET_PACKAGES=(Resolve-Path '.nuget').Path
dotnet restore .\src\backend\RadarBolsa.Api\RadarBolsa.Api.csproj
dotnet run --project .\src\backend\RadarBolsa.Api\RadarBolsa.Api.csproj
```

### Frontend
```powershell
Set-Location .\src\frontend
npm install
npm run dev
```

### Docker
```powershell
docker compose up --build
```

Se quiser sobrescrever portas ou credenciais do ambiente local, use `.env` com base em `.env.example`.

O `docker-compose.yml` define o ambiente local inicial do projeto com:
- `mysql`: banco MySQL 8.4 com volume persistente, scripts em `infra/mysql/init` e acesso host em `localhost:3307`
- `api`: backend ASP.NET Core exposto em `http://localhost:8081`
- rede dedicada `radarbolsa-network` para comunicacao entre os servicos

Nesta etapa, o `docker compose` sobe MySQL e API com seed inicial de oportunidades. O frontend roda localmente via Vite.

Os scripts em `infra/mysql/init` sao executados automaticamente na primeira inicializacao de um volume novo do MySQL.

## Validacao rapida
```powershell
dotnet build .\src\backend\RadarBolsa.Api\RadarBolsa.Api.csproj -v minimal
Set-Location .\src\frontend
npm run build
curl.exe -i http://localhost:8081/health
```

## Endpoints iniciais
- `GET /health`
- `GET /api/opportunities`
- `GET /api/opportunities?minScore=80`
- `GET /api/opportunities?sector=Energia`
- `GET /api/tracked-assets`
- `GET /api/tracked-assets/{ticker}`
- `POST /api/tracked-assets`
- `GET /api/signals`
- `POST /api/signals`

## Contrato de oportunidades
- `GET /api/opportunities` retorna um array JSON simples para consumo direto do frontend.
- Ordenacao atual: `score` decrescente e, em caso de empate, `capturedAt` mais recente primeiro.
- Quando nenhum item atende aos filtros, a API responde `200 OK` com array vazio.
- Quando parametros invalidos sao informados, a API responde `400` com `ValidationProblem`.
- Parametros de query do MVP:
  - `minScore`: inteiro opcional para retornar apenas oportunidades com score minimo.
  - `sector`: texto opcional para filtrar por setor, sem diferenciar maiusculas e minusculas.
  - `minUpside`: decimal opcional para filtrar upside minimo em percentual.
  - `maxUpside`: decimal opcional para filtrar upside maximo em percentual.
  - `sortBy`: texto opcional com `score` ou `upside`. O padrao atual e `score`.
  - `sortDirection`: texto opcional com `desc` ou `asc`. O padrao atual e `desc`.
- Campos de resposta por item:
  - `ticker`
  - `companyName`
  - `sector`
  - `currentPrice`
  - `targetPrice`
  - `score`
  - `thesis`
  - `capturedAt`
  - `upsidePercent`
- Exemplos:
  - `GET /api/opportunities`
  - `GET /api/opportunities?minScore=80`
  - `GET /api/opportunities?sector=Financeiro`
  - `GET /api/opportunities?minScore=75&sector=Energia`
  - `GET /api/opportunities?minUpside=10`
  - `GET /api/opportunities?minUpside=10&maxUpside=16`
  - `GET /api/opportunities?sortBy=upside`
  - `GET /api/opportunities?sector=Financeiro&sortBy=upside&sortDirection=asc`
  - `GET /api/opportunities?minScore=abc` retorna `400`
  - `GET /api/opportunities?minUpside=20&maxUpside=10` retorna `400`

## Proximas etapas
1. Evoluir ingestao automatizada de sinais.
2. Ampliar a camada analitica com watchlists e alertas.
3. Preparar autenticacao e segregacao por usuario.
