# Route Controller API Documentation

**Path base:** `/route`

---

## Overview
Este documento descreve as rotas expostas pelo `RouteController` e os formatos de request/response.

---

## Endpoints

### 1) Criar rota
**POST** `/route/create`

**Descrição:** Cria uma nova rota aérea no sistema associando dois aeroportos e definindo a distância.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "fromAirportId": 1,
  "toAirportId": 3,
  "distance": 9660.0
}
```

**Campos (RouteInsertRequestBody):**
- `fromAirportId` (int) - **obrigatório**: identificador do aeroporto de origem.
- `toAirportId` (int) - **obrigatório**: identificador do aeroporto de destino.
- `distance` (double) - **obrigatório**: distância entre os aeroportos em quilômetros.

**Respostas possíveis:**
- `201 Created` — Rota criada com sucesso. 
- `400 Bad Request` — Retorna JSON `{ "Errors": "..." }` quando há erros de validação — lançada como `ValidationException`.
- `500 Internal Server Error` — Erro inesperado.


---

### 2) Listar rotas
**GET** `/route/list`

**Descrição:** Retorna a lista de rotas cadastradas no sistema com opção de filtros.

**Query parameters (RouteListFiltersDTO):**
- `fromAirportId` (int) — opcional: filtrar rotas por aeroporto de origem.
- `toAirportId` (int) — opcional: filtrar rotas por aeroporto de destino.

**Exemplos:**

```
GET /route/list
GET /route/list?fromAirportId=1
GET /route/list?toAirportId=3
GET /route/list?fromAirportId=1&toAirportId=3
```

**Sucesso (200 OK)**
Retorna um array de `RouteListDTO` com o formato:

```json
[
  {
    "routeID": 1
  },
  {
    "routeID": 2
  },
  {
    "routeID": 3
  }
]
```

**Campos do `RouteListDTO`:**
- `routeID` (int): identificador único da rota.

**Respostas possíveis:**
- `200 OK` — Lista de rotas retornada (pode estar vazia se nenhum match com filtros).
- `500 Internal Server Error` — Erro inesperado.


