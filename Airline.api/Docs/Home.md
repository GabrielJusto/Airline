# Airline API — Wiki Home

**Bem-vindo à wiki da Airline API.** Este é o ponto de entrada para documentação de uso, rotas, execução local e contribuições.

---

## Sumário
- [Visão Geral](#visão-geral)
- [Como começar (rápido)](#como-começar-rápido)
- [Executando localmente](#executando-localmente)
- [Documentação das Rotas da API](#documentacao-rotas)
- [Contribuindo](#contribuindo-)
- [Contato / Ajuda](#contato--ajuda)

---

## Visão Geral
A `Airline API` faz parte da "Saga dos Aviõeszinhos", que tem o objetivo de fazer uma aplicação completa, usando como contexto um app de compra de passagens aéreas. Esse projeto contempla a parte da API que será consumida pelo frontend. O objetivo é que ela tenha todas as rotas necessárias para a criação, gerenciamento e venda de passagens, que futuramente poderão ser usadas, tanto pelo app para o usuário final, quanto por um portal de cadastro de voos, rotas etc...

---

## Como começar (rápido)
**Requisitos:**
- .NET 9 (`TargetFramework: net9.0`)
- PostgreSQL: Sugiro usar o Supabase ou Neon, caso não queira instalar um banco de dados localmente.

Comandos básicos:

```bash
# Restaurar dependências
dotnet restore
# Build
dotnet build
# Rodar a API (a partir da pasta do projeto Airline.api)
dotnet run --project Airline.api
```

O projeto também inclui `Dockerfile` e `docker-compose.yml` para ambientes conteinerizados.

---

## Executando localmente 
1. Configure variáveis de ambiente ou `appsettings.Development.json` (conexão com DB, JWT, etc.).
2. Aplique migrations se necessário:

```bash
dotnet ef database update --project Airline.api
```

3. Inicie a aplicação com `dotnet run` ou via Docker Compose.

---

## Documentação das Rotas da API

- [AircraftController](AircraftController)
- [AirportController](AirportController)
- [FlightController](FlightController)
- [RouteController](RouteController)
- [SeatController](SeatController)
- [TicketController](TicketController)
---
 
## Contribuindo 
- Abra uma issue descrevendo a alteração ou correção desejada.
- Faça um fork e PR com testes quando possível.
- Siga as convenções de código e execute `dotnet format` antes de abrir PR.

---

## Contato / Ajuda
Se precisar de ajuda, abra uma issue no repositório ou me peça para adicionar exemplos, links adicionais ou traduções.

---
