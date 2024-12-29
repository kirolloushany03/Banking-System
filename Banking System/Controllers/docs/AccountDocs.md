### **1. Create Account**

**Endpoint:**  
`POST /api/accounts`

**Description:**  
This API creates a new account in the system. The user must provide the account number, account type, initial balance, overdraft limit, and interest rate. The behavior of the overdraft limit and interest rate depends on the type of account being created:
- **Checking Account (Type 0):** The overdraft limit should be provided, and interest rate should be set to `0` (No interest).
- **Savings Account (Type 1):** The interest rate should be provided, and overdraft limit should be set to `0` (No overdraft).
- **Credit Account (Type 2):** Both the overdraft limit and interest rate can be provided.

**Request Body:**
```json
{
  "accountNumber": "string",   // Unique identifier for the account.
  "accountType": "integer",    // 0: Checking, 1: Savings, 2: Credit
  "initialBalance": "number",  // The starting balance of the account.
  "overdraftLimit": "number", // Only used for Checking and Credit accounts. Set to 0 for Savings.
  "interestRate": "number"    // Only used for Savings and Credit accounts. Set to 0 for Checking.
}
```

**Request Parameters:**
- `accountNumber` (string): A unique identifier for the account.
- `accountType` (integer): The type of account being created. Possible values are:
  - `0`: Checking
  - `1`: Savings
  - `2`: Credit
- `initialBalance` (number): The initial balance of the account (usually set to 0).
- `overdraftLimit` (number): The overdraft limit applied to the account (relevant only for Checking and Credit accounts).
- `interestRate` (number): The interest rate applied to the account (relevant only for Savings and Credit accounts).

**Response:**
```json
{
  "accountNumber": "string",     // Account number assigned to the new account.
  "accountType": "string",       // The type of the account (Checking, Savings, Credit).
  "initialBalance": "number",    // The starting balance of the account.
  "overdraftLimit": "number",    // Overdraft limit applied (if applicable).
  "interestRate": "number"      // Interest rate applied (if applicable).
}
```

**Response Parameters:**
- `accountNumber` (string): The unique account number assigned to the new account.
- `accountType` (string): The type of account created (e.g., "Checking", "Savings", or "Credit").
- `initialBalance` (number): The balance of the new account.
- `overdraftLimit` (number): The overdraft limit (only relevant for Checking and Credit accounts).
- `interestRate` (number): The interest rate (only relevant for Savings and Credit accounts).

**Example:**
1. **Creating a Checking Account (Type 0)**
   Request:
   ```json
   {
     "accountNumber": "CHK12345",
     "accountType": 0,
     "initialBalance": 0,
     "overdraftLimit": 500,
     "interestRate": 0
   }
   ```
   Response:
   ```json
   {
     "accountNumber": "CHK12345",
     "accountType": "Checking",
     "initialBalance": 0,
     "overdraftLimit": 500,
     "interestRate": 0
   }
   ```

2. **Creating a Savings Account (Type 1)**
   Request:
   ```json
   {
     "accountNumber": "SAV12345",
     "accountType": 1,
     "initialBalance": 1000,
     "overdraftLimit": 0,
     "interestRate": 2.5
   }
   ```
   Response:
   ```json
   {
     "accountNumber": "SAV12345",
     "accountType": "Savings",
     "initialBalance": 1000,
     "overdraftLimit": 0,
     "interestRate": 2.5
   }
   ```

3. **Creating a Credit Account (Type 2)**
   Request:
   ```json
   {
     "accountNumber": "CRE12345",
     "accountType": 2,
     "initialBalance": 0,
     "overdraftLimit": 1000,
     "interestRate": 10
   }
   ```
   Response:
   ```json
   {
     "accountNumber": "CRE12345",
     "accountType": "Credit",
     "initialBalance": 0,
     "overdraftLimit": 1000,
     "interestRate": 10
   }
   ```

---

### **2. Get Account Balance**

**Endpoint:**  
`GET /api/accounts/{accountNumber}/balance`

**Description:**  
This API retrieves the balance of a specific account.

**Request Parameters:**
- `accountNumber` (string): The unique identifier of the account whose balance is being queried.

**Response:**
```json
{
  "accountNumber": "string",    // The account number of the queried account.
  "balance": "number"          // The current balance of the account.
}
```

**Response Parameters:**
- `accountNumber` (string): The account number being queried.
- `balance` (number): The current balance of the account.

**Example:**
Request:
```json
{
  "accountNumber": "SAV12345"
}
```
Response:
```json
{
  "accountNumber": "SAV12345",
  "balance": 1000
}
```

---