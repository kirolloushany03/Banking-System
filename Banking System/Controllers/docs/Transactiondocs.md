
### **3. Deposit**

**Endpoint:**  
`POST /api/accounts/deposit`

**Description:**  
This API allows users to deposit a specified amount into an account.

**Request Body:**
```json
{
  "accountNumber": "string",  // The account number where the deposit is made.
  "amount": "number"          // The amount to deposit into the account.
}
```

**Request Parameters:**
- `accountNumber` (string): The unique identifier of the account to which the deposit is being made.
- `amount` (number): The amount to be deposited into the account.

**Response:**
```json
{
  "accountNumber": "string",    // The account number of the account where the deposit was made.
  "balance": "number"          // The updated balance of the account.
}
```

**Response Parameters:**
- `accountNumber` (string): The account number to which the deposit was made.
- `balance` (number): The updated balance after the deposit.

**Example:**
Request:
```json
{
  "accountNumber": "SAV12345",
  "amount": 500
}
```
Response:
```json
{
  "accountNumber": "SAV12345",
  "balance": 1500
}
```

---

### **4. Withdraw**

**Endpoint:**  
`POST /api/accounts/withdraw`

**Description:**  
This API allows users to withdraw a specified amount from an account.

**Request Body:**
```json
{
  "accountNumber": "string",  // The account number from which the withdrawal is made.
  "amount": "number"          // The amount to withdraw from the account.
}
```

**Request Parameters:**
- `accountNumber` (string): The unique identifier of the account from which the withdrawal is made.
- `amount` (number): The amount to be withdrawn from the account.

**Response:**
```json
{
  "accountNumber": "string",    // The account number from which the withdrawal was made.
  "balance": "number"          // The updated balance of the account after the withdrawal.
}
```

**Response Parameters:**
- `accountNumber` (string): The account number from which the withdrawal was made.
- `balance` (number): The updated balance after the withdrawal.

**Example:**
Request:
```json
{
  "accountNumber": "SAV12345",
  "amount": 200
}
```
Response:
```json
{
  "accountNumber": "SAV12345",
  "balance": 1300
}
```

---

### **5. Transfer**

**Endpoint:**  
`POST /api/accounts/transfer`

**Description:**  
This API allows users to transfer a specified amount from one account to another.

**Request Body:**
```json
{
  "sourceAccountNumber": "string",   // The account number from which the money is being transferred.
  "targetAccountNumber": "string",   // The account number to which the money is being transferred.
  "amount": "number"                 // The amount to transfer.
}
```

**Request Parameters:**
- `sourceAccountNumber` (string): The account number from which the transfer is made.
- `targetAccountNumber` (string): The account number to which the transfer is made.
- `amount` (number): The amount to be transferred.

**Response:**
```json
{
  "sourceAccountNumber": "string",   // The source account number from which money was transferred.
  "amount": "number",                // The amount that was transferred.
  "sourceBalance": "number",         // The balance of the source account after the transfer.
  "targetBalance": "number"          // The balance of the target account after the transfer.
}
```

**Response Parameters:**
- `sourceAccountNumber` (string): The source account number from which money was transferred.
- `amount` (number): The amount transferred.
- `sourceBalance` (number): The updated balance of the source account after the transfer.
- `targetBalance` (number): The updated balance of the target account after the transfer.

**Example:**
Request:
```json
{
  "sourceAccountNumber": "SAV12345",
  "targetAccountNumber": "CHK12345",
  "amount": 500
}
```
Response:
```json
{
  "sourceAccountNumber": "SAV12345",
  "amount": 500,
  "sourceBalance": 800,
  "targetBalance": 500
}
```

---

### **6. Transaction**

**Endpoint:**  
`POST /api/accounts/transaction`

**Description:**  
This API logs a transaction related to an account (such as deposit, withdrawal, transfer, etc.).

**Request Body:**
```json
{
  "accountId": "integer",       // The ID of the account to which the transaction belongs.
  "amount": "number",           // The amount involved in the transaction.
  "transactionType": "integer", // The type of transaction (Deposit, Withdraw, etc.).
  "timestamp": "string"         // The timestamp when the transaction occurred.
}
```

**Request Parameters:**
- `accountId` (integer): The unique identifier of the account involved in the transaction.
- `amount` (number): The amount involved in the transaction.
- `transactionType` (integer): The type of transaction. Possible values could be:
  - `0`: Deposit
  - `1`: Withdraw
  - `2`: Transfer
- `timestamp` (string): The timestamp when the transaction occurred, in ISO 8601 format.

**Response:**
```json
{
  "transactionId": "string",    // The unique identifier of the transaction.
  "accountId": "integer",       // The account ID involved in the transaction.
  "amount": "number",           // The amount involved in the transaction.
  "transactionType": "integer", // The type of transaction.
  "timestamp": "string"         // The timestamp of the transaction.
}
```

**Response Parameters:**
- `transactionId` (string): The unique identifier of the transaction.
- `accountId` (integer): The account ID involved in the transaction.
- `amount` (number): The amount involved in the transaction.
- `transactionType` (integer): The type of transaction.
- `timestamp` (string): The timestamp of the transaction.

**Example:**
Request:
```json
{
  "accountId": 1,
  "amount": 500,
  "transactionType": 0,
  "timestamp": "2024-12-29T10:15:00Z"
}
```
Response:
```json
{
  "transactionId": "TXN12345",
  "accountId": 1,
  "amount": 500,
  "transactionType": 0,
  "timestamp": "2024-12-29T10:15:00Z"
}
```

---

### **7. Get Transaction by ID**

**Endpoint:**  
`GET /api/accounts/transaction/{id}`

**Description:**  
This API retrieves the details of a specific transaction by its ID.

**Request Parameters:**
- `id` (integer): The unique identifier of the transaction.

**Response:**
```json
{
  "transactionId": "string",    // The unique identifier of the transaction.
  "accountId": "integer",       // The account ID involved in the transaction.
  "amount": "number",           // The amount involved in the transaction.
  "transactionType": "integer", // The type of transaction.
  "timestamp": "string"         // The timestamp of the transaction.
}
```

**Response Parameters:**
- `transactionId` (string): The unique identifier of the transaction.
- `accountId` (integer): The account ID involved in the transaction.
- `amount` (number): The amount involved in the transaction.
- `transactionType` (integer): The type of transaction.
- `timestamp` (string): The timestamp of the transaction.

**Example:**
Request:
```json
{
  "id": 12345
}
```
Response:
```json
{
  "transactionId": "TXN12345",
  "accountId": 1,
  "amount": 500,
  "transactionType": 0,
  "timestamp": "2024-12-29T10:15:00Z"
}
```

---