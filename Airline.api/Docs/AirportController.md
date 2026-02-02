# Airport Controller API Documentation

**Path base:** `/airport`

---

## Overview
Este documento descreve as rotas expostas pelo `AirportController` e os formatos de request/response.

---

## Endpoints

### 1) Criar aeroporto
**POST** `/airport/create`

**Descrição:** Cria um novo aeroporto no sistema.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "iataCode": "GRU",
  "name": "São Paulo/Guarulhos International Airport",
  "city": "São Paulo",
  "country": "Brazil",
  "timeZoneName": "America/Sao_Paulo"
}
```

**Campos (AirportCreateDTO):**
- `iataCode` (string) - **obrigatório**: código IATA do aeroporto (ex.: "GRU", "JFK", "LHR").
- `name` (string) - **obrigatório**: nome completo do aeroporto.
- `city` (string) - **obrigatório**: cidade onde o aeroporto está localizado.
- `country` (string) - **obrigatório**: país onde o aeroporto está localizado.
- `timeZoneName` (string) - **obrigatório**: nome da zona horária do aeroporto (ex.: "America/Sao_Paulo", "America/New_York", "Europe/London").

**Respostas possíveis:**
- `201 Created` — Aeroporto criado com sucesso. (Controller retorna `Results.Created()` sem body adicional.)
- `400 Bad Request` — Erros de validação retornados como array de erros. Lançada como `ValidationServiceException`.
- `500 Internal Server Error` — Erro inesperado.

**Validações:**
- `iataCode` — Não pode ser vazio ou nulo. Deve ser único no sistema. Máximo 3 caracteres.
- `name` — Não pode ser vazio ou nulo.
- `city` — Não pode ser vazio ou nulo.
- `country` — Não pode ser vazio ou nulo.
- `timeZoneName` — Não pode ser vazio ou nulo. Deve ser um identificador válido de zona horária (IANA Time Zone Database).

**Exemplo de resposta de erro (400):**
```json
{
    "errors": [
        "iataCode is required",
        "iataCode must be 3 characters",
        "iataCode already exists",
        "timeZoneName is required",
        "timeZoneName is invalid"
    ]
}
```
**Notas / Observações:**
- Todos os campos são obrigatórios.
- O código IATA deve ser único no sistema.
- A zona horária deve seguir o padrão IANA (ex.: "America/New_York", "Europe/London", "Asia/Tokyo").

---

### 2) Listar aeroportos
**GET** `/airport/list`

**Descrição:** Retorna a lista de aeroportos cadastrados no sistema com opção de filtros.

**Query parameters (AirportListFilters):**
- `city` (string) — opcional: filtrar aeroportos por cidade.
- `country` (string) — opcional: filtrar aeroportos por país.

**Exemplos:**

```
GET /airport/list
GET /airport/list?city=São Paulo
GET /airport/list?country=Brazil
GET /airport/list?city=São Paulo&country=Brazil
```

**Sucesso (200 OK)**
Retorna um array de `AirportListDetailDTO` com o formato:

```json
[
  {
    "airportId": 1,
    "iataCode": "GRU",
    "name": "São Paulo/Guarulhos International Airport",
    "city": "São Paulo",
    "country": "Brazil"
  },
  {
    "airportId": 2,
    "iataCode": "CGH",
    "name": "São Paulo/Congonhas Airport",
    "city": "São Paulo",
    "country": "Brazil"
  },
  {
    "airportId": 3,
    "iataCode": "JFK",
    "name": "John F. Kennedy International Airport",
    "city": "New York",
    "country": "United States"
  }
]
```

**Campos do `AirportListDetailDTO`:**
- `airportId` (int): identificador único do aeroporto.
- `iataCode` (string): código IATA do aeroporto.
- `name` (string): nome completo do aeroporto.
- `city` (string): cidade do aeroporto.
- `country` (string): país do aeroporto.

**Respostas possíveis:**
- `200 OK` — Lista de aeroportos retornada (pode estar vazia se nenhum match com filtros).
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- A listagem retorna todos os aeroportos se nenhum filtro for fornecido.
- Filtros podem ser combinados para resultados mais específicos (ex.: filtrar por cidade AND país).
- Esta operação é assíncrona.
