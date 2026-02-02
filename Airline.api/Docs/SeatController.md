# Seat Controller API Documentation 

**Path base:** `/seat`

---

## Overview
Este documento descreve as rotas expostas pelo `SeatController` e os formatos de request/response.

---

## Endpoints

### 1) Criar assentos
**POST** `/seat/create` 

**Descrição:** Cria assentos para um determinado voo (quantidade por classe).

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "flight_id": 123,
  "quantityEconomic": 30,
  "quantityExecutive": 8,
  "quantityFirstClass": 4
}
```

**Campos (SeatCreateRequestDTO):**
- `flight_id` (int) - **obrigatório**: id do voo ao qual os assentos serão adicionados.
- `quantityEconomic` (int) - **obrigatório**: quantidade de assentos na classe econômica (pode ser 0).
- `quantityExecutive` (int) - **obrigatório**: quantidade de assentos na classe executiva.
- `quantityFirstClass` (int) - **obrigatório**: quantidade de assentos na primeira classe.

**Respostas possíveis:**
- `201 Created` — Assentos criados com sucesso. (Controller retorna `Results.Created()` sem body adicional.)
- `404 Not Found` — Retorna JSON `{ "Message": "..." }` quando a entidade referenciada não é encontrada (ex.: voo não existe) — lançada como `EntityNotFoundException`.
- `400 Bad Request` — Erros de binding/JSON inválido ou formato de campos inválidos.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- Validações de domínio (ex.: limites máximos/mínimos) podem ser aplicadas pela camada de serviço; mensagens de erro serão retornadas como 4xx ou 5xx conforme o caso.

---

### 2) Listar assentos disponíveis para compra de passagem
**GET** `/seat/list-available-for-ticket` 

**Descrição:** Retorna a lista de assentos disponíveis para compra (com dados do voo e preço).

**Query parameters (SeatListFilterDTO):**
- `flightId` (int) — opcional: filtrar por id do voo.
- `fromIATACode` (string) — **obrigatório**: código IATA do aeroporto de origem (ex.: "GRU").
- `toIATACode` (string) — **obrigatório**: código IATA do aeroporto de destino.
- `departureDate` (YYYY-MM-DD) — opcional: filtrar por data de partida (formato DateOnly).

**Exemplo:**

```
GET /seat/list-available-for-ticket?fromIATACode=GRU&toIATACode=JFK&departureDate=2026-02-15
```

**Sucesso (200 OK)**
Retorna um array de `SeatTicketListDTO` com o formato:

```json
[
  {
    "seatId": 10,
    "fromIATACode": "GRU",
    "fromCity": "São Paulo",
    "toIATACode": "JFK",
    "toCity": "New York",
    "departure": "2026-02-15T08:30:00+00:00",
    "arrival": "2026-02-15T14:45:00+00:00",
    "flightDuration": "06:15",
    "price": 450.00,
    "flightNumber": "0123",
    "seatClass": "Economic"
  }
]
```

**Campos do `SeatTicketListDTO`:**
- `seatId` (int)
- `fromIATACode` (string)
- `fromCity` (string)
- `toIATACode` (string)
- `toCity` (string)
- `departure` (ISO 8601 datetime string)
- `arrival` (ISO 8601 datetime string)
- `flightDuration` (string, formato `hh:mm`)
- `price` (decimal)
- `flightNumber` (string)
- `seatClass` (string)

**Respostas possíveis:**
- `200 OK` — Retorna lista (pode ser vazia se não houver assentos).
- `400 Bad Request` — Retorna mensagem de validação quando filtros são inválidos (`ValidationException` capturada no controller), payload de erro é plain message.
- `500 Internal Server Error` — Erro inesperado.

---



