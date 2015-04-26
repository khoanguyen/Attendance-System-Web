CREATE TABLE [dbo].[ClassSession]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[ClassId] INT NOT NULL,
	[StartTime] TIME NOT NULL,
	[EndTime] TIME NOT NULL,
	[Weekday] CHAR(3) NOT NULL, 
    [Room] NVARCHAR(64) NOT NULL, 
    CONSTRAINT [FK_ClassSession_Class] FOREIGN KEY ([ClassId]) REFERENCES [Class]([Id]) ON DELETE CASCADE, 
)
