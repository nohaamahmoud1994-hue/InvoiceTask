# Invoice Management System

A full-stack Invoice Management System built using ASP.NET Core Web API and Angular.

## Technologies

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- Angular
- TypeScript
- Reactive Forms
- Bootstrap

## Features

- View all invoices
- View invoice details
- Create invoices
- Edit invoices
- Delete invoices
- Invoice validation
- Invoice line management
- Tax and discount calculations
- Server-side validation and calculations

## Project Structure

```text
InvoiceTask
├── Invoice.API
├── Invoice.Core
├── Invoice.Angular
└── README.md
Backend Setup

The backend uses Entity Framework Core Code First with existing migrations.

Configure the SQL Server connection string in:

Invoice.API/appsettings.json

Apply the database migrations:

dotnet ef database update

Run the API using Visual Studio.

API URL:

https://localhost:7082

Swagger:

https://localhost:7082/swagger
Angular Setup

Open a terminal inside:

Invoice.Angular

Install dependencies:

npm install

Run the application:

ng serve

Angular application:

http://localhost:4200
API Configuration

The Angular API URL is configured in:

Invoice.Angular/src/environments/environment.ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:7082/api/InvoiceApi'
};
API Endpoints
Method	Endpoint	Description
GET	/api/InvoiceApi	Get all invoices
GET	/api/InvoiceApi/{id}	Get invoice by ID
POST	/api/InvoiceApi	Create invoice
PUT	/api/InvoiceApi/{id}	Update invoice
DELETE	/api/InvoiceApi/{id}	Delete invoice
Run the Project
Start SQL Server.
Configure the connection string.
Run the database migrations.
Start Invoice.API.
Run Angular using ng serve.
Open http://localhost:4200.
