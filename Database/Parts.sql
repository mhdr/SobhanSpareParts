CREATE TABLE [dbo].[Parts]
(
	[PartId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ResolutionPartName] NVARCHAR(50) NULL, 
    [TagName] NVARCHAR(50) NULL, 
    [PartNo] NVARCHAR(50) NOT NULL, 
    [PartName] NVARCHAR(50) NOT NULL, 
    [PartNoOrignal] NVARCHAR(50) NULL, 
    [BrandId] INT NOT NULL, 
    [MachineId] INT NOT NULL, 
    [TimeStamp] TIMESTAMP NOT NULL, 
    CONSTRAINT [FK_Parts_Brands] FOREIGN KEY ([BrandId]) REFERENCES [Brands]([BrandId]), 
    CONSTRAINT [FK_Parts_Machines] FOREIGN KEY ([MachineId]) REFERENCES [Machines]([MachineId])
)

GO

CREATE INDEX [IX_Parts_PartName] ON [dbo].[Parts] ([PartName])
