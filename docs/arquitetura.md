# RadarBolsa

## Objetivo
Sistema para rastrear e analisar oportunidades de investimento, com foco em um MVP capaz de consolidar sinais, destacar oportunidades priorizadas por score e oferecer uma base clara para evolucao futura.

## Diretrizes
- Construir por etapas.
- Priorizar o MVP antes de automacoes mais complexas.
- Evitar camadas desnecessarias nesta primeira entrega.
- Manter backend, frontend e infraestrutura desacoplados.

## Stack
- .NET 9 para a API
- Vue 3 para a interface web
- MySQL para persistencia
- Docker para ambiente local

## Escopo do MVP
- Exibir oportunidades de investimento com score, setor e tese resumida.
- Disponibilizar uma API simples para consulta dessas oportunidades.
- Preparar a base para persistencia em MySQL.
- Estruturar o repositorio para expansao futura sem reescrita imediata.

## Estrutura Inicial
- `src/backend/RadarBolsa.Domain`: entidades centrais do dominio.
- `src/backend/RadarBolsa.Application`: casos de uso e contratos da aplicacao.
- `src/backend/RadarBolsa.Infrastructure`: persistencia MySQL e integracoes.
- `src/backend/RadarBolsa.Api`: API ASP.NET Core com endpoints e composicao.
- `src/frontend`: aplicacao Vue 3 para o painel inicial.
- `infra/mysql`: scripts de banco para ambiente local.
- `docker-compose.yml`: orquestracao dos servicos base.
- `README.md`: guia rapido do projeto.

## Entrega Desta Etapa
- Estrutura inicial do monorepo.
- Backend organizado em Clean Architecture simples e pragmatica.
- API inicial com endpoints de saude e oportunidades.
- Frontend Vue com painel demonstrativo.
- Base de Docker e script inicial do MySQL.

## Proxima Etapa Recomendada
- Popular oportunidades no MySQL com seed inicial.
- Adicionar ingestao de ativos monitorados e filtros por perfil.
