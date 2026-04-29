
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
|   `-- mysql/init/001_create_schema.sql
|-- src/
|   |-- backend/RadarBolsa.Api
|   `-- frontend
|-- docker-compose.yml
`-- RadarBolsa.slnx
```

## Entregas desta etapa
- API inicial com `/health` e `/api/opportunities`
- Frontend Vue com painel demonstrativo
- Script inicial de banco para ativos monitorados e oportunidades
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

Nesta etapa, o `docker compose` sobe MySQL e API. O frontend roda localmente via Vite.

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

## Proximas etapas
1. Conectar a API ao MySQL.
2. Substituir dados mockados por repositorio real.
3. Adicionar cadastro e monitoramento de ativos.
4. Evoluir para ingestao automatizada e alertas.
