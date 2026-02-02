
# Airline API 

A Airline API faz parte da "Saga dos Aviõeszinhos", que tem o objetivo de fazer uma aplicação completa, usando como contexto um app de compra de passagens aéreas. Esse projeto contempla a parte da API que será consimida pelo frontend. O objetivo é que ela tenha todas as rotas necessárias para a criação, gerenciamento e venda de passagens, que futuramente poderão ser usadas, tanto pelo app para o usuário final, quanto por um portal de cadastro de voos, rotas etc..

---

## Sumário
- [Requisitos](#requisitos)
- [Como Executar](#como-executar)
- [Execução via Docker](#execução-via-docker)
- [Testes](#testes)
- [Documentação](#documentação)
- [Contribuindo](#contribuindo)
- [Contato](#contato)

---

## Requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- PostgreSQL (ou outro banco configurado em `appsettings.json`)
- Docker (opcional, para execução conteinerizada)

---

## Como Executar
1. Clone o repositório:
	```bash
	git clone https://github.com/GabrielJusto/Airline.git
	cd Airline
	```
2. Restaure as dependências:
	```bash
	dotnet restore
	```
3. Configure o banco de dados em `Airline.api/appsettings.Development.json`.
4. Aplique as migrations:
	```bash
	dotnet ef database update --project Airline.api
	```
5. Execute a API:
	```bash
	dotnet run --project Airline.api
	```

---

## Execução via Docker
1. Configure variáveis de ambiente no `.env` ou `appsettings.json`.
2. Execute:
	```bash
	docker-compose up --build
	```

---

## Testes
Os testes estão em `Airline.Tests`.

Para rodar:
```bash
dotnet test Airline.Tests
```

---
## Documentação
- Consulte a [Wiki do Projeto](../../wiki) para documentação completa e exemplos de uso.

---

## Contribuindo
- Faça um fork e PR
- Siga as convenções de código
- Execute `dotnet format` antes de submeter
- Abra issues para bugs ou sugestões

---

## Contato
Dúvidas ou sugestões? Abra uma issue ou entre em contato pelo repositório.

---

> Projeto desenvolvido para fins didáticos e demonstração de arquitetura .NET moderna.
