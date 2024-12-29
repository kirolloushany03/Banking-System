# Banking System API

## Overview

The **Banking System API** is a RESTful application designed to provide basic banking functionalities such as account management, transactions, deposits, withdrawals, and transfers. It includes features to track transactions, manage account balances, and calculate interest rates.

This API provides various endpoints for performing actions like account creation, transaction history, and handling deposits and withdrawals.

## API Documentation

For detailed documentation about the API endpoints, visit the [Controllers Documentation](https://github.com/kirolloushany03/Banking-System/tree/master/Banking%20System/Controllers/docs).

### Key Endpoints

1. **Create Account**  
   - `POST /api/accounts`
   - Create a new account with parameters like account number, type, balance, etc.

2. **Get Account Balance**  
   - `GET /api/accounts/{accountNumber}/balance`
   - Retrieve the balance for a specified account.

3. **Deposit Money**  
   - `POST /api/accounts/deposit`
   - Deposit funds into an account.

4. **Withdraw Money**  
   - `POST /api/accounts/withdraw`
   - Withdraw funds from an account.

5. **Transfer Funds**  
   - `POST /api/accounts/transfer`
   - Transfer funds between two accounts.

6. **Log Transaction**  
   - `POST /api/accounts/transaction`
   - Record a transaction (deposit, withdrawal, transfer, etc.).

7. **Get Transaction by ID**  
   - `GET /api/accounts/transaction/{id}`
   - Fetch details of a transaction by its ID.

## Folder Structure

```
/Banking System
|-- /Controllers          # Documentation for the Account and Transaction controllers
|-- /Data                 # Making the DataContext and setting up Entity Framework
|-- /Dtos                 # Creating DTOs for Account and Transaction
|-- /Entities             # Entities for Account and Transaction
|-- /Mapping              # Mapping for DTOs
|-- /Migrations           # Database migrations
|-- /Properties           # Project files and settings
|-- /Services             # Services related to Account and Transaction operations
|-- CronScheduler.cs      # Scheduling for interest rate calculations
|-- Program.cs            # DataContext setup and Entity Framework configuration
|-- appsettings.json      # Configuration for the project
```
---
