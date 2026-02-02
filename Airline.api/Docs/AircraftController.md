# Aircraft Controller API Documentation

**Path base:** `/aircraft`

---

## Overview
Este documento descreve as rotas expostas pelo `AircraftController` e os formatos de request/response.

---

## Endpoints

### 1) Criar aeronave
**POST** `/aircraft/create`

**Descrição:** Cria uma nova aeronave no sistema.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "model": "Boeing 737",
  "capacity": 189,
  "range": 5400.0,
  "averageFuelConsumption": 750.5
}
```

**Campos (AircraftCreateDTO):**
- `model` (string) - **obrigatório**: modelo da aeronave (ex.: "Boeing 737", "Airbus A320").
- `capacity` (int) - **obrigatório**: capacidade de passageiros da aeronave.
- `range` (double) - **obrigatório**: alcance máximo da aeronave em quilômetros.
- `averageFuelConsumption` (double) - **obrigatório**: consumo médio de combustível por hora.

**Respostas possíveis:**
- `201 Created` — Aeronave criada com sucesso. 
- `400 Bad Request` — Erros de binding/JSON inválido ou campos obrigatórios faltando.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- Todos os campos são obrigatórios.

---

### 2) Listar aeronaves
**GET** `/aircraft/list`

**Descrição:** Retorna a lista de todas as aeronaves cadastradas no sistema.

**Query parameters:**
- `page` (int) — opcional, padrão: 1: número da página para paginação.
- `perPage` (int) — opcional, padrão: 10: quantidade de registros por página.

**Exemplo:**

```
GET /aircraft/list?page=1&perPage=10
```

**Sucesso (200 OK)**
Retorna um array de `Aircraft` com o formato:

```json
[
  {
    "aircraftID": 1,
    "model": "Boeing 737",
    "capacity": 189,
    "range": 5400.0,
    "averageFuelConsumption": 750.5
  },
  {
    "aircraftID": 2,
    "model": "Airbus A320",
    "capacity": 194,
    "range": 6300.0,
    "averageFuelConsumption": 850.0
  }
]
```

**Campos do `Aircraft`:**
- `aircraftID` (int): identificador único da aeronave.
- `model` (string): modelo da aeronave.
- `capacity` (int): capacidade de passageiros.
- `range` (double): alcance máximo em quilômetros.
- `averageFuelConsumption` (double): consumo médio de combustível.

---

### 3) Obter detalhes de uma aeronave
**GET** `/aircraft/{aircraftId}`

**Descrição:** Retorna os detalhes de uma aeronave específica pelo ID.

**Path parameters:**
- `aircraftId` (int) - **obrigatório**: identificador da aeronave.

**Exemplo:**

```
GET /aircraft/1
```

**Sucesso (200 OK)**
Retorna um objeto `Aircraft`:

```json
{
  "aircraftID": 1,
  "model": "Boeing 737",
  "capacity": 189,
  "range": 5400.0,
  "averageFuelConsumption": 750.5
}
```

**Campos do `Aircraft`:**
- `aircraftID` (int): identificador único da aeronave.
- `model` (string): modelo da aeronave.
- `capacity` (int): capacidade de passageiros.
- `range` (double): alcance máximo em quilômetros.
- `averageFuelConsumption` (double): consumo médio de combustível.

**Respostas possíveis:**
- `200 OK` — Aeronave encontrada e retornada.
- `404 Not Found` — Retorna JSON `{ "Message": "Aircraft not found!" }` quando a aeronave não é encontrada.

---

### 4) Atualizar aeronave
**PATCH** `/aircraft/update/{id}`

**Descrição:** Atualiza os dados de uma aeronave existente. Todos os campos são opcionais; apenas os fornecidos serão atualizados.

**Path parameters:**
- `id` (int) - **obrigatório**: identificador da aeronave a ser atualizada.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "capacity": 200,
  "range": 5500.0,
  "averageFuelConsumption": 760.0
}
```

**Campos (AircraftUpdateRequestBody):**
- `capacity` (int?) — opcional: nova capacidade de passageiros.
- `range` (double?) — opcional: novo alcance máximo em quilômetros.
- `averageFuelConsumption` (double?) — opcional: novo consumo médio de combustível.

**Respostas possíveis:**
- `200 OK` — Aeronave atualizada com sucesso.
- `404 Not Found` — Quando a aeronave não é encontrada — lançada como `EntityNotFoundException`.
- `400 Bad Request` — Erros de binding/JSON inválido.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- É possível atualizar apenas alguns campos deixando os outros nulos.
- Apenas os campos fornecidos (não nulos) serão atualizados.

---

### 5) Deletar aeronave
**DELETE** `/aircraft/{aircraftId}`

**Descrição:** Remove uma aeronave do sistema.

**Path parameters:**
- `aircraftId` (int) - **obrigatório**: identificador da aeronave a ser deletada.

**Exemplo:**

```
DELETE /aircraft/1
```

**Respostas possíveis:**
- `200 OK` — Aeronave deletada com sucesso.
- `404 Not Found` — Quando a aeronave não é encontrada — lançada como `EntityNotFoundException`.
- `500 Internal Server Error` — Erro inesperado.

**Notas / Observações:**
- Esta operação é assíncrona.
- Ao deletar uma aeronave, validar se existem voos associados a ela antes de permitir a exclusão.
