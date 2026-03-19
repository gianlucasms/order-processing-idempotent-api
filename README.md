# Idempotency API (.NET 8)

A simple .NET 8 API demonstrating how to handle duplicate requests using idempotency.

## Overview

This project simulates a common scenario where the same request may be sent multiple times (e.g. retries, timeouts, or double clicks).

To prevent duplicated operations, the API uses an `Idempotency-Key` header.

- Same key → same response  
- No duplicated data  
- No reprocessing  

## How it works

- Every `POST /orders` request requires an `Idempotency-Key`
- The key and response are stored
- If the same key is received again:
  - The request is not executed again
  - The stored response is returned
  - Header `x-idempotency-replayed: true` is added

## Example


POST /orders
Idempotency-Key: 123


Repeat the same request:

- Same response is returned  
- No new record is created  
- `x-idempotency-replayed: true` header is included  

## Structure


Application/
Domain/
Infrastructure/
Controllers/
Middleware/


## Tech

- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQLite
- Swagger

## Run

```bash
dotnet run
