# Notes

## Database design

- Game
  - Id
  - Title
  - Body
  - Image
  - Rating
  - Release Date
  - GameCompanies
  - GameGenres
- GameCompany
  - Id
  - GameId
  - CompanyId
  - Title
  - Body
  - StartDate
  - EndDate
  - Game
  - Company
- Company
  - Id
  - Title
  - Body
  - Image
  - Founding Date
  - GameCompanies
- GameGenre
  - GameId
  - GenreId
  - Game
  - Genre
- Genre
  - Id
  - Title
  - Body
  - GameGenres

## Create a mvc project use CLI

```bash
dotnet new mvc -n ProjectName -o ProjectName
dotnet new gitignore
```

## Add NuGet packages

```bash
dotnet tool uninstall --global dotnet-aspnet-codegenerator
dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SQLite
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

## Generate controller based on Models (game, company, genre)

```bash
dotnet aspnet-codegenerator controller -name GameController -m Game -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider mssql-server -f
dotnet aspnet-codegenerator controller -name CompanyController -m Company -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider mssql-server -f
dotnet aspnet-codegenerator controller -name GenreController -m Genre -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider mssql-server -f
```

## Migration

```bash
dotnet ef migrations add SqlServerMigration
dotnet ef database update
```

## Create a mvc project use CLI

```bash
dotnet new mvc -n ProjectName -o ProjectName
dotnet new gitignore
```

## Generate a SQL Schema Script

```bash
dotnet ef migrations script -o migration.sql
```

## Pass data to client

Pass data in .Net Core

```csharp
ViewData["Companies"] = JsonConvert.SerializeObject(companies);
```

Extract data in JavaScript

```js
const someData = @Html.Raw(ViewData["Companies"]);
```
