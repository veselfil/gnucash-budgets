# GnuCashBudget.GnuCashData.Generator

As an architecture of "workers" I used is from [this microsoft guide](https://learn.microsoft.com/en-us/dotnet/core/extensions/scoped-service)

I needed to use `Scoped` services with `BackgroundService` because underlying services are using Scoped services.
For the purposes of this simple Console Application it is better to use Scoped then to change all to Transient

## How the Generator works

1. First it gets ROOT account
2. Then it gets 1 INCOME, 1 BANK and 1 EXPENSE account (main ones) - if they don't exist generator creates them
3. Then it creates (and always creates) 1 child INCOME, 1 child BANK and N children EXPENSE accounts
4. All Incomes and Expenses are generated based on the settings from `GeneratorOptions`
5. To INCOME and BANK account the "Salary/Income" transactions are created and paired (1 transaction)
6. to BANK and INCOME accounts the "Expenses" transactions are created and paired (N transactions)

SQL description from [GNUCASH website](https://wiki.gnucash.org/wiki/SQL)

### Account Creation

To create an account we need to add `AccountEntity` to `accounts` table in GNUCASH SQLITE database

As a Commodity we are using a commodity from ROOT account (if the root account is in USD, all other accounts will be in USD)

```sqlite
CREATE TABLE accounts (
    guid            CHAR(32) PRIMARY KEY NOT NULL,
    name            text(2048) NOT NULL,
    account_type    text(2048) NOT NULL,
    commodity_guid  CHAR(32) NOT NULL,
    commodity_scu   integer NOT NULL,
    non_std_scu     integer NOT NULL,
    parent_guid     CHAR(32),
    code            text(2048),
    description     text(2048),
    hidden          integer NOT NULL,
    placeholder     integer NOT NULL
);
```

### Transaction Creation

To create a transaction we need to do these things:

1. Add a row to `transactions` table via `TransactionEntity`
2. and add 2 rows to `splits` table  via `SplitEntity` with `transaction.Id`. First for the `CREDIT` part and second for the `DEBIT` part

`DEBIT=`we take something from an account

`CREDIT=`we give something to an account

When we take something from an account (debit) we want to use `-` operator before number in `value_num`

When we give something to an account (credit) we want to use `+` operator before number in `value_num`

* Income transaction means creating `-INCOME` and `+BANK` rows in `splits` table
* Expense transaction means creating `-BANK` and `+EXPENSE` rows in `splits` table


```sqlite
CREATE TABLE transactions (
    guid            CHAR(32) PRIMARY KEY NOT NULL,
    currency_guid   CHAR(32) NOT NULL,
    num             text(2048) NOT NULL,
    post_date       timestamp NOT NULL,
    enter_date      timestamp NOT NULL,
    description     text(2048)
);
```

```sqlite
CREATE TABLE splits (
    guid            CHAR(32) PRIMARY KEY NOT NULL,
    tx_guid         CHAR(32) NOT NULL,
    account_guid    CHAR(32) NOT NULL,
    memo            text(2048) NOT NULL,
    action          text(2048) NOT NULL,
    reconcile_state text(1) NOT NULL,
    reconcile_date  timestamp NOT NULL,
    value_num       integer NOT NULL,
    value_denom     integer NOT NULL,
    quantity_num    integer NOT NULL,
    quantity_denom  integer NOT NULL,
    lot_guid        CHAR(32)
);
```