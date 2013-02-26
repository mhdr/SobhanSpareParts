CREATE TABLE [dbo].[Parts]
(
	[PartId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[Location] NVARCHAR(50) NULL, 
    [TagName] NVARCHAR(50) NULL, 
	[ResolutionPartNo] NVARCHAR(50) NULL, 
    [PartNo] NVARCHAR(50) NULL, 
    [PartName] NVARCHAR(50) NULL, 
    [PartNoOrignal] NVARCHAR(50) NULL, 
    [BrandId] INT NULL, 
    [MachineId] INT NULL, 
    [TimeStamp] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_Parts_Brands] FOREIGN KEY ([BrandId]) REFERENCES [Brands]([BrandId]), 
    CONSTRAINT [FK_Parts_Machines] FOREIGN KEY ([MachineId]) REFERENCES [Machines]([MachineId])
)

GO

CREATE INDEX [IX_Parts_PartNo] ON [dbo].[Parts] ([PartNo])

GO

CREATE INDEX [IX_Parts_ResolutionPartNo] ON [dbo].[Parts] ([ResolutionPartNo])
