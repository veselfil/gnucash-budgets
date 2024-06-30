<!-- TOC -->
* [Design](#design)
  * [Cloud vs Localhost](#cloud-vs-localhost)
    * [1. Running on Cloud](#1-running-on-cloud)
      * [This approach would mean](#this-approach-would-mean)
      * [Benefits](#benefits)
      * [Cons](#cons)
    * [2. Running on Localhost](#2-running-on-localhost)
      * [This approach would mean](#this-approach-would-mean-1)
      * [Benefits](#benefits-1)
      * [Cons](#cons-1)
    * [Summary](#summary)
      * [How to overcome a problem with accessing the app from a mobile device](#how-to-overcome-a-problem-with-accessing-the-app-from-a-mobile-device)
  * [REST vs push/pull model](#rest-vs-pushpull-model)
    * [1. REST api](#1-rest-api)
      * [This approach would mean](#this-approach-would-mean-2)
      * [Benefits](#benefits-2)
      * [Cons](#cons-2)
    * [2. Push/Pull model](#2-pushpull-model)
      * [This approach would mean](#this-approach-would-mean-3)
      * [Benefits](#benefits-3)
      * [Cons](#cons-3)
    * [Summary](#summary-1)
  * [Summary of design](#summary-of-design)
* [Connector API](#connector-api)
  * [Adapters](#adapters)
  * [Specification](#specification)
    * [Authentication and Authorization:](#authentication-and-authorization)
    * [Data Format:](#data-format)
    * [Error Handling:](#error-handling)
    * [Endpoints and their Descriptions:](#endpoints-and-their-descriptions)
      * [/expenses/history](#expenseshistory)
        * [Request Parameters:](#request-parameters)
        * [API Responses:](#api-responses)
          * [Successful response (Status code 200):](#successful-response-status-code-200)
        * [Error responses (Status code 4xx and 5xx):](#error-responses-status-code-4xx-and-5xx)
    * [Inputs](#inputs)
    * [Outputs](#outputs)
<!-- TOC -->

# Design

First we need to look at the system design before we can talk about adapters and how to fetch expenses data.

GnuCashBudget Generator exists only for testing purposes. It generates expenses at random and for the purposes of design
could be ignored

Summary of design [here](#summary-of-design)

## Cloud vs Localhost

We have two sensible ways how to look at our system - it will run on [Cloud](#1-running-on-cloud) or [Locally](#2-running-on-localhost)

### 1. Running on Cloud

![Cloud schema](connector-api_cloud-schema.png)

#### This approach would mean

* Having User Management (log in, etc.)
  * Or using Google OAuth Login **only** and not have own user management
* Connecting Google Drive in which the GNUCash.sqlite file is saved
  * This would further mean have some checks in place to know if the file was edited (user can download a file, do some changes to Assets and upload it back)
  * Implementing Google OAuth
* When an adapter would need permissions to read mail (bank doesn't have an API &rarr; we parse it from the mail) we need to ask user for that and store it somewhere

#### Benefits
* Flexibility of using the app from everywhere
  * Even on phone (you buy something &rarr; expense is read and put in queue in app &rarr; you can immediately categorize it)

#### Cons
* Devs need to implement more stuff and it is more complicated
* User's data (and all expenses, assets) are uploaded to "somewhere"
* It would cost more money (deployments, managed databases)

### 2. Running on Localhost

![Local schema](connector-api_local-schema.png)

#### This approach would mean

* No User Management
* There is no need to connect Google Drive
  * You can install a plugin to your desktop and read the file from filesystem
* Mail user/pass is saved in appsettings/local secrets and read directly

#### Benefits
* Easier for the devs to implement
* User has all data locally
* Cheaper

#### Cons
* Initial setup
  * User needs to run Docker, setup variables and have file in filesystem
* No access from mobile devices (when you buy something you need to go to your desktop to categorize the expense)

### Summary

After careful consideration I am recommending to target localhost for now because of the overall simplicity for developers.
In cloud approach we would need to set up user management, authentication and authorization and pay for cloud infrastructure.
Also, some users (this is an open source project) wouldn't like that their entire GNUcash file is uploaded somewhere
(they have their assets and whole financials in a file). Only thing we cannot do when we use localhost approach is we cannot
access the app from mobiles. This will be painful if let's say user is in shop, pays for something and wants to categorize it right then and there.
In localhost approach user has his file on disk whole time (with Google Drive plugin it automatically synchronizes to users cloud)
and after initial set up its easy for him to use the app

#### How to overcome a problem with accessing the app from a mobile device

We can create an adapter just for categorizing expenses from mobile device. This adapter would be connected to a mail address.
When user starts the app on localhost this adapter will read the mails and present them to a user as expenses. These expenses
will be already categorized by the user (all needed data are there) and he will only "click" OK button and expenses will
be written to the database as categorized

We would need to provide some webpage with a form which a user would fill. Then it would send a mail to specific email
address which is connected to the adapter. It doesn't matter if we create our own webpage and deploy it, or we use 
Google Forms for instance

Basically approach would be: have Google Form (webpage) &rarr; user fills a form as an expense &rarr; mail is sent to 
an email address &rarr; user starts up the app on localhost &rarr; adapter reads mails as expenses and presents them to
the user &rarr; user clicks OK and expenses are categorized

## REST vs push/pull model

We also need to look at a way how will adapters communicate with main app

We have two sensible ways how to do it - via [REST](#1-rest-api) or [PUSH/PULL model](#2-pushpull-model)

### 1. REST api

![REST schema](connector-api_rest-schema.png)

#### This approach would mean

* Main app development logic
  * All transactions which are acquired from adapter service will be put in some database table with un-categorized expense state
  and when user wants to see un-categorized expenses just show him expenses with corresponding state
  * When user categorizes an expense it is moved from un-categorized state to categorized one
  * Main application only gets new data (from adapters) based on continuation-token logic (older and un-categorized data 
  are already in the database table from earlier)
* Implementing continuation-token in adapter services
  * When there isn't a token &rarr; adapter sends back all data
  * When there is a token &rarr; adapter sends back only part of data based on the input in continuation-token

#### Benefits

* Simpler initial setup of adapter services
  * Read the data (either all or based on continuation token) &rarr; send them to the caller
* Much less complexity without push/pull provider

#### Cons

* Implementing continuation-tokens and special table to remember all expenses (and not having it in messaging queue)
  * **This is also a good thing because we will only have 1 database for the whole app**

### 2. Push/Pull model

![PUSHPULL schema](connector-api_pushpull-schema.png)

#### This approach would mean

* Having to setup messaging provider
  * Adding it to the Dockerfile
  * Queue needs to be durable and persistent
    * When we quit the whole application (docker) we need to be **sure** the messages added to the queue doesn't disappear
* Having Queue already in place for later usage (showing the user the un-categorized expenses)
* Implementing simple database to remember last datetime of expense data sent to queue
  * Only expenses which weren't put into the queue can be put there
* We would need to trigger somehow the adapter to read and send all new expenses to the messaging provider

#### Benefits

* Simple development on the main app part
  * When adapter does its work the main app only reads a message from queue and shows it to the user
  * If user does not categorize it the message will remain in the queue until he does (via manual acknowledgements)
 
#### Cons

* Push/Pull provider needs to support DurableQueues and Persistent messages (along with manual acknowledgements)
  * Probably save data to the file-storage &rarr; setting docker correctly to allow it
* Using (simple) database table in adapter services to remember datetime of last expense put into the queue
* Complexity
  * There would be many places in which we need to save current state (database in adapter, messaging queue, databases
  in the main app)
  * Potentially we could lose data if there is some problem

### Summary

After careful consideration I am recommending to use REST model because of the overall simplicity for developers.
Adapter services will be developed easier for REST model (no database in rest model, only continuation-token).
Main application will be easier to develop with REST model because we save the data from adapters in database table,
save continuation-token and next time we are calling an adapter we use that token, and we get only new data.
Then the main app shows a user only expenses with un-categorized state from that table.


## Summary of design

Application will be used only on **localhost** ([Explanation here](#summary)) and be using **REST** ([Explanation for that here](#summary-1))

![Design schema](connector-api_design-schema.png)

# Connector API

We are going to integrate with many banks. Some of them have their own API, some of them don't.

For those who don't we will turn on sending mails to our inbox and then parse all the mails (with all transactions)

![Connector API schema](connector-api_schema.png)

## Adapters

* Adapters are going to work via REST API
* Every bank adapter will have standalone service

## Specification

This document outlines the interface and functionalities of the Bank Connector API,
tailored specifically for retrieving expense data within our custom banking application.
It enables seamless integration with the bank's existing API for fetching expense-related information.

### Authentication and Authorization:

We won't implement authentication and authorization because we plan to run the services only locally via Docker

### Data Format:

The bank's API returns all data in `JSON` format,
containing details such as `amount`, `currency`, `category`, `description`, `timestamp` and `adapter_name`.

### Error Handling:

Developers should handle potential errors gracefully by checking for error responses and displaying relevant messages to users.
Possible error codes include `4xx` client errors (e.g., invalid parameters) and `5xx` server errors.

### Endpoints and their Descriptions:

#### /expenses/history

* Endpoint: `/expenses/history`
* Method: `GET`
* Description: This endpoint retrieves the history of expense transactions for the user's account.

##### Request Parameters:

Optional parameters:

* `continuation_token`: Specifies the continuation-token which could be parsed to specific value by which adapter knows
  from which point in time it needs to send data.
  * Data Type: `String (Base64)`
  * Example: `"continuation_token": "base64value"`
  * Description: This parameter allows filtering expense transactions based on their transaction dates.
  * If provided, only transactions that occurred on or after the specified continuation token value will be included in the response. 
  * If omitted, transactions from any date will be included in the response.
* `currency`: Specifies the currency in which the expense transactions should be retrieved.
  * Data Type: `String`
  * Example: `"currency": "USD"`
  * Description: This parameter allows filtering expense transactions based on the currency used for the transactions. 
  * If provided, only transactions in the specified currency will be included in the response.
  * If omitted, expense transactions in all currencies will be included in the response.

##### API Responses:

###### Successful response (Status code 2xx):

* The response contains
  * a continuation-token string in Base64
  * an array of expense objects, each representing a single transaction.
    * Each expense object includes fields like `amount`, `currency`, `timestamp` and `adapter_name`.

| Name             | Nullable | Data Type                | Description                                                                                                |
|------------------|----------|--------------------------|------------------------------------------------------------------------------------------------------------|
| `amount`         | No       | Decimal                  | The amount of the transaction spent                                                                        |
| `currency`       | No       | String                   | The currency in which the transaction was made (e.g., USD, EUR)                                            |
| `category`       | Yes      | String                   | The category to which the expense belongs (e.g., groceries, housing, transportation)                       |
| `description`    | Yes      | String                   | A description of the transaction, providing additional information about what the transaction was made for |
| `timestamp`      | No       | String (ISO 8601 format) | The timestamp of the transaction, indicating the date and time when the transaction occurred               |
| `merchant`       | Yes      | String                   | The name or identifier of the merchant where the transaction was made                                      |
| `payment_method` | Yes      | String                   | The payment method used for the transaction (e.g., debit card, credit card, bank transfer)                 |
| `location`       | Yes      | String                   | The location where the transaction was made (e.g., merchant's address or GPS coordinates)                  |
| `adapter_name`   | No       | String                   | The name of the adapter so when categorizing we know from which source this expense is                     |

```json
{
  "continuation_token": "base64value",
  "expenses": [
    {
      "amount": 50.00,
      "currency": "USD",
      "category": "Groceries",
      "description": "Grocery shopping for the week",
      "timestamp": "2024-04-02T12:00:00Z",
      "merchant": "ABC Supermarket",
      "payment_method": "Debit Card",
      "location": "123 Main St, City, Country",
      "adapter_name": "Adapter 1"
    },
    {
      "amount": 20.00,
      "currency": "USD",
      "category": null,
      "description": null,
      "timestamp": "2024-03-30T13:30:00Z",
      "merchant": "XYZ Restaurant",
      "payment_method": "Credit Card",
      "location": "456 Elm St, City, Country",
      "adapter_name": "Adapter 1"
    }
  ]
}

```

##### Error responses (Status code 4xx and 5xx):

* Error messages and corresponding status codes will be returned in case of invalid requests or server errors.

```json
{
  "error": {
    "code": 400,
    "message": "Invalid date format provided"
  }
}

```

### Inputs

Inputs for each adapter are data got from the bank (either via Bank API or mail parsing/etc.)

To get only new data adapters need to implement continuation-token. They will get it in GET request and use it
to respond only with new data. Continuation-token could be as simple as `DateTime` value in Base64 format

For Bank API you can save the value and send REST request to your Bank API to only get expenses newer than <datetime value>

For Mail parser you can parse mails from the TOP and when you find mail older than <datetime value> you stop parsing.

### Outputs

Outputs of each adapter are data send as a Response to GET request

Either adapter sends all data back (no continuation-token in a request) or sends only new data back (continuation-token
was present in a request)

In every response there needs to be a continuation-token generated by adapter so the main app can save it and use it later

![Flow schema](connector-api_flow.png)
