# Notes

<!--FIX: this docs-->

## Database design

- Company
  - ID
  - Title
  - Body
  - Image
  - Games
  - Founding Date
- Game
  - ID
  - Title
  - Body
  - Image
  - Rating
  - Release Date
  - Company
  - CompanyId
  - Genres
  - GenreIds
- Genre
  - ID
  - Title
  - Body
  - Games

## App flow

- layout header
  - title
  - home
  - game
  - company
  - genre
  - about
  - privacy
- home
  - index
  - about
  - privacy
- games
  - create new
  - filter - drop down select company - drop down select genre - drop down select rating - drop down select sort by - search bar game title
  - table - info preview - title - rating - release date - company - genres - links - to a genre detail - to a company detail - to a game detail - to edit game - to delete game
- companies
  - create new
  - filter - drop down select sort by - search bar company title
  - table - info preview - title - founding date - games - links - to a game detail - to a company detail - to edit company - to delete company
- genres
  - create new
  - filter - drop down select sort by - search bar genre title
  - table - info preview - title - games - links - to a game detail - to a genre detail - to edit genre - to delete genre
- game
  - create - form
  - detail - info
  - edit - form
  - delete - confirm
- company
  - create - form
  - detail - info - games created by this company
  - edit - form - games created by this company
  - delete - confirm - remove games created by this company before deleting
- genres
  - create - form
  - detail - info - games created with this genre
  - edit - form - games created with this genre
  - delete - confirm - remove games created with this genre before deleting

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
dotnet aspnet-codegenerator controller -name GameController -m Game -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
dotnet aspnet-codegenerator controller -name CompanyController -m Company -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
dotnet aspnet-codegenerator controller -name GenreController -m Genre -dc GameManagementMvc.Data.GameManagementMvcContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries --databaseProvider sqlite
```

## Entity Framework Core (ORM)

Create and update a database to match the data models

```bash
dotnet ef migrations add SqlServerMigration
dotnet ef database update
```

## Embeded the variables in the View

```csharp
MultiSelectList companies = await GetAllCompaniesSelectList();
ViewData["Companies"] = JsonConvert.SerializeObject(companies);
```

```js
const someData = @Html.Raw(ViewData["Companies"]);
```
