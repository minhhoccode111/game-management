# Game Management App Mvc

ASP.NET Core MVC

## Screenshot

<details>
    <summary>Click me</summary>
</details>

## [Demo](https://github.com/minhhoccode111)

## Features

- CRUD

## Outcome

- C#
- MVC
- Razor
- SQLite
- EF Core
- Bootstrap
- ASP.NET Core
- ...

## To-do

- Use MS SQL instead of SQLite
- Upload images instead of hard coded links
  - Or validate if an [image link is valid](https://stackoverflow.com/questions/11082804/detecting-image-url-in-c-net)
- Add `Sort by`: Rating, Name, Date, etc.
- Deploy

## Getting started

Clone the repo

```bash
git clone git@github.com:minhhoccode111/GameManagementMvc.git
```

Run

```bash
dotnet restore
dotnet run
# or develop
# dotnet watch run
```

## Design choices and tradeoffs

<!---->
<!-- - Avoid using a `ICollection<>` field in Models for scalability and use ViewModels to pass needed data to Views instead -->
<!---->
<!-- Example connection between Game and Genre is many-to-many -->
<!---->
<!-- ```csharp -->
<!-- // Bad -->
<!-- public class Game -->
<!-- { -->
<!--     // ... fields -->
<!-- } -->
<!-- public class Genre -->
<!-- { -->
<!--     // ... fields -->
<!--     public ICollection<Game>? Games {get; set;} -->
<!-- } -->
<!-- // Good: Because the likelihood that a Game Model has infinite genres is low -->
<!-- public class Game -->
<!-- { -->
<!--     // ... fields -->
<!--     public ICollection<Genre>? Genres {get; set;} -->
<!-- } -->
<!-- public class Genre -->
<!-- { -->
<!--     // ... fields -->
<!-- } -->
<!-- ``` -->
<!---->
<!-- Similarly, connection between Company and Game is one-to-many -->
<!---->
<!-- ```csharp -->
<!-- // Bad -->
<!-- public class Game -->
<!-- { -->
<!--     // ... fields -->
<!-- } -->
<!-- public class Company -->
<!-- { -->
<!--     // ... fields -->
<!--     public ICollection<Game>? Games {get; set;} -->
<!-- } -->
<!-- // Good -->
<!-- public class Game -->
<!-- { -->
<!--     // ... fields -->
<!--     public Company Company {get; set;} -->
<!-- } -->
<!-- public class Company -->
<!-- { -->
<!--     // ... fields -->
<!-- } -->
<!-- ``` -->
