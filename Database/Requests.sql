CREATE TABLE [dbo].[Requests]
(
	[RequestId] INT NOT NULL PRIMARY KEY, 
    [PartNo] NVARCHAR(50) NULL, 
    [ResolutionPartNo] NVARCHAR(50) NULL, 
    [PartNoOrignal] NVARCHAR(50) NULL, 
	[Quantity] INT NOT NULL,
    [CreationDate] DATETIME2 NOT NULL, 
    [EntranceDate] DATETIME2 NULL, 
    [TimeStamp] TIMESTAMP NOT NULL,  
    [Description] NVARCHAR(100) NULL
)
