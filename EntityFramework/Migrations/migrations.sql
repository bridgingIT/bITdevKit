IF OBJECT_ID(N'[core].[__MigrationsHistory]') IS NULL
BEGIN
    IF SCHEMA_ID(N'core') IS NULL EXEC(N'CREATE SCHEMA [core];');
    CREATE TABLE [core].[__MigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___MigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    IF SCHEMA_ID(N'core') IS NULL EXEC(N'CREATE SCHEMA [core];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[__Outbox_DomainEvents] (
        [Id] uniqueidentifier NOT NULL,
        [EventId] nvarchar(256) NOT NULL,
        [Type] nvarchar(2048) NOT NULL,
        [Content] nvarchar(max) NULL,
        [ContentHash] nvarchar(64) NULL,
        [CreatedDate] datetimeoffset NOT NULL,
        [ProcessedDate] datetimeoffset NULL,
        [Properties] nvarchar(max) NULL,
        [RowVersion] rowversion NULL,
        CONSTRAINT [PK___Outbox_DomainEvents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[__Outbox_Messages] (
        [Id] uniqueidentifier NOT NULL,
        [MessageId] nvarchar(256) NOT NULL,
        [Type] nvarchar(2048) NOT NULL,
        [Content] nvarchar(max) NULL,
        [ContentHash] nvarchar(64) NULL,
        [CreatedDate] datetimeoffset NOT NULL,
        [ProcessedDate] datetimeoffset NULL,
        [Properties] nvarchar(max) NULL,
        [RowVersion] rowversion NULL,
        CONSTRAINT [PK___Outbox_Messages] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[__Storage_Documents] (
        [Id] uniqueidentifier NOT NULL,
        [Type] nvarchar(1024) NOT NULL,
        [PartitionKey] nvarchar(512) NOT NULL,
        [RowKey] nvarchar(512) NOT NULL,
        [Content] nvarchar(max) NULL,
        [ContentHash] nvarchar(64) NULL,
        [CreatedDate] datetimeoffset NOT NULL,
        [UpdatedDate] datetimeoffset NULL,
        [Properties] nvarchar(max) NULL,
        [RowVersion] rowversion NULL,
        CONSTRAINT [PK___Storage_Documents] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Bills] (
        [Id] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NULL,
        [DinnerId] uniqueidentifier NULL,
        [GuestId] uniqueidentifier NULL,
        [Price_Amount] decimal(5,2) NULL,
        [Price_Currency] nvarchar(8) NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Bills] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Dinners] (
        [Id] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NULL,
        [MenuId] uniqueidentifier NULL,
        [Price_Amount] decimal(5,2) NULL,
        [Price_Currency] nvarchar(max) NULL,
        [Location_Name] nvarchar(max) NULL,
        [Location_AddressLine1] nvarchar(256) NULL,
        [Location_AddressLine2] nvarchar(256) NULL,
        [Location_PostalCode] nvarchar(16) NULL,
        [Location_City] nvarchar(128) NULL,
        [Location_Country] nvarchar(128) NULL,
        [Location_WebsiteUrl] nvarchar(max) NULL,
        [Location_Latitude] decimal(10,7) NULL,
        [Location_Longitude] decimal(10,7) NULL,
        [Status] int NULL,
        [Name] nvarchar(128) NOT NULL,
        [Description] nvarchar(512) NOT NULL,
        [Schedule_StartDateTime] datetimeoffset NULL,
        [Schedule_EndDateTime] datetimeoffset NULL,
        [StartedDateTime] datetimeoffset NULL,
        [EndedDateTime] datetimeoffset NULL,
        [ImageUrl] nvarchar(max) NULL,
        [IsPublic] bit NOT NULL,
        [MaxGuests] int NOT NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Dinners] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Guests] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [FirstName] nvarchar(128) NOT NULL,
        [LastName] nvarchar(128) NOT NULL,
        [ProfileImage] nvarchar(max) NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Guests] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Hosts] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [AverageRating_Value] float NULL,
        [AverageRating_NumRatings] int NULL,
        [FirstName] nvarchar(128) NOT NULL,
        [LastName] nvarchar(128) NOT NULL,
        [ProfileImage] nvarchar(max) NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Hosts] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[MenuReviews] (
        [Id] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NULL,
        [DinnerId] uniqueidentifier NULL,
        [MenuId] uniqueidentifier NULL,
        [GuestId] uniqueidentifier NULL,
        [Rating_Value] int NULL,
        [Comment] nvarchar(512) NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_MenuReviews] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Menus] (
        [Id] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NULL,
        [Name] nvarchar(128) NOT NULL,
        [Description] nvarchar(512) NOT NULL,
        [AverageRating_Value] float NULL,
        [AverageRating_NumRatings] int NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Menus] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[Users] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(128) NOT NULL,
        [LastName] nvarchar(128) NOT NULL,
        [Email_Value] nvarchar(256) NULL,
        [Password] nvarchar(256) NOT NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[DinnerReservations] (
        [Id] uniqueidentifier NOT NULL,
        [DinnerId] uniqueidentifier NOT NULL,
        [GuestCount] int NOT NULL,
        [GuestId] uniqueidentifier NULL,
        [BillId] uniqueidentifier NULL,
        [Status] int NULL,
        [ArrivalDateTime] datetimeoffset NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_DinnerReservations] PRIMARY KEY ([DinnerId], [Id]),
        CONSTRAINT [FK_DinnerReservations_Dinners_DinnerId] FOREIGN KEY ([DinnerId]) REFERENCES [core].[Dinners] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestBillIds] (
        [Id] int NOT NULL IDENTITY,
        [BillId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GuestBillIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuestBillIds_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestMenuReviewIds] (
        [Id] int NOT NULL IDENTITY,
        [MenuReviewId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GuestMenuReviewIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuestMenuReviewIds_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestPastDinnerIds] (
        [Id] int NOT NULL IDENTITY,
        [DinnerId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GuestPastDinnerIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuestPastDinnerIds_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestPendingDinnerIds] (
        [Id] int NOT NULL IDENTITY,
        [DinnerId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GuestPendingDinnerIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuestPendingDinnerIds_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestRatings] (
        [Id] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NULL,
        [DinnerId] uniqueidentifier NULL,
        [Rating_Value] int NULL,
        [AuditState_CreatedBy] nvarchar(256) NULL,
        [AuditState_CreatedDate] datetimeoffset NULL,
        [AuditState_CreatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedBy] nvarchar(256) NULL,
        [AuditState_UpdatedDate] datetimeoffset NULL,
        [AuditState_UpdatedDescription] nvarchar(1024) NULL,
        [AuditState_UpdatedReasons] nvarchar(max) NULL,
        [AuditState_Deactivated] bit NULL,
        [AuditState_DeactivatedReasons] nvarchar(max) NULL,
        [AuditState_DeactivatedBy] nvarchar(256) NULL,
        [AuditState_DeactivatedDate] datetimeoffset NULL,
        [AuditState_DeactivatedDescription] nvarchar(1024) NULL,
        [AuditState_Deleted] bit NULL,
        [AuditState_DeletedBy] nvarchar(256) NULL,
        [AuditState_DeletedDate] datetimeoffset NULL,
        [AuditState_DeletedReason] nvarchar(1024) NULL,
        [AuditState_DeletedDescription] nvarchar(1024) NULL,
        CONSTRAINT [PK_GuestRatings] PRIMARY KEY ([Id], [GuestId]),
        CONSTRAINT [FK_GuestRatings_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[GuestUpcomingDinnerIds] (
        [Id] int NOT NULL IDENTITY,
        [DinnerId] uniqueidentifier NOT NULL,
        [GuestId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_GuestUpcomingDinnerIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_GuestUpcomingDinnerIds_Guests_GuestId] FOREIGN KEY ([GuestId]) REFERENCES [core].[Guests] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[HostDinnerIds] (
        [Id] int NOT NULL IDENTITY,
        [HostDinnerId] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_HostDinnerIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HostDinnerIds_Hosts_HostId] FOREIGN KEY ([HostId]) REFERENCES [core].[Hosts] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[HostMenuIds] (
        [Id] int NOT NULL IDENTITY,
        [HostMenuId] uniqueidentifier NOT NULL,
        [HostId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_HostMenuIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_HostMenuIds_Hosts_HostId] FOREIGN KEY ([HostId]) REFERENCES [core].[Hosts] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[MenuDinnerIds] (
        [Id] int NOT NULL IDENTITY,
        [DinnerId] uniqueidentifier NOT NULL,
        [MenuId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MenuDinnerIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MenuDinnerIds_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [core].[Menus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[MenuReviewIds] (
        [Id] int NOT NULL IDENTITY,
        [ReviewId] uniqueidentifier NOT NULL,
        [MenuId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_MenuReviewIds] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MenuReviewIds_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [core].[Menus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[MenuSections] (
        [Id] uniqueidentifier NOT NULL,
        [MenuId] uniqueidentifier NOT NULL,
        [Name] nvarchar(128) NULL,
        [Description] nvarchar(512) NULL,
        CONSTRAINT [PK_MenuSections] PRIMARY KEY ([Id], [MenuId]),
        CONSTRAINT [FK_MenuSections_Menus_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [core].[Menus] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE TABLE [core].[MenuSectionItems] (
        [Id] uniqueidentifier NOT NULL,
        [MenuSectionId] uniqueidentifier NOT NULL,
        [MenuId] uniqueidentifier NOT NULL,
        [Name] nvarchar(128) NOT NULL,
        [Description] nvarchar(512) NOT NULL,
        CONSTRAINT [PK_MenuSectionItems] PRIMARY KEY ([Id], [MenuSectionId], [MenuId]),
        CONSTRAINT [FK_MenuSectionItems_MenuSections_MenuSectionId_MenuId] FOREIGN KEY ([MenuSectionId], [MenuId]) REFERENCES [core].[MenuSections] ([Id], [MenuId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Outbox_DomainEvents_EventId] ON [core].[__Outbox_DomainEvents] ([EventId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Outbox_DomainEvents_Type] ON [core].[__Outbox_DomainEvents] ([Type]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Outbox_Messages_MessageId] ON [core].[__Outbox_Messages] ([MessageId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Outbox_Messages_Type] ON [core].[__Outbox_Messages] ([Type]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Storage_Documents_PartitionKey] ON [core].[__Storage_Documents] ([PartitionKey]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Storage_Documents_RowKey] ON [core].[__Storage_Documents] ([RowKey]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX___Storage_Documents_Type] ON [core].[__Storage_Documents] ([Type]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestBillIds_GuestId] ON [core].[GuestBillIds] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestMenuReviewIds_GuestId] ON [core].[GuestMenuReviewIds] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestPastDinnerIds_GuestId] ON [core].[GuestPastDinnerIds] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestPendingDinnerIds_GuestId] ON [core].[GuestPendingDinnerIds] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestRatings_GuestId] ON [core].[GuestRatings] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_GuestUpcomingDinnerIds_GuestId] ON [core].[GuestUpcomingDinnerIds] ([GuestId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_HostDinnerIds_HostId] ON [core].[HostDinnerIds] ([HostId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_HostMenuIds_HostId] ON [core].[HostMenuIds] ([HostId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_MenuDinnerIds_MenuId] ON [core].[MenuDinnerIds] ([MenuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_MenuReviewIds_MenuId] ON [core].[MenuReviewIds] ([MenuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_MenuSectionItems_MenuSectionId_MenuId] ON [core].[MenuSectionItems] ([MenuSectionId], [MenuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    CREATE INDEX [IX_MenuSections_MenuId] ON [core].[MenuSections] ([MenuId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20231116134941_Initial'
)
BEGIN
    INSERT INTO [core].[__MigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231116134941_Initial', N'8.0.5');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20240110174640_Initial2'
)
BEGIN
    CREATE INDEX [IX___Outbox_Messages_CreatedDate] ON [core].[__Outbox_Messages] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20240110174640_Initial2'
)
BEGIN
    CREATE INDEX [IX___Outbox_Messages_ProcessedDate] ON [core].[__Outbox_Messages] ([ProcessedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20240110174640_Initial2'
)
BEGIN
    CREATE INDEX [IX___Outbox_DomainEvents_CreatedDate] ON [core].[__Outbox_DomainEvents] ([CreatedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20240110174640_Initial2'
)
BEGIN
    CREATE INDEX [IX___Outbox_DomainEvents_ProcessedDate] ON [core].[__Outbox_DomainEvents] ([ProcessedDate]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [core].[__MigrationsHistory]
    WHERE [MigrationId] = N'20240110174640_Initial2'
)
BEGIN
    INSERT INTO [core].[__MigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240110174640_Initial2', N'8.0.5');
END;
GO

COMMIT;
GO

