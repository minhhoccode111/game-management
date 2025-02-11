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

Build the Docker Image

```bash
docker build -t game-management .
```

Install MSSQL Server Image

```bash
docker pull mcr.microsoft.com/mssql/server:2022-latest
```

Run the MSSQL Server Container

```bash
# escape the password because it contains `!`, which is a special character in shell
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=P@ssw0rd" \
   -p 1433:1433 --name sql_server_container \
   -d mcr.microsoft.com/mssql/server:2022-latest
```

Run the application container

```bash
docker run -p 8080:80 --name game-management \
   --link sql_server_container \
   -e "ConnectionStrings__DefaultConnection=Server=sql_server_container;Database=GameManagement;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;" \
   game-management
```

Access the application at `http://localhost:8080` to check the app
