IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624063433_InitialCreate')
BEGIN
    CREATE TABLE [BenchMarkResult] (
        [Id] int NOT NULL IDENTITY,
        [CloudProvider] int NOT NULL,
        [HostingEnvironment] int NOT NULL,
        [Runtime] int NOT NULL,
        [Success] bit NOT NULL,
        [RequestDuration] int NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL DEFAULT ((getdate())),
        [IsColdRequest] bit NOT NULL,
        CONSTRAINT [BenchMarkResults_pk] PRIMARY KEY ([Id])
    );
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624063433_InitialCreate')
BEGIN
    CREATE INDEX [BenchMarkResults__CreatedAt_index] ON [BenchMarkResult] ([CreatedAt]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624063433_InitialCreate')
BEGIN
    CREATE UNIQUE INDEX [BenchMarkResults_Id_uindex] ON [BenchMarkResult] ([Id]);
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624063433_InitialCreate')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200624063433_InitialCreate', N'3.1.5');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624075128_Adding_FunctionVersion')
BEGIN
    ALTER TABLE [BenchMarkResult] ADD [FunctionVersion] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624075128_Adding_FunctionVersion')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200624075128_Adding_FunctionVersion', N'3.1.5');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624112643_AddedRunPosition')
BEGIN
    ALTER TABLE [BenchMarkResult] ADD [CallPosition] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624112643_AddedRunPosition')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200624112643_AddedRunPosition', N'3.1.5');
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624134358_AddedRunPositionNumber_ToTestNewDesignFactory')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[BenchMarkResult]') AND [c].[name] = N'CallPosition');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [BenchMarkResult] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [BenchMarkResult] DROP COLUMN [CallPosition];
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624134358_AddedRunPositionNumber_ToTestNewDesignFactory')
BEGIN
    ALTER TABLE [BenchMarkResult] ADD [CallPositionNumber] int NOT NULL DEFAULT 0;
END;

GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20200624134358_AddedRunPositionNumber_ToTestNewDesignFactory')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20200624134358_AddedRunPositionNumber_ToTestNewDesignFactory', N'3.1.5');
END;

GO

