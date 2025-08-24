# Currency Converter API

## Setup Instructions

1. Clone the repo
2. Run `docker-compose up` to start SQL Server
3. Update `appsettings.json` with DB connection string
4. Run migrations: `dotnet ef database update`
5. Start the API: `dotnet run`

## Endpoints

- `GET /api/currency/rates`
- `POST /api/currency/convert`
- `GET /api/currency/conversions?currency=USD&from=2023-01-01&to=2023-01-31`
