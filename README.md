# Dotnet WebAPI + EF demo

## _Description_

.Net Core 6 WebAPI + EntityFramwork的RESTful API範例

## _Tech_

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [EF Core](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore)

## _Docker_

```sh
docker-compose up -d
```

debug mode測試，可以只run db container

```sh
docker compose -f docker-compose-mssql.yml up -d

dotnet run
```

## _API Port_

```sh
5000:5000
```

## _Mssql Port_

```sh
1433:1433
```

## _功能說明_

- Product、Order、Customer的CRUD
