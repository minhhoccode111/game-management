IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Company] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(64) NOT NULL,
    [Body] nvarchar(2048) NOT NULL,
    [Image] nvarchar(2048) NULL,
    [FoundingDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Company] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Genre] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(32) NOT NULL,
    [Body] nvarchar(2048) NOT NULL,
    CONSTRAINT [PK_Genre] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Game] (
    [Id] int NOT NULL IDENTITY,
    [CompanyId] int NOT NULL,
    [GenreIds] nvarchar(max) NULL,
    [Title] nvarchar(64) NOT NULL,
    [Body] nvarchar(2048) NOT NULL,
    [Image] nvarchar(2048) NULL,
    [Rating] int NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Game] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Game_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [GameGenre] (
    [GameId] int NOT NULL,
    [GenreId] int NOT NULL,
    CONSTRAINT [PK_GameGenre] PRIMARY KEY ([GameId], [GenreId]),
    CONSTRAINT [FK_GameGenre_Game_GameId] FOREIGN KEY ([GameId]) REFERENCES [Game] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GameGenre_Genre_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genre] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Game_CompanyId] ON [Game] ([CompanyId]);
GO

CREATE INDEX [IX_GameGenre_GenreId] ON [GameGenre] ([GenreId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240813024750_SqlServerMigration', N'8.0.7');
GO

COMMIT;
GO

