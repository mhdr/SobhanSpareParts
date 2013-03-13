CREATE TABLE [dbo].[Requests]
(
	[RequestId] INT NOT NULL PRIMARY KEY, 
    [ResolutionPartNo] NVARCHAR(50) NULL, 
    [PartNo] NVARCHAR(50) NULL, 
    [PartNoOriginal] NVARCHAR(50) NULL, 
    [RequestDate] DATETIME2 NOT NULL, 
    [Qty] INT NOT NULL, 
    [EntranceDate] DATETIME2 NULL, 
    [Description] NVARCHAR(50) NULL, 
    [TimeStamp] TIMESTAMP NOT NULL 
)

GO


CREATE INDEX [IX_Requests_ResolutionPartNo] ON [dbo].[Requests] ([ResolutionPartNo])

GO

CREATE INDEX [IX_Requests_PartNo] ON [dbo].[Requests] ([PartNo])

GO

CREATE INDEX [IX_Requests_PartNoOriginal] ON [dbo].[Requests] ([PartNoOriginal])
