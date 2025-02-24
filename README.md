# Product Price Negotiation API

Product Price Negotiation API to REST API umożliwiające **zarządzanie produktami** oraz **negocjowanie cen** między klientami a pracownikami sklepu.  
API wspiera **uwierzytelnianie JWT** dla pracowników sklepu, podczas gdy **klienci mogą korzystać bez logowania**.

---

## 📌 Instalacja
Aby uruchomić API lokalnie:

```bash
git clone https://github.com/TwojeRepo/ProductPriceNegotiationApi.git
cd ProductPriceNegotiationApi
dotnet restore
dotnet run
```

---

## 🔐 Autoryzacja
- **JWT (JSON Web Token)** jest wymagany dla pracowników sklepu, aby akceptować/odrzucać negocjacje.
- **Klienci** mogą składać oferty i pobierać produkty bez logowania.

### 🔑 Logowanie (JWT)
#### ➤ Żądanie:
```http
POST /api/auth/login
```
**Body:**
```json
{
  "username": "admin",
  "password": "admin"
}
```
#### ✅ Odpowiedź (200 OK):
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5c..."
}
```
#### ❌ Błędy:
- `401 Unauthorized` – Niepoprawne dane logowania.

Każde żądanie wymagające JWT musi zawierać nagłówek:
```http
Authorization: Bearer {TOKEN}
```

---

## 🛍️ Produkty

### ➕ Dodanie produktu
```http
POST /api/products
```
**Body:**
```json
{
  "name": "Laptop",
  "price": 5000.00
}
```
**Odpowiedź (201 Created):**
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 5000.00
}
```
**Błędy:**
- `400 Bad Request` – Niepoprawne dane (`name` pusty, `price` <= 0).

### 📥 Pobranie wszystkich produktów
```http
GET /api/products
```
**Odpowiedź (200 OK):**
```json
[
  { "id": 1, "name": "Laptop", "price": 5000.00 },
  { "id": 2, "name": "Smartphone", "price": 3000.00 }
]
```

### 🔍 Pobranie produktu
```http
GET /api/products/{id}
```
**Odpowiedź (200 OK):**
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 5000.00
}
```
**Błędy:**
- `404 Not Found` – Produkt nie istnieje.

---

## 💰 Negocjacje

### 📝 Złożenie oferty
```http
POST /api/negotiations/{productId}
```
**Body:**
```json
{
  "proposedPrice": 4000.00
}
```
**Odpowiedź (201 Created):**
```json
{
  "message": "Negotiation submitted successfully.",
  "negotiationId": 1
}
```
**Błędy:**
- `400 Bad Request` – Niepoprawna cena (`<= 0`).
- `404 Not Found` – Produkt nie istnieje.
- `409 Conflict` – Przekroczono 3 próby negocjacji.

### 🔄 Zmiana oferty (klient może zmienić cenę)
```http
PUT /api/negotiations/{negotiationId}/update
```
**Body:**
```json
{
  "proposedPrice": 4200.00
}
```
**Odpowiedź (200 OK):**
```json
{
  "message": "Negotiation updated successfully."
}
```
**Błędy:**
- `400 Bad Request` – Niepoprawna cena.
- `404 Not Found` – Negocjacja nie istnieje.
- `409 Conflict` – Osiągnięto limit 3 prób.

### ✅ Akceptacja/Odrzucenie negocjacji (pracownik sklepu)
```http
PUT /api/negotiations/{negotiationId}/response?accept=true
```
**Wymaga JWT ✅**

**Odpowiedź (200 OK):**
```json
{
  "message": "Negotiation accepted."
}
```
**Błędy:**
- `401 Unauthorized` – Brak tokena.
- `404 Not Found` – Negocjacja nie istnieje.
- `409 Conflict` – Negocjacja wygasła po 7 dniach.

### 📄 Pobranie negocjacji
```http
GET /api/negotiations/{negotiationId}
```
**Odpowiedź (200 OK):**
```json
{
  "negotiationId": 1,
  "productId": 1,
  "proposedPrice": 4000.00,
  "isAccepted": false,
  "attempts": 1,
  "dateProposed": "2025-02-24T12:00:00"
}
```
**Błędy:**
- `404 Not Found` – Negocjacja nie istnieje.

---

## 📌 Statusy odpowiedzi
| Kod | Opis |
|------|---------------------------------|
| 200 OK | Żądanie zakończone sukcesem |
| 201 Created | Element został utworzony |
| 400 Bad Request | Niepoprawne dane wejściowe |
| 401 Unauthorized | Brak autoryzacji (JWT wymagany) |
| 404 Not Found | Nie znaleziono zasobu |
| 409 Conflict | Negocjacja osiągnęła limit prób |

---

## 🧪 Testowanie
Aby uruchomić testy jednostkowe i integracyjne:

```bash
dotnet test
```

**Oczekiwany wynik:**
```yaml
Test Run Successful.
Total tests: 18
Passed: 18
Failed: 0
Skipped: 0
```

---

## 🛠️ Technologie
- **.NET 9.0**
- **Entity Framework Core**
- **SQLite / InMemory DB**
- **JWT Authentication**
- **xUnit, Moq**
