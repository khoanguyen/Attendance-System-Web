CREATE TABLE [dbo].[Ticket]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StudentId] INT NOT NULL, 
    [ClassId] INT NOT NULL, 
    [QrCode] VARBINARY(MAX) NOT NULL, 
    CONSTRAINT [FK_Ticket_Student] FOREIGN KEY (StudentId) REFERENCES [Student]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_Ticket_Class] FOREIGN KEY (ClassId) REFERENCES [Class]([Id]),

)
