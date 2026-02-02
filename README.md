
# Airline API 

Este projeto foi criado com o objetivo de aprender a fazer uma API REST em .NET. Estou usando um contexto de uma empresa aéria para desenvolver a API, o objetivo é fazer um sistema que seja possível cadastrar aviões, rotas, calcular preço de passagens e simular a venda de passagens para um voo. Contrubuições são sempre bem-vindas :)

---

## Sumário
- [Visão Geral](#visão-geral)
- [Requisitos](#requisitos)
- [Como Executar](#como-executar)
- [Execução via Docker](#execução-via-docker)
- [Testes](#testes)
- [Documentação](#documentação)
- [Contribuindo](#contribuindo)
- [Contato](#contato)

---

## Visão Geral
Esta API permite:
- Cadastro e consulta de aeroportos, aviões, voos e assentos
- Compra de tickets
- Filtros passagens para busca de voos e assentos
- Autenticação JWT para rotas protegidas

A arquitetura segue boas práticas REST e utiliza .NET 9, Entity Framework Core e PostgreSQL.

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
