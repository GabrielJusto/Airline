# Ticket Controller API Documentation

**Path base:** `/ticket`

---

## Overview
Este documento descreve as rotas expostas pelo `TicketController` e os formatos de request/response.

---

## Endpoints

### 1) Comprar passagem
**POST** `/ticket/purchase`

**Descrição:** Realiza a compra de uma passagem aérea, associando um assento a um usuário. O assento é marcado como indisponível após a compra bem-sucedida.

**Request**
- Headers:
  - `Content-Type: application/json`
- Body (JSON):

```json
{
  "seatId": 15,
  "userId": 2,
  "ownerDocument": "12345678901"
}
```

**Campos (TicketPurchaseRequestDTO):**
- `seatId` (int) - **obrigatório**: identificador do assento a ser comprado.
- `userId` (int) - **obrigatório**: identificador do usuário que está comprando a passagem.
- `ownerDocument` (string) - **obrigatório**: documento de identificação do proprietário da passagem.

**Respostas possíveis:**
- `200 OK` — Passagem comprada com sucesso. Retorna JSON:

```json
{
  "ticketId": 42
}
```

- `404 Not Found` — Retorna JSON `{ "Message": "..." }` quando:
  - O assento não é encontrado (ex.: "Seat not found.")
  - O usuário não é encontrado (ex.: "User not found.")
  - Lançada como `EntityNotFoundException`.

- `400 Bad Request` — Retorna JSON `{ "Message": "..." }` quando há erro de negócio, como:
  - O usuário já possui uma passagem para o mesmo voo (ex.: "This user already has a ticket for this flight.")
  - Lançada como `TicketPurchaseException`.

- `500 Internal Server Error` — Retorna JSON `{ "Message": "An error occurred while processing the request." }` para erros inesperados.

**Notas / Observações:**
- Todos os campos são obrigatórios.
- Um usuário não pode comprar mais de uma passagem para o mesmo voo.
- O assento deve estar disponível (IsAvailable = true) para poder ser comprado.
- Após a compra bem-sucedida, o assento é marcado como indisponível (IsAvailable = false).
- A validação verifica se o usuário já possui uma passagem para o voo associado ao assento, prevenindo duplicatas.

**Fluxo de validação:**
1. Verifica se o assento existe
2. Verifica se o usuário existe
3. Verifica se o usuário já possui uma passagem para este voo
4. Se todas as validações passarem, marca o assento como indisponível
5. Cria o registro da passagem e retorna o TicketId
