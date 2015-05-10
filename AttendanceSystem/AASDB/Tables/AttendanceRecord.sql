CREATE TABLE [dbo].[AttendanceRecord]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY ,
	[StudentId] INT NOT NULL Constraint FK_AttendanceRecord_Student Foreign Key References [dbo].[Student]([Id]) ,
	[SessionId] INT NOT NULL Constraint FK_AttendanceRecord_ClassSession Foreign Key References [dbo].[ClassSession]([Id])  ON DELETE CASCADE,
	[TicketId] INT NOT NULL Constraint FK_AttendanceRecord_Tiket Foreign Key References [dbo].[Ticket]([Id]) ON DELETE CASCADE,
	[RecordDate] Date NOT NULL,
	[CheckinTime] DateTimeOffSet(7) NOT NULL
)
