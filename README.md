# Game Management MVC

Try ASP.NET Core MVC

## Features

- CRUD

## To-do

- Upload images instead of hard coded links
  - Or validate if an [image link is valid](https://stackoverflow.com/questions/11082804/detecting-image-url-in-c-net)

## Getting started

Clone the repo

```bash
git clone git@github.com:minhhoccode111/game-management.git
```

Install packages

```bash
dotnet restore
```

Install Microsoft SQL Server Docker (optional)

```bash
# pull image
docker pull mcr.microsoft.com/mssql/server:2022-latest
# run container
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourStrong!Passw0rd" \
   -p 1433:1433 --name sql_server_container \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

Migration

```bash
dotnet ef migrations add SqlServerMigration
dotnet ef database update
```

Run

```bash
dotnet run
```
