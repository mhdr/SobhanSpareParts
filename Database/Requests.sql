CREATE TABLE [dbo].[Requests]
(
	[RequestId] INT NOT NULL PRIMARY KEY, 
    [PartNo] NVARCHAR(50) NULL, 
    [ResolutionPartNo] NVARCHAR(50) NULL, 
    [PartNoOrignal] NVARCHAR(50) NULL, 
    [CreationDate] DATETIME2 NOT NULL, 
    [EntranceDate] DATETIME2 NULL, 
    [TimeStamp] TIMESTAMP NOT NULL
)
