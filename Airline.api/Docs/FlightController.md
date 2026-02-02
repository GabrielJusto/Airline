# Flight Controller API Documentation

**Path base:** `/flight`

---

## Overview
Este documento descreve as rotas expostas pelo `FlightController` e os formatos de request/response.

---

## Endpoints

### 1) Criar voo
**POST** `/flight/create`

**Descrição:** Cria um novo voo no sistema associando uma rota e uma aeronave com datas de partida e chegada.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "routeId": 1,
  "aircraftId": 2,
  "departure": "2026-02-15T08:30:00+00:00",
  "arrival": "2026-02-15T14:45:00+00:00"
}
```

**Campos (FlightCreateDTO):**
- `routeId` (int) - **obrigatório**: identificador da rota associada ao voo.
- `aircraftId` (int) - **obrigatório**: identificador da aeronave que operará o voo.
- `departure` (ISO 8601 datetime string) - **obrigatório**: data e hora de partida (com timezone).
- `arrival` (ISO 8601 datetime string) - **obrigatório**: data e hora de chegada (com timezone).

**Respostas possíveis:**
- `201 Created` — Voo criado com sucesso. (Controller retorna `Results.Created()` sem body adicional.)
- `404 Not Found` — Retorna JSON `{ "Message": "..." }` quando a rota ou aeronave não é encontrada — lançada como `EntityNotFoundException`.
- `400 Bad Request` — Erros de binding/JSON inválido ou campos obrigatórios faltando.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- Todos os campos são obrigatórios.
- A data de chegada deve ser posterior à data de partida.

---

### 2) Obter detalhes de um voo
**GET** `/flight/{flightId}`

**Descrição:** Retorna os detalhes de um voo específico pelo ID, incluindo informações da aeronave.

**Path parameters:**
- `flightId` (int) - **obrigatório**: identificador do voo.

**Exemplo:**

```
GET /flight/5
```

**Sucesso (200 OK)**
Retorna um objeto `FlightDetailDTO`:

```json
{
  "flightId": 5,
  "departure": "2026-02-15T08:30:00Z",
  "arrival": "2026-02-15T14:45:00Z",
  "aircraftModel": "Boeing 737"
}
```

**Campos do `FlightDetailDTO`:**
- `flightId` (int): identificador único do voo.
- `departure` (ISO 8601 datetime string): data e hora de partida (UTC).
- `arrival` (ISO 8601 datetime string): data e hora de chegada (UTC).
- `aircraftModel` (string): modelo da aeronave que opera o voo.

**Respostas possíveis:**
- `200 OK` — Voo encontrado e retornado.
- `404 Not Found` — Retorna JSON `{ "Message": "..." }` quando o voo não é encontrado — lançada como `EntityNotFoundException`.

**Notas / Observações:**
- Esta operação é assíncrona.

---

### 3) Listar voos
**GET** `/flight/list`

**Descrição:** Retorna a lista de voos cadastrados no sistema com opção de filtros por rota e data.

**Query parameters (FlightListFilterDto):**
- `from` (string) — **obrigatório**: código IATA do aeroporto de origem (ex.: "GRU").
- `to` (string) — **obrigatório**: código IATA do aeroporto de destino (ex.: "JFK").
- `startDepartureDate` (YYYY-MM-DD) — opcional: data inicial de partida para filtro de intervalo.
- `endDepartureDate` (YYYY-MM-DD) — opcional: data final de partida para filtro de intervalo.

**Exemplos:**

```
GET /flight/list?from=GRU&to=JFK
GET /flight/list?from=GRU&to=JFK&startDepartureDate=2026-02-15
GET /flight/list?from=GRU&to=JFK&startDepartureDate=2026-02-15&endDepartureDate=2026-02-20
```

**Sucesso (200 OK)**
Retorna um array de `FlightListDTO` com o formato:

```json
[
  {
    "flightId": 1,
    "departure": "2026-02-15T08:30:00+00:00",
    "arrival": "2026-02-15T14:45:00+00:00"
  },
  {
    "flightId": 2,
    "departure": "2026-02-15T18:00:00+00:00",
    "arrival": "2026-02-16T00:15:00+00:00"
  }
]
```

**Campos do `FlightListDTO`:**
- `flightId` (int): identificador único do voo.
- `departure` (ISO 8601 datetime string): data e hora de partida (com timezone).
- `arrival` (ISO 8601 datetime string): data e hora de chegada (com timezone).

**Respostas possíveis:**
- `200 OK` — Lista de voos retornada (pode estar vazia se nenhum match com filtros).
- `400 Bad Request` — Erro se os campos `from` e `to` não forem fornecidos.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- Os parâmetros `from` e `to` são obrigatórios para realizar a busca.
- Os filtros de data (`startDepartureDate` e `endDepartureDate`) são opcionais e podem ser usados para restringir a busca a um período específico.
- Se apenas `startDepartureDate` for fornecido, serão retornados voos a partir dessa data.
- Se apenas `endDepartureDate` for fornecido, serão retornados voos até essa data.
- Esta operação é assíncrona.
