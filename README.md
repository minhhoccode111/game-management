# Game Management App MVC

Learn ASP.NET Core MVC

## Screenshots

<details>
    <summary>Click me</summary>
</details>

## [Demo]()

## Features

- CRUD

## Outcome

- Used C#
- Used MVC
- Used Razor
- Used SQLite
- Used EF Core
- Used .NET Core
- Used Bootstrap
- ...

## To-do

- Use MS SQL instead of SQLite
- Upload images instead of hard coded links
  - Or validate if an [image link is valid](https://stackoverflow.com/questions/11082804/detecting-image-url-in-c-net)

## Getting started

Clone the repo

```bash
git clone git@github.com:minhhoccode111/game-management.git
```

Run

```bash
dotnet restore
dotnet run
# or develop
# dotnet watch run
```

## Design choices and tradeoffs

- Used `[NotMapped]` for field like `Company` in `Game` model and manually populate that field when we need it
  - More code complexity
  - Less database size
  - Can be combined with `[Bind(...)] Game` to handle form submit easier
  - Less performance
