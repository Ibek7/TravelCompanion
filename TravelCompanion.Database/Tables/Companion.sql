﻿CREATE TABLE [dbo].[Companion]
(
	[CompanionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CompanionGuid] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
	TripId INT NOT NULL FOREIGN KEY REFERENCES Trip(TripId),
	[Name] NVARCHAR(100) NOT NULL,
	CompanionNotes NVARCHAR(MAX) NULL
)
