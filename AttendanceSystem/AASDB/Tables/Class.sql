CREATE TABLE [dbo].[Class]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(256) NOT NULL,
	[ProfessorName] NVARCHAR(256) NOT NULL,
	[StartDate] Date NOT NULL,
	[EndDate] Date NOT NULL, 
    [ExcusedTime] TIME NULL,
)
