﻿CREATE TABLE [dbo].[Machines]
(
	[MachineId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MachineName] NVARCHAR(50) NOT NULL, 
	[MachineCode] INT NULL,
    [TimeStamp] TIMESTAMP NOT NULL, 
)
